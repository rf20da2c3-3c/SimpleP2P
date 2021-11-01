using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using P2pModels.model;

namespace P2PRegistery.managers
{
    public class RegistryManager: IRegistryManager
    {
        private static readonly Dictionary<String, HashSet<FileEndPoint>> register 
            = new Dictionary<string, HashSet<FileEndPoint>>();

        public IEnumerable<FileEndPoint> Get(string filename)
        {
            if (!register.Keys.Contains(filename))
                return new HashSet<FileEndPoint>();

            return register[filename];
        }

        public int CreateOrUpdate(string filename, FileEndPoint value)
        {
            int res = -1;

            // If no filename entries - create one 
            if (!register.Keys.Contains(filename))
            {
                register.Add(filename, new HashSet<FileEndPoint>());
            }

            // update entry with file EndPoint
            var fileset = register[filename];
            res = fileset.Add(value) ? 1 : 0;

            return res;
        }

        public int DeleteOrUpdate(string filename, FileEndPoint value)
        {
            int res = -1;

            if (!register.ContainsKey(filename))
            {
                throw new KeyNotFoundException("No file entry with name " + filename);
            }

            //update set of entries
            var fileset = register[filename];
            res = fileset.Remove(value) ? 1 : 0;

            // if no more entries - delete the entry
            if (fileset.Count == 0)
            {
                // no more entries for this filename
                register.Remove(filename);
            }

            return res;
        }



        public Dictionary<string, HashSet<FileEndPoint>> Get()
        {
            return new Dictionary<string, HashSet<FileEndPoint>>(register);
        }
    }
}
