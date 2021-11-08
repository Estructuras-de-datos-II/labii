using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using APICompress4.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Compress;
using Compression;

namespace APICompress4.Controllers
{
    [Route("api/decompress")]
    [ApiController]
    public class DescompressController : CompressIController
    {

        [HttpPost]
        public ActionResult<string> descompress([FromForm]List<IFormFile> files)
        {
            string path = Path.Combine("../..", "Archivos");
            
            byte[] fileBytes = null;
            try
            {
                IFormFile file = files[0];
                
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    fileBytes = ms.ToArray();
                    HuffmanCompression x = new HuffmanCompression();
                    fileBytes = x.Decompress(fileBytes);
                }
                double SrazonDeCompresion = Convert.ToDouble((Convert.ToDouble(fileBytes.Length) / Convert.ToDouble(file.Length)) * 100);
                double SfactorDeCompresion = Convert.ToDouble(100 / SrazonDeCompresion);
                double SporcentajeDeReduccion = Convert.ToDouble(100 - SrazonDeCompresion);
                CompressionData compress = new CompressionData(file.FileName, Path.Combine(path, file.FileName), SrazonDeCompresion, SfactorDeCompresion, SporcentajeDeReduccion);
                uploadedFiles.Add(compress);
                string fileWihoutExtension = file.FileName.Substring(0, file.FileName.IndexOf('.'));
                string newFileName = fileWihoutExtension + ".txt";
                var cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = newFileName,
                    Inline = true,
                };

                Response.Headers.Add("Content-Disposition", cd.ToString());

                return File(fileBytes, "text/plain");
            }
            catch (Exception e)
            {
                Console.WriteLine(e + "\n\n\n");
                StatusCodeResult x = new StatusCodeResult(StatusCodes.Status500InternalServerError);
                return x;
            }

            
        }

    }
}
