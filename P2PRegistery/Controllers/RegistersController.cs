using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P2pModels.model;
using P2PRegistery.managers;

namespace P2PRegistery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistersController : ControllerBase
    {
        private static readonly IRegistryManager mgr = new RegistryManager();
        
        // GET: api/Registers
        [HttpGet]
        public Dictionary<String, HashSet<FileEndPoint>> Get()
        {
            return mgr.Get();
        }

        // GET: api/Registers/peter.txt
        [HttpGet("{filename}")]
        public IEnumerable<FileEndPoint> Get(String filename)
        {
            return mgr.Get(filename);
        }

        // POST: api/Registers/peter.txt
        [HttpPost("{filename}")]
        public int Post(String filename, [FromBody] FileEndPoint value)
        {
            return mgr.CreateOrUpdate(filename, value);
        }

        // PUT: api/ApiWithActions/5
        [HttpPut("{filename}")]
        public int Put(String filename, [FromBody] FileEndPoint value)
        {
            try
            {
                return mgr.DeleteOrUpdate(filename, value);
            }
            catch (KeyNotFoundException knfe)
            {
                // return empty list
                return 0;
            }
        }
    }
}
