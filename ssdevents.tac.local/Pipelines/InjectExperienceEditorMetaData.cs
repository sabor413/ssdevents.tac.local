using System;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.GetChromeData;

namespace ssdevents.tac.local.Pipelines
{
	public class InjectExperienceEditorMetaData : GetChromeDataProcessor
	{
		public override void Process(GetChromeDataArgs args)
		{
			const string websiteSharedContentFolderItemId = "{9B2493D2-6270-4BEA-A58B-64321C1E1D2F}";
			const string globalSharedContentFolderItemId = "{0A71BD21-0EB1-4345-AC43-6FA4BDBB87B2}";

			Assert.ArgumentNotNull(args, "args");

			if (args.ChromeType.Equals("Rendering", StringComparison.InvariantCultureIgnoreCase))
			{
				var websiteSharedQuery = $"./ancestor::*[@@tid=\"{websiteSharedContentFolderItemId}\"]";
				var globalSharedQuery = $"./ancestor::*[@@id=\"{globalSharedContentFolderItemId}\"]";
				var websiteSharedContentFolder = args.Item.Axes.SelectSingleItem(websiteSharedQuery);
				var globalSharedContentFolder = args.Item.Axes.SelectSingleItem(globalSharedQuery);
				if (websiteSharedContentFolder != null)
				{
					args.ChromeData.DisplayName += " (SHARED)";
				}
				else if (globalSharedContentFolder != null)
				{
					args.ChromeData.DisplayName += " (GLOBAL SHARED)";
				}
			}
		}
	}
}