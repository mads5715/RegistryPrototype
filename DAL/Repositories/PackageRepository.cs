using RegistryPrototype.BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistryPrototype.DAL.Repositories
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1063:Implement IDisposable Correctly", Justification = "<Pending>")]
    public class PackageRepository : IRepository<MinimalPackage, string>
    {
        private readonly List<MinimalPackage> _dbOriginal;
        private List<MinimalPackage> _packages;
        public PackageRepository() {
            var objs = new GetAllRawPackagesQuery().Execute();
            _packages = objs;
            _dbOriginal = objs;

        }
        public bool DeleteElement(string input)
        {
            //This just deletes everything that has that name, and returns true if the amount is equel 1 or false if larger or smaller
            return _packages.RemoveAll(x => x._ID == input) <=1 ? true : false;
        }

        public void Dispose()
        {
           //var newPackages = _packages.Except(_dbOriginal);
           ////Insert this list into the DB
           //foreach (var item in newPackages)
           //{
           //    _ = new AddPackageCommand().Execute(item.RawMetaData);
           //}
        }

        public bool ElementExist(string input)
        {
            var dbHasIt = _dbOriginal.Find(x => x._ID == input);
            var repoHasIt = _packages.Find(x => x._ID == input);
            if (dbHasIt != null && repoHasIt != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<MinimalPackage> GetAllElements()
        {
            return _packages;
        }

        public MinimalPackage GetSingleElement(string input)
        {
            if (ElementExist(input))
            {
                return _packages.First(x => x._ID == input);
            }
            return null;
        }

        public void InsertElement(MinimalPackage element)
        {
            _packages.Add(element);
        }
       
    }
}
