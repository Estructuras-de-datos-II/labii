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
    public class compressController : ControllerBase
    {
        Encoding utf8 = Encoding.UTF8;
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
        public IActionResult PostFileCompress([FromForm] IFormFile File, [FromRoute] string name)
        {
            using var archivo = new MemoryStream();
            try
            {
                File.CopyToAsync(archivo);
                var coleccion = Encoding.UTF8.GetString(archivo.ToArray()); //pasa el texto a cadena 
                Byte[] texto_bytes = utf8.GetBytes(coleccion); // texto a bytes 
                string texto = "";
                texto = Encoding.UTF8.GetString(texto_bytes);
                Singleton.Instance.comp = new HuffmanFinal.Comprimir(texto);
                
                return Ok();


            }
            catch (Exception)
            {
                return StatusCode(500);
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
