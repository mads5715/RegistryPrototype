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
