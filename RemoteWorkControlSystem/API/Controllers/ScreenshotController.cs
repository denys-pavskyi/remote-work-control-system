using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
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


        // POST api/Screenshot/
        [HttpPost("Screenshot/")]
        public async Task<ActionResult> UploadScreenshot(IFormFile file)
        {
            BlobContainerClient blobContainerClient = new BlobContainerClient(_screenshotPath, "bloprwcs");

            
            using ( var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                stream.Position = 0;
                try
                {
                    await blobContainerClient.UploadBlobAsync(file.FileName, stream);

                }
                catch(Exception ex)
                {
                    
                    return BadRequest();
                }
                
            }
            
            return Ok("File uploaded successfully");
        }


        // GET api/Screenshot/fromFolder/{folderName}
        [HttpGet("Screenshot/fromFolder/{folderName}")]
        public async Task<List<string>> GetURLsFromFolder(string folderName)
        {
            List<string> urls = new List<string>();
            string connectionString = _screenshotPath;
            string containerName = "bloprwcs";
            string folderPath = folderName; // E.g., "images/"
            string fileExtension = ".png"; // Specify the desired file extension

            // Create a BlobServiceClient object by passing the connection string
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            // Get a reference to the container
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            // List all blobs in the container with the specified folder path
            await foreach (BlobItem blobItem in containerClient.GetBlobsAsync(prefix: folderPath))
            {
                // Check if the blob is an image based on the file extension
                if (blobItem.Name.EndsWith(fileExtension, StringComparison.OrdinalIgnoreCase))
                {
                    // Get the URL of the image blob
                    BlobClient blobClient = containerClient.GetBlobClient(blobItem.Name);
                    Uri blobUri = blobClient.Uri;
                    urls.Add(blobUri.ToString());
                }
            }

            return urls;
        }


    }
}
