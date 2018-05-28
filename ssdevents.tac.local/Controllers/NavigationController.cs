using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ssdevents.tac.local.Models;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Mvc.Presentation;

namespace ssdevents.tac.local.Controllers
{
    public class NavigationController : Controller
    {
        private const string BaseNavigationGuid = "{96F5676B-4515-4046-A962-1C62253BE1AD}";
        private NavigationMenu CreateNavigationMenu(Item root, Item current)
        {
            NavigationMenu menu = new NavigationMenu()
            {
                Title = root.DisplayName,
                URL = LinkManager.GetItemUrl(root),
                Children = root.Axes.IsAncestorOf(current) ? 
                    root.GetChildren()
                        .Where(i => i["ExcludeFromNavigation"] != "1")
                        .Where(i => IsBasedOn(i, new Sitecore.Data.ID (BaseNavigationGuid)))
                        .Select(i => CreateNavigationMenu(i, current)) : null
            };

            return menu;
        }
        // GET: Navigation
        public ActionResult Index()
        {
            Item dataSourceItem = RenderingContext.Current.Rendering.Item; //datasource only
            Item currentItem = RenderingContext.Current.PageContext.Item; //current page item
            //Item currentitem = RenderingContext.Current.ContextItem;
            
            //Item section = currentitem.Axes.GetAncestors()
                //.FirstOrDefault(i => i.TemplateName == "Events Section");
            var model = CreateNavigationMenu(dataSourceItem, currentItem);
            return View(model);
        }

        public static bool IsBasedOn(Item item, Sitecore.Data.ID templateID)
        {
            var template = Sitecore.Data.Managers.TemplateManager.GetTemplate(item.TemplateID, item.Database);
            return template.DescendsFromOrEquals(templateID);
        }
    }
}