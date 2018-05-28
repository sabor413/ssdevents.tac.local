using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ssdevents.tac.local.Areas.Importer.Models;
using Newtonsoft.Json;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.SecurityModel;

namespace ssdevents.tac.local.Areas.Importer.Controllers
{
    public class EventsController : Controller
    {
        // GET: Importer/Events
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file, string parentPath)
        {
            IEnumerable<Event> events = null;
            string message = null;
            Item childItem = null;
            int updatecount = 0;
            int createcount = 0;
            using (var reader = new System.IO.StreamReader(file.InputStream))
            {
                var contents = reader.ReadToEnd();
                try
                {
                    events = JsonConvert.DeserializeObject<IEnumerable<Event>>(contents);
                }
                catch (Exception ex)
                {
                    //to be added later
                }
            }

            var database = Sitecore.Configuration.Factory.GetDatabase("master");
            var parentItem = database.GetItem(parentPath);
            var templateID = new TemplateID(new ID(Constants.Events.EventDetailsGuid));
            using (new SecurityDisabler())
            {
                foreach(var ev in events)
                {
                    var name = ItemUtil.ProposeValidItemName(ev.ContentHeading);

                    childItem = parentItem.Axes.GetChild(name);
                    if (childItem == null)
                    {
                        childItem = parentItem.Add(name, templateID);
                        createcount++;
                    } else
                    {
                        updatecount++;
                    }

                    //Item item = parentItem.Add(name, templateID);
                    childItem.Editing.BeginEdit();
                    childItem["ContentHeading"] = ev.ContentHeading;
                    childItem["ContentIntro"] = ev.ContentIntro;
                    childItem["Difficulty Level"] = ev.Difficulty.ToString();
                    childItem["Duration"] = ev.Duration.ToString();
                    childItem["Start Date"] = ev.StartDate;
                    childItem["Highlights"] = ev.Highlights;

                    childItem[FieldIDs.Workflow] = "{0D0C01D1-5200-4A35-A1E2-5C6BA89D1797}";
                    childItem[FieldIDs.WorkflowState] = "{D1E19355-942B-43B6-A0AC-9E6A289F11D8}";
                    childItem.Editing.EndEdit();
                }
            }

            ViewBag.CreateCount = createcount;
            ViewBag.UpdateCount = updatecount;
            return View();
        }
    }
}