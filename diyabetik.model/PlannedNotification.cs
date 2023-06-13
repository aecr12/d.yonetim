using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diyabetik.model
{
    public class PlannedNotification
    {   
        public string NotificationId {get; set;}
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime Date {get; set; }
        public bool State {get; set; }
    }
}