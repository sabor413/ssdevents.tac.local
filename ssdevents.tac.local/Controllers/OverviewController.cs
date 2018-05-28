using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Mvc.Presentation;
using Sitecore.Links;
using Sitecore.Web.UI.WebControls;
using System.Web.Mvc;
using ssdevents.tac.local.Models;

namespace ssdevents.tac.local.Controllers
{
    public class OverviewController : Controller
    {
        // GET: Overview
        public ActionResult Index()
        {
            var model = new OverviewList()
            {
                ReadMore = Sitecore.Globalization.Translate.Text("Read More")
            };

            model.AddRange(RenderingContext.Current.ContextItem.GetChildren()
                .Select(i => new OverviewItem()
                {
                    URL = LinkManager.GetItemUrl(i),
                    Title = new HtmlString(FieldRenderer.Render(i, "contentheading")),
                    Image = new HtmlString(FieldRenderer.Render(i, "decorationbanner", "mw=500&mh=333"))
                }
                ));

            return View(model);
        }
    }
}