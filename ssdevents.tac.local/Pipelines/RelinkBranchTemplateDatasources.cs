﻿using System;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Layouts;
using Sitecore.Pipelines.ItemProvider.AddFromTemplate;
using Sitecore.SecurityModel;

namespace ssdevents.tac.local.Pipelines
{
	public class RelinkBranchTemplateDatasources : AddFromTemplateProcessor
	{
		public override void Process(AddFromTemplateArgs args)
		{
			Assert.ArgumentNotNull(args, nameof(args));

			if (args.Destination.Database.Name != "master")
			{
				return;
			}

			var templateItem = args.Destination.Database.GetItem(args.TemplateId);
			Assert.IsNotNull(templateItem, "RelinkBranchTemplateDatasources.Process: Template does not exist " + args.TemplateId);

			// if this isn't a branch template, we can use the stock behavior
			if (templateItem.TemplateID != TemplateIDs.BranchTemplate)
			{
				return;
			}

			Assert.HasAccess(
				args.Destination.Access.CanCreate(),
				$"RelinkBranchTemplateDatasources.Process: Inadequate access for (destination: {args.Destination.ID}, template: {args.TemplateId})");

			var newItem = args.Destination.Database.Engines.DataEngine
				.AddFromTemplate(args.ItemName, args.TemplateId, args.Destination, args.NewId);

			RewriteBranchRenderingDataSources(newItem, templateItem);
			args.Result = newItem;
		}

		private static void RewriteBranchRenderingDataSources(Item item, BranchItem branchTemplateItem)
		{
			var branchBasePath = branchTemplateItem.InnerItem.Paths.FullPath;
			ApplyActionToAllRenderings(item, FieldIDs.LayoutField, branchBasePath);
			ApplyActionToAllRenderings(item, FieldIDs.FinalLayoutField, branchBasePath);
		}

		private static void ApplyActionToAllRenderings(Item item, ID fieldId, string branchBasePath)
		{
			var currentLayoutXml = LayoutField.GetFieldValue(item.Fields[fieldId]);
			if (string.IsNullOrEmpty(currentLayoutXml))
			{
				return;
			}

			var newXml = ApplyActionToLayoutXml(item, currentLayoutXml, branchBasePath);

			// save a modified layout value if necessary
			if (newXml == null)
			{
				return;
			}

			using (new SecurityDisabler())
			using (new EditContext(item))
			{
				LayoutField.SetFieldValue(item.Fields[fieldId], newXml);
			}
		}

		private static string ApplyActionToLayoutXml(Item item, string xml, string branchBasePath)
		{
			var layout = LayoutDefinition.Parse(xml);
			xml = layout.ToXml();

			for (var deviceIndex = layout.Devices.Count - 1; deviceIndex >= 0; deviceIndex--)
			{
				var device = layout.Devices[deviceIndex] as DeviceDefinition;
				if (device == null)
				{
					continue;
				}

				for (var renderingIndex = device.Renderings.Count - 1; renderingIndex >= 0; renderingIndex--)
				{
					var rendering = device.Renderings[renderingIndex];
					if (rendering is RenderingDefinition)
					{
						var renderingdefn = rendering as RenderingDefinition;
						RelinkRenderingDatasource(item, renderingdefn, branchBasePath);
					}
				}
			}

			var layoutXml = layout.ToXml();
			return layoutXml != xml ? layoutXml : null;
		}

		private static void RelinkRenderingDatasource(Item item, RenderingDefinition rendering, string branchBasePath)
		{
			if (string.IsNullOrWhiteSpace(rendering.Datasource))
			{
				return;
			}

			// note: queries and multiple item datasources are not supported
			var renderingTargetItem = item.Database.GetItem(rendering.Datasource);
			if (renderingTargetItem == null)
			{
				Log.Warn(
					$"Error while expanding branch template rendering datasources: data source {rendering.Datasource} was not resolvable",
					"RelinkBranchTemplateDatasources.RelinkRenderingDatasource");
			}

			// if there was no valid target item OR the target item is not a child of the branch template we skip out
			if (renderingTargetItem == null ||
				!renderingTargetItem.Paths.FullPath.StartsWith(branchBasePath, StringComparison.OrdinalIgnoreCase))
			{
				return;
			}

			var relativeRenderingPath = renderingTargetItem.Paths.FullPath.Substring(branchBasePath.Length).TrimStart('/');

			// we need to skip the "/$name" at the root of the branch children
			relativeRenderingPath = relativeRenderingPath.Substring(relativeRenderingPath.IndexOf('/'));

			var newTargetPath = item.Paths.FullPath + relativeRenderingPath;
			var newTargetItem = item.Database.GetItem(newTargetPath);

			// if the target item was a valid under branch item, but the same relative path does not exist under the branch instance
			// we set the datasource to something invalid to avoid any potential unintentional edits of a shared data source item
			if (newTargetItem == null)
			{
				rendering.Datasource = "INVALID_BRANCH_SUBITEM_ID";
				return;
			}

			rendering.Datasource = newTargetItem.ID.ToString();
		}
	}
}