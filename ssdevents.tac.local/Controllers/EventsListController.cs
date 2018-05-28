using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sitecore.Mvc.Presentation;
using Sitecore.ContentSearch;
using ssdevents.tac.local.Models;
using Sitecore.ContentSearch.Linq;

namespace ssdevents.tac.local.Controllers
{
    public class EventsListController : Controller
    {
        // GET: EventsList
        private const int PageSize = 4;
        public ActionResult Index(int page = 1)
        {
            var contextItem = RenderingContext.Current.ContextItem;
            var model = new EventsList();
            var databaseName = contextItem.Database.Name.ToLower();
            var indexName = string.Format("events_{0}_index", databaseName);
            var index = ContentSearchManager.GetIndex(indexName);
            using (var context = index.CreateSearchContext())
            {
                var results = context.GetQueryable<EventsDetails>()
                    .Where(i => i.Paths.Contains(contextItem.ID)
                        && i.Language == contextItem.Language.Name)
                    .Page(page - 1, PageSize)
                    .GetResults();
                model.Events = results.Hits.Select(h => h.Document).ToList();
                model.TotalResultCount = results.TotalSearchResults;
                model.PageSize = PageSize;
            }
            return View(model);
        }
    }
}