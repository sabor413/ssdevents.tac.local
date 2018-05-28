using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ssdevents.tac.local.Models
{
    public class SubscribeEmail
    {
        public SubscribeEmail() { }
        public string emailRec { get; set; }
    }

    public class SubscribeForm
    {
        public SubscribeForm() { }
        public HtmlString Heading { get; set; }
        public HtmlString Intro { get; set; }
        public HtmlString ButtonText { get; set; }
    }
}