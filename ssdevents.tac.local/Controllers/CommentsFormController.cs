using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAC.Utils.Mvc;

namespace ssdevents.tac.local.Controllers
{
    public class CommentsFormController : Controller
    {
        // GET: CommentsForm
        public ActionResult Index()
        {
            return View();
        }

        [ValidateFormHandler]
        [HttpPost]
        public ActionResult Index(string comment, string name)
        {
            return View("Confirmation");
        }

        public ActionResult IndexJS()
        {
            return View();
        }

        [ValidateFormHandler]
        [HttpPost]
        public ActionResult IndexJS(string comment, string name)
        {
            return View("Confirmation");
        }
    }
}