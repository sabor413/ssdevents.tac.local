using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Web.UI.WebControls;

namespace ssdevents.tac.local.Models
{
    public class EventsDetails : SearchResultItem
    {
        public EventsDetails() { }
        public string ContentHeading { get; set; }
        public string ContentIntro { get; set; }
        [Sitecore.ContentSearch.IndexField("Start Date")]
        public DateTime EventStartDate { get; set; }
        public HtmlString EventImage {
            get { return new HtmlString(FieldRenderer.Render(GetItem(), "Event Image", "DisableWebEditing=true&mw=150")); }
        }
    }
}