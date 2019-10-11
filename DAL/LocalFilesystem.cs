using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RegistryPrototype.DAL
{
    public class LocalFilesystemRegistry
    {
        public void SaveFile(string filename,byte[] data) {
            File.WriteAllBytes(filename, data);
        }
    }
}
