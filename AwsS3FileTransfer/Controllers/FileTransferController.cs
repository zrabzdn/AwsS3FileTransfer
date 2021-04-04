using AwsS3FileTransfer.Interfaces;
using AwsS3FileTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AwsS3FileTransfer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileTransferController : DefaultController
    {
        private readonly IAwsS3Service _service;

        public FileTransferController(IAwsS3Service service, ILogger<AwsS3Service> logger) : base(logger)
        {
            _service = service;
        }

        [HttpPost("CreateBucket")]
        public async Task<IActionResult> CreateBucketAsync(string bucketName)
        {
            return string.IsNullOrWhiteSpace(bucketName)
                ? BadRequest("please provide bucketName")
                : await ActionAsync(async () =>
                {
                    var response = await _service.CreateBucketAsync(bucketName);
                    return StatusCode((int)response.HttpStatusCode, response);
                });
        }

        [HttpDelete("DeleteBucket")]
        public async Task<IActionResult> DeleteBucketAsync(string bucketName)
        {
            return string.IsNullOrWhiteSpace(bucketName)
                ? BadRequest("please provide bucketName")
                : await ActionAsync(async () =>
                {
                    var response = await _service.DeleteBucketAsync(bucketName);
                    return StatusCode((int)response.HttpStatusCode, response);
                });
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> UploadAsync(IFormFile file, string bucketName)
        {
            return file.Length == 0
                ? BadRequest("please provide valid file")
                : await ActionAsync(async () =>
            {
                var response = await _service.GetPutObjectResonseAsync(file, bucketName);
                return StatusCode((int)response.HttpStatusCode, response);
            });
        }

        [HttpGet("GetObjects")]
        public async Task<IActionResult> GetObjectsAsync(string bucketName)
        {
            return string.IsNullOrEmpty(bucketName)
                ? BadRequest("please provide valid bucketName name")
                : await ActionAsync(async () =>
            {
                var response = await _service.GetObjectsAsync(bucketName);
                return StatusCode((int)response.HttpStatusCode, response);
            });
        }

        [HttpDelete("Remove")]
        public async Task<IActionResult> RemoveFileAsync(string fileName, string bucketName)
        {
            return string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(bucketName)
                ? BadRequest("please provide valid file and/or valid bucketName name")
                : await ActionAsync(async () =>
            {
                var response = await _service.RemoveFileAsync(fileName, bucketName);
                return StatusCode((int)response.HttpStatusCode, response);
            });
        }
    }
}
