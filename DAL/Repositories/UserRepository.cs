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
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
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
