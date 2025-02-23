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

        public static void SaveEvent(Event e) 
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("insert into Event(ID, Date, EventName) values (@Id, @EventDate, @EventName)", e);
            }
        }

        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}
