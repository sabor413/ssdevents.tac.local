using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ssdevents.tac.local.Models
{
    public class OverviewItem
    {
        public OverviewItem() { }
        public HtmlString Title { get; set; }
        public HtmlString Image { get; set; }
        public string URL { get; set; }
    }
}