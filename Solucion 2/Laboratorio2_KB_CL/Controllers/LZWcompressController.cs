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
    public class LZWcompressController : ControllerBase
    {
        Encoding utf8 = Encoding.UTF8;
        // GET: api/<LZWcompressController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<LZWcompressController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<LZWcompressController>
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

                
                string textocomprimido = Singleton.Instance.lzw.CompressText(textoparacomprimir);
               
                





                // Set a variable to the Documents path.
                string docPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                // Write the string array to a new file named "WriteLines.txt".
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, nombrearchivofinal + ".lzw")))
                {

                    outputFile.Write(textocomprimido);
                }

                //double SrazonDeCompresion = Convert.ToDouble((Convert.ToDouble(Singleton.Instance.comp.contadorFinal) / Convert.ToDouble(textomientras.Length)) * 100);
                //double SfactorDeCompresion = Convert.ToDouble(100 / SrazonDeCompresion);
                //double SporcentajeDeReduccion = Convert.ToDouble(100 - SrazonDeCompresion);
                //Historial objHistorial = new Historial()
                //{
                //    nombre = "Nombre del archivo: " + nombrearchivofinal,
                //    nombreYruta = "Nombre del archivo final: " + nombrearchivofinal + ".huff" + " Ruta: desktop",
                //    razonDeCompresion = "Razón de compresión: " + SrazonDeCompresion + "%",
                //    factorDeCompresion = "Factor de compresión: " + SfactorDeCompresion,
                //    porcentajeDeReduccion = "Porcentaje de reducción: " + SporcentajeDeReduccion + "%",
                //};
                //Singleton.Instance.historial.Add(objHistorial);
                return Created("", textocomprimido);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

        }

        // PUT api/<LZWcompressController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<LZWcompressController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
