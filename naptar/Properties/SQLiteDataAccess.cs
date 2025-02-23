using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace naptar
{
    public class SQLiteDataAccess
    {
        public static List<Event> LoadEvents()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<Event>("select * from Event", new DynamicParameters());
                return output.ToList();
            }
        }

        public static List<Event> LoadEventsPartialParameters()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<Event>("select EventName, Date as EventDate from Event", new DynamicParameters());
                return output.ToList();
            }
        }

        public static void SaveEvent(Event e) 
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("insert into Event(\"Date\", \"Time\", \"EventName\") values (@EventDate, @EventTime, @EventName)", e);
            }
        }

        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        public static string LoadEventByDay(List<Event> events, string currentDate)
        {

            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                // Paraméter helyes átadása objektumként
                var output = cnn.Query<Event>("select Time as EventTime, EventName from Event where Date = @EventDate", new { EventDate = currentDate });

                // Stringgé alakítás
                string result = string.Join("\n", output.Select(ev => $"{ev.EventTime}:00 - {ev.EventName}"));
                return result;
            }

        }
        public static void DeleteOldEvents(string oldDate)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("delete from Event where Date <= @oldDate", new { oldDate });
            }
        }

        public static void DeleteManually(string name, string date)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("delete from Event where EventName = @EventName and Date = @EventDate", new { EventName = name, EventDate = date });
            }
        }
    }
}
