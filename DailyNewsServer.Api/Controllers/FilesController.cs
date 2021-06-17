using DailyNewsServer.Core.Interfaces;
using DailyNewsServer.Core.Models.Communication.Files;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyNewsServer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private IFilesRepository _filesRepository;

        public FilesController(IFilesRepository filesRepository)
        {
            _filesRepository = filesRepository;
        }

        [HttpPost]
        [Route("{bucketName}/add")]
        public async Task<ActionResult<AddFileResponse>>AddFiles(string bucketName, IList<IFormFile> formFiles)
        {
            if(formFiles == null)
            {
                return BadRequest("The request doesnt contain any files to be uploaded.");
            }

            var response = await _filesRepository.UploadFiles(bucketName, formFiles);

            if(response == null)
            {
                return BadRequest();
            }

            return Ok(response);
        }
    }
}
