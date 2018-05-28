using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ssdevents.tac.local.Models;
using Sitecore.Mvc.Presentation;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Web.UI.WebControls;

namespace ssdevents.tac.local.Controllers
{
    public class BreadcrumbController : Controller
    {
        private IEnumerable<NavigationItem> CreateModel()
        {
            var dataSourceItem = RenderingContext.Current.Rendering.Item;
            var currentItem = RenderingContext.Current.PageContext.Item;
            var homeItem = Sitecore.Context.Database.GetItem(Sitecore.Context.Site.StartPath);
            Item topItem;

            if (dataSourceItem.ID == currentItem.ID)
            {
                topItem = homeItem;
            }
            else
            {
                topItem = dataSourceItem;
            }

            var breadcrumb = RenderingContext.Current.ContextItem.Axes.GetAncestors()
                .Where(i => i.Axes.IsDescendantOf(topItem))
                .Concat(new Item[] { currentItem })
                .ToList();

            IEnumerable<NavigationItem> NavigationList = breadcrumb.Select(s => new NavigationItem
            {
                Title = s.DisplayName,
                URL = LinkManager.GetItemUrl(s),
                Active = (s.ID == currentItem.ID)
            });

            return NavigationList;
        }

        // GET: Breadcrumb
        public ActionResult Index()
        {
            return View(CreateModel());
        }
    }
}