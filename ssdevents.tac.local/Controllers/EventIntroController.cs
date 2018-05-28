using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ssdevents.tac.local.Models;
using Sitecore.Mvc.Presentation;
using Sitecore.Web.UI.WebControls;

namespace ssdevents.tac.local.Controllers
{
    public class EventIntroController : Controller
    {
        private static EventIntro CreateModel()
        {
            //var item = RenderingContext.Current.ContextItem;
	        var item = RenderingContext.Current.Rendering.Item;

            // for DEMO
            var colorParameter = RenderingContext.Current.Rendering.Parameters["Color"];
            // end DEMO

            var eventIntro = new EventIntro()
            {
                Heading = new HtmlString(FieldRenderer.Render(item, "ContentHeading")),
                EventImage = new HtmlString(FieldRenderer.Render(item, "Event Image", "mw=1000&mh=568&class=img-responsive")),
                Highlights = new HtmlString(FieldRenderer.Render(item, "Highlights")),
                Intro = new HtmlString(FieldRenderer.Render(item, "ContentIntro")),
                StartDate = new HtmlString(FieldRenderer.Render(item, "Start Date")),
                Duration = new HtmlString(FieldRenderer.Render(item, "Duration")),
                Difficulty = new HtmlString(FieldRenderer.Render(item, "Difficulty")),
                
                //for DEMO
                Color = !string.IsNullOrEmpty(colorParameter) ? colorParameter : "lightgrey"
                //end DEMO
            };
            return eventIntro;
        }
        // GET: EventIntro
        public ActionResult Index()
        {
            return View(CreateModel());
        }

        public ActionResult Demo()
        {
            return View(CreateModel());
        }
    }
}