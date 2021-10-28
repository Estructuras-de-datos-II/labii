﻿using System;
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
    [Route("api/lzwdescompress")]
    [ApiController]
    public class LZWDescompressController : CompressIController
    {
        [HttpPost]
        public ActionResult<string> descompress([FromForm]List<IFormFile> files)
        {
            string path = Path.Combine("../..", "Archivos");
            string newFileName = "f";
            try
            {
                IFormFile file = files[0];
                saveFile(file, path);
                //compression
                byte[] fileBytes = null;
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    fileBytes = ms.ToArray();
                    fileBytes = LZWCompresscs.LzwDecompress(fileBytes);
                }
                newFileName = file.FileName.Substring(0, file.FileName.IndexOf('.')) + ".txt";
                saveFileAfter(fileBytes, path, newFileName);

                CompressionData compress = new CompressionData(file.FileName, Path.Combine(path, newFileName), 1, 2, fileBytes.Length / file.Length);
                uploadedFiles.Add(compress);
            }
            catch (Exception e)
            {
                Console.WriteLine(e + "\n\n\n");
                StatusCodeResult x = new StatusCodeResult(StatusCodes.Status500InternalServerError);
                return x;
            }

            return newFileName; 
        }

        private string saveFile(IFormFile file, string path)
        {
            string fileName = Path.GetFileName(file.FileName);
            using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return Path.Combine(path, fileName);
        }

        private void saveFileAfter(byte[] fileData, string path, string fileName)
        {
            using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
            {
                stream.Write(fileData, 0, fileData.Length);
            }
        }
    }
}