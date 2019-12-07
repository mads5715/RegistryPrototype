using Dapper;
using MySql.Data.MySqlClient;
using RegistryPrototype.BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistryPrototype.DAL.Queries
{
    public class GetAllUsersQuery : IQuery<List<User>>
    {
        public List<User> Execute()
        {
            var connectionString = ServerStrings.GetMySQLConnectionString();
            var _lists = new List<User>();
            using (var conn = new MySqlConnection(connectionString))
            {
                _lists = conn.Query<User>("SELECT Email,UserName AS Name,UserPassword as Password,(SELECT Name FROM UserTypes WHERE ID = Users.UserType) AS UserType FROM Users").ToList();
                return _lists;
            }
        }
    }
}
