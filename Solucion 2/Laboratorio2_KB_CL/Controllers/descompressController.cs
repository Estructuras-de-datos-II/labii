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
    public class descompressController : ControllerBase
    {
        Encoding utf8 = Encoding.UTF8;
        // GET: api/<descompressController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<descompressController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<descompressController>
        [HttpPost]
        public IActionResult PostFileCompress([FromForm] IFormFile file, [FromRoute] string name)
        {
            using var archivotexto = new MemoryStream();
            try
            {

                string nombrearchiv = file.FileName;
                string nombrearchivofinal = nombrearchiv.Split(".").First();
                file.CopyToAsync(archivotexto);
                var textomientras = Encoding.UTF8.GetString(archivotexto.ToArray());


                Byte[] stringbytes = utf8.GetBytes(textomientras);


                string textoparacomprimir = "";


                textoparacomprimir = Encoding.UTF8.GetString(stringbytes);

                Singleton.Instance.descomp = new HuffmanFinal.Descomprimir(textoparacomprimir);
                string textodescomprimido = Singleton.Instance.descomp.mensajeDescomprimido;





                // Set a variable to the Documents path.
                string docPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                // Write the string array to a new file named "WriteLines.txt".
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, nombrearchivofinal + ".txt")))
                {

                    outputFile.Write(textodescomprimido);
                }
                return Created("", textodescomprimido);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

        }

        // PUT api/<descompressController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<descompressController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
