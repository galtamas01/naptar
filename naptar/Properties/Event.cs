using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace naptar
{
    public class Event
    {
        //int id;
        string eventDate;
        int eventTime;
        string eventName;

        //public int Id { get => id; set => id = value; }
        public string EventDate { get => eventDate; set => eventDate = value; }
        public int EventTime { get => eventTime; set => eventTime = value; }
        public string EventName { get => eventName; set => eventName = value; }
    }
}
