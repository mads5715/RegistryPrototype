using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistryPrototype.DAL
{
    public interface IRepository<T,Y> : IDisposable
    {
        List<T> GetAllElements();
        T GetSingleElement(Y input);
        void InsertElement(T element);
        bool DeleteElement(Y input);
        bool ElementExist(Y input);
    }
}
