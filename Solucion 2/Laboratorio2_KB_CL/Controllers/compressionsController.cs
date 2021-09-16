using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Laboratorio2_KB_CL.Models;
using Laboratorio2_KB_CL.Data;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Laboratorio2_KB_CL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class compressionsController : ControllerBase
    {
        // GET: api/<compressionsController>
        [HttpGet]
        public IEnumerable<Historial> Compresionesvergas()
        {
            return Singleton.Instance.historial;

        }

        // GET api/<compressionsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<compressionsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<compressionsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<compressionsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
