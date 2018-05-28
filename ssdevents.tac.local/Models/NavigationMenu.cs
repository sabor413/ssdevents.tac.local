using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ssdevents.tac.local.Models
{
    public class NavigationMenu : NavigationItem
    {
        public NavigationMenu() { }
        public IEnumerable<NavigationMenu> Children { get; set; }
    }
}