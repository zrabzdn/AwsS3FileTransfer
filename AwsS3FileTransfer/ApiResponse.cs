using System.Net;

namespace AwsS3FileTransfer
{
    public class ApiResponse<T>
    {
        public HttpStatusCode StatusCode { get; set; }

        public T Data { get; set; }
    }
}
