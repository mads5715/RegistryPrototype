using Dapper;
using MySql.Data.MySqlClient;
using RegistryPrototype.BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistryPrototype.DAL
{
    public class GetAllRawPackagesQuery : IQuery<List<MinimalPackage>>
    {
        public List<MinimalPackage> Execute()
        {
            var connectionString = "server = 192.168.0.18; user id = RegistryClone; password = RegistryClone2019; port = 3306; database = NPMRegistryClone;";
            var _lists = new List<MinimalPackage>();
            using (var conn = new MySqlConnection(connectionString))
            {
                _lists = conn.Query<MinimalPackage>("SELECT * FROM Packages").ToList();
                return _lists;
            }
        }
    }
}
