using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using APICompress4.Model;
using Compress;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APICompress4.Controllers
{
    [Route("api/lzwcompress")]
    [ApiController]
    public class LZWCompressController : CompressIController
    {
        [HttpPost("{name}")]
        public ActionResult<string> compress([FromForm]List<IFormFile> files, string name)
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
                    fileBytes = LZWCompresscs.LzwCompress(fileBytes);
                }
                saveFileAfter(fileBytes, path, name + ".lzw");

                CompressionData compress = new CompressionData(file.FileName, Path.Combine(path, newFileName), 1, 2, fileBytes.Length / file.Length);
                uploadedFiles.Add(compress);

            }
            catch (Exception e)
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