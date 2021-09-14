using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laboratorio2_KB_CL.Models;
using Laboratorio2_KB_CL.Data;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Laboratorio2_KB_CL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class compressController : ControllerBase
    {
        // GET: api/<compressController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<compressController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<compressController>
        [HttpPost]
        public void Post([FromBody] compress newValue)
        {
            try
            {
                Singleton.Instance.textoparacomprimir = Convert.ToString(newValue.textoparacomprimir);
                

            }
            catch (Exception ex)
            {
                
            }
        }

        // PUT api/<compressController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<compressController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
