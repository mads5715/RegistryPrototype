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
using RegistryPrototype.BE;
using RegistryPrototype.DAL.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistryPrototype.DAL.Repositories
{
    public sealed class UserRepository : IRepository<User, string>
    {
        private readonly List<User> _users;

        public UserRepository() {
            _users = new GetAllUsersQuery().Execute();
        }

        public bool DeleteElement(string input)
        {
            return _users.RemoveAll(x => x.Name == input) <= 1 ? true : false;
        }

        public void Dispose()
        {
            return;
        }

        public bool ElementExist(string input)
        {
            return _users.Find(x => x.Name == input) != null? true : false;
        }

        public List<User> GetAllElements()
        {
            return _users;
        }

        public User GetSingleElement(string input)
        {
            return _users.Find(x => x.Name == input);
        }

        public void InsertElement(User element)
        {
            _users.Add(element);
        }
    }
}
