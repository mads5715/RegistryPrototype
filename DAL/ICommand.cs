using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistryPrototype.DAL
{
    public interface ICommand<T>
    {
        string Execute(T input);
    }
}
