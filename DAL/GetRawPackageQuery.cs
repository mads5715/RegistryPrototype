using Dapper;
using MySql.Data.MySqlClient;
using RegistryPrototype.BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistryPrototype.DAL
{
    public class GetRawPackageQuery : IQuery<string, string>
    {
        public string Execute(string input)
        {
            var connectionString = "server = 192.168.0.18; user id = RegistryClone; password = RegistryClone2019; port = 3306; database = NPMRegistryClone;";
            var _lists = new List<RetreivalObject>();
            using (var conn = new MySqlConnection(connectionString))
            {
                _lists = conn.Query<RetreivalObject>("SELECT RawMetaData FROM Packages WHERE _ID LIKE @id", new { id = input }).ToList();
                return _lists.FirstOrDefault().RawMetaData;
            }

        }
    }
}
