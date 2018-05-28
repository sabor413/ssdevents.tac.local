using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ssdevents.tac.local.Models
{
    public class EventsList
    {
        public EventsList() { }
        public IEnumerable<EventsDetails> Events { get; set; }
        public int TotalResultCount { get; set; }
        public int PageSize { get; set; }
    }
}