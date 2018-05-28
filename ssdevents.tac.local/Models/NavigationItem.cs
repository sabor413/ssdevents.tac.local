using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ssdevents.tac.local.Models
{
    public class NavigationItem
    {
        public NavigationItem() { }
        public string Title { get; set; }
        public string URL { get; set; }
        public bool Active { get; set; }
    }
}