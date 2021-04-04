using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AwsS3FileTransfer.Interfaces
{
    public interface IAwsS3Service
    {
        Task<PutBucketResponse> CreateBucketAsync(string bucketName);
        Task<DeleteBucketResponse> DeleteBucketAsync(string bucketName);
        Task<PutObjectResponse> GetPutObjectResonseAsync(IFormFile file, string bucketName);
        Task<ListObjectsResponse> GetObjectsAsync(string bucketName);        
        Task<DeleteObjectResponse> RemoveFileAsync(string file, string bucketName);
    }
}
