using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using P2pModels.model;

namespace P2PRegistery.managers
{
    public interface IRegistryManager
    {
        IEnumerable<FileEndPoint> Get(String filename);
        int CreateOrUpdate(String filename, FileEndPoint value);
        int DeleteOrUpdate(String filename, FileEndPoint value);

        // for other purposes
        Dictionary<String, HashSet<FileEndPoint>> Get();
    }
}
