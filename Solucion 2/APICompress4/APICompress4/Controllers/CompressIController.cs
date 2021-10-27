using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APICompress4.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APICompress4.Controllers
{
    [Route("api/compressions")]
    [ApiController]
    public class CompressIController : ControllerBase
    {
        public static List<Compression> uploadedFiles = new List<Compression>();

        [HttpGet]
        public ActionResult<List<Compression>> compressions()
        {
            return uploadedFiles;
        }
    }
}