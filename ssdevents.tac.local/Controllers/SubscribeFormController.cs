using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Analytics.Model.Entities;
using Sitecore.Mvc.Presentation;
using Sitecore.Web.UI.WebControls;
using Sitecore.Analytics.Outcome.Extensions;
using System.Web;
using System.Web.Mvc;
using TAC.Utils.Mvc;
using ssdevents.tac.local.Models;

namespace ssdevents.tac.local.Controllers
{
    public class SubscribeFormController : Controller
    {
        // GET: SubscribeForm
        public ActionResult Index()
        {
            var item = RenderingContext.Current.Rendering.Item;
            var subscribeForm = new SubscribeForm()
            {
                Heading = new HtmlString(FieldRenderer.Render(item, "ContentHeading")),
                Intro = new HtmlString(FieldRenderer.Render(item, "ContentIntro")),
                ButtonText = new HtmlString(FieldRenderer.Render(item, "ButtonText"))
            };
            return View(subscribeForm);
        }

        [ValidateFormHandler, HttpPost]
        public ActionResult Index(string email)
        {
            Sitecore.Analytics.Tracker.Current.Session.Identify(email);
            var contact = Sitecore.Analytics.Tracker.Current.Contact;
            var emails = contact.GetFacet<IContactEmailAddresses>("Emails");
            if (!emails.Entries.Contains("personal"))
            {
                emails.Preferred = "personal";
                var personalEmail = emails.Entries.Create("personal");
                personalEmail.SmtpAddress = email;
            }

            var outcome = new Sitecore.Analytics.Outcome.Model.ContactOutcome(Sitecore.Data.ID.NewID, new Sitecore.Data.ID("{322343ED-74CC-4C2E-9ECF-6E6596E20AE4}"), new Sitecore.Data.ID(Sitecore.Analytics.Tracker.Current.Contact.ContactId));

            Sitecore.Analytics.Tracker.Current.RegisterContactOutcome(outcome);

            var subscribeEmail = new SubscribeEmail(){ emailRec = email };
            return View("Confirmation",subscribeEmail);
        }
    }
}