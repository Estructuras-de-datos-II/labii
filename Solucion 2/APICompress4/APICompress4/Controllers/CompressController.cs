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
            string newFileName = "f";
            IFormFile file = files[0];
            try
            {
                
                byte[] fileBytes = null;
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    fileBytes = ms.ToArray();
                    HuffmanCompression x = new HuffmanCompression();
                    fileBytes = x.Compress(fileBytes);
                }
                saveFileAfter(fileBytes, path, name + ".huff");

                double SrazonDeCompresion = Convert.ToDouble((Convert.ToDouble(fileBytes.Length) / Convert.ToDouble(file.Length)) * 100);
                double SfactorDeCompresion = Convert.ToDouble(100 / SrazonDeCompresion);
                double SporcentajeDeReduccion = Convert.ToDouble(100 - SrazonDeCompresion);

                CompressionData compress = new CompressionData(file.FileName, Path.Combine(path, newFileName), SrazonDeCompresion, SfactorDeCompresion, SporcentajeDeReduccion);

            }
            catch(Exception e)
            {
                StatusCodeResult x = new StatusCodeResult(StatusCodes.Status500InternalServerError);
                return x;
            }

            return file.FileName;
        }

        private string saveFile(IFormFile file, string path)
        {
            string fileName = Path.GetFileName(file.FileName);
            using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
            {
                file.CopyTo(stream);
                //uploadedFiles.Add(fileName);
            }
            return Path.Combine(path, fileName);
        }

        private void saveFileAfter(byte[] fileData, string path, string fileName)
        {
            using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
            {
                stream.Write(fileData, 0, fileData.Length);
                //uploadedFiles.Add(fileName);
            }
        }
    }
}