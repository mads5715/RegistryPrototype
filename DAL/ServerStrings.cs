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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistryPrototype.DAL
{
    public static class ServerStrings
    {
        /// <summary>
        /// Returns the connection string a connection string for the Docker-compose MySQL, 
        /// or if one is specified in the environment with ConnectionString: server=...;userid+... etc. this will take priority.
        /// </summary>
        /// <returns>string: connectionstring</returns>
        public static string GetMySQLConnectionString() {
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("ConnectionString"))) {
                return "server = mysqlServer; user id = RegistryClone; password = RegistryClone2019; port = 3306; database = NPMRegistryClone;";
            }
            else {
                return Environment.GetEnvironmentVariable("ConnectionString");
            }
        }

        public static string GetString(string stringName) {
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(stringName))){
                return ""; //User Cryptographic Random for the salt
            } else {
                return Environment.GetEnvironmentVariable(stringName);
            }
        }
    }
}
