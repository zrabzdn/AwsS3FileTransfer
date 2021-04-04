using Amazon.S3;
using Amazon.S3.Model;
using AwsS3FileTransfer.Interfaces;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace AwsS3FileTransfer.Models
{
    public class AwsS3Service : IAwsS3Service
    {
        private readonly IAmazonS3 _client;    
        private readonly AwsS3BucketOptions _bucketOptions;

        public AwsS3Service(IAmazonS3 client, AwsS3BucketOptions bucketOptions)
        {
            _client = client;
            _bucketOptions = bucketOptions;
        }

        public async Task<PutBucketResponse> CreateBucketAsync(string bucketName)
        {
            PutBucketRequest request = new PutBucketRequest();
            request.BucketName = bucketName;
            return await _client.PutBucketAsync(request);
        }

        public async Task<DeleteBucketResponse> DeleteBucketAsync(string bucketName)
        {
            DeleteBucketRequest request = new DeleteBucketRequest();
            request.BucketName = bucketName;
            return await _client.DeleteBucketAsync(request);
        }

        public async Task<PutObjectResponse> GetPutObjectResonseAsync(IFormFile file, string bucketName)
        {
            using var fileStream = file.OpenReadStream();
            using var ms = new MemoryStream();
            await fileStream.CopyToAsync(ms);
            PutObjectRequest request = new PutObjectRequest
            {
                BucketName = bucketName ?? _bucketOptions.BucketName,
                Key = file.FileName
            };
            request.InputStream = fileStream;
            return await _client.PutObjectAsync(request);
        }

        public async Task<ListObjectsResponse> GetObjectsAsync(string bucketName)
        {
            ListObjectsRequest request = new ListObjectsRequest();
            request.BucketName = bucketName;
            return await _client.ListObjectsAsync(request);
        }

        public async Task<DeleteObjectResponse> RemoveFileAsync(string file, string bucketName)
        {
            DeleteObjectRequest request = new DeleteObjectRequest();
            request.BucketName = bucketName;
            request.Key = file;
            return await _client.DeleteObjectAsync(request);
        }
    }
}
