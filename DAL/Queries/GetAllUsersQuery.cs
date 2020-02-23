/*
    Copyright (C) 2019  Mads Dürr-Wium

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/
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
