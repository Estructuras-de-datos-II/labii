using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using APICompress4.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Compression;

namespace APICompress4.Controllers
{
    [Route("api/compress")]
    [ApiController]
    public class CompressController : CompressIController
    {
        
        

        [HttpPost("{name}")]
        public ActionResult<string> compress ([FromForm]List<IFormFile> files, string name )
        {
            string path = Path.Combine("../..", "Archivos");
            
            IFormFile file = files[0];
            byte[] fileBytes = null;
            try
            {
                
                
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    fileBytes = ms.ToArray();
                    HuffmanCompression x = new HuffmanCompression();
                    fileBytes = x.Compress(fileBytes);
                }
                double SrazonDeCompresion = Convert.ToDouble((Convert.ToDouble(fileBytes.Length) / Convert.ToDouble(file.Length)) * 100);
                double SfactorDeCompresion = Convert.ToDouble(100 / SrazonDeCompresion);
                double SporcentajeDeReduccion = Convert.ToDouble(100 - SrazonDeCompresion);
                CompressionData compress = new CompressionData(file.FileName, Path.Combine(path, file.FileName), SrazonDeCompresion, SfactorDeCompresion, SporcentajeDeReduccion);
                uploadedFiles.Add(compress);

            }
            catch(Exception e)
            {
                StatusCodeResult x = new StatusCodeResult(StatusCodes.Status500InternalServerError);
                return x;
            }
            string newFileName = name + ".huff";
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = newFileName,
                Inline = true,
            };

            Response.Headers.Add("Content-Disposition", cd.ToString());

            return File(fileBytes, "text/plain");
        }
    }
}