using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ScreenshotController : ControllerBase
    {
        private string _screenshotPath = "DefaultEndpointsProtocol=https;AccountName=rwcsblopcontainer;AccountKey=HJ+6AUcDqM3cAIU4q5mdGefzzNMlYicVHkq4mQqTX4sOvDa9eQdjjd7PyeyDbpuhSb7cetAmAedt+ASty3fwpA==;EndpointSuffix=core.windows.net";

        public ScreenshotController()
        {
            
        }


        // POST api/Screenshot/{id}
        [HttpPost("Screenshot/{id}")]
        public async Task<ActionResult> UploadScreenshot(IFormFile file)
        {
            BlobContainerClient blobContainerClient = new BlobContainerClient(_screenshotPath, "bloprwcs");
            
            using( var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                stream.Position = 0;
                await blobContainerClient.UploadBlobAsync(file.FileName, stream);
            }
            
            return Ok("File uploaded successfully");
        }

    }
}
