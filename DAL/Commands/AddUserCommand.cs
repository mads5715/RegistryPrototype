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

namespace RegistryPrototype.DAL.Commands
{
    public class AddUserCommand : ICommand<User>
    {
        public string Execute(User input)
        {
            var connectionString = ServerStrings.GetMySQLConnectionString();
            using (var conn = new MySqlConnection(connectionString))
            {
                var result = conn.Execute("INSERT IGNORE INTO Users (UserName,DisplayName,UserPassword,UserType,OrgDepartment,Email)" +
                    " VALUES (@userName,@dispName,@passwrd,(SELECT ID FROM UserTypes WHERE UserTypes.Name LIKE @userTypeName),(SELECT ID FROM OrgDepartments WHERE OrgDepartments.Name LIKE 'MDW Computing'), @userEmail)",
                   new
                   {
                       userName = input.Name,
                       dispName = input.Name,
                       passwrd = input.Password,
                       userTypeName = input.Type,
                       userEmail = input.Email

                   });
            }
            return input.Name;
        }
    }
}
