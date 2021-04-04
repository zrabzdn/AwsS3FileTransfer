using Amazon.S3;
using AwsS3FileTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AwsS3FileTransfer.Controllers
{
    public class DefaultController : ControllerBase
    {
        private readonly ILogger<AwsS3Service> _logger;

        public DefaultController(ILogger<AwsS3Service> logger)
        {
            _logger = logger;
        }

        protected async Task<IActionResult> ActionAsync(Func<Task<IActionResult>> func)
        {
            try
            {
                var result = await func().ConfigureAwait(false);
                return Ok(result);
            }
            catch (AmazonS3Exception e) when ((e.ErrorCode?.Equals("InvalidAccessKeyId") == true)
                || e.ErrorCode.Equals("InvalidSecurity"))
            {
                _logger.LogError("Please check the provided AWS Credentials.");
                return StatusCode((int)e.StatusCode, e.Message);
            }
            catch (AmazonS3Exception e)
            {
                _logger.LogError($"An error occurred with the message '{ e.Message }'");
                return StatusCode((int)e.StatusCode, e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"Unknown encountered on server. Message:'{ e.Message }'");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"{ AppDomain.CurrentDomain.FriendlyName } : { e.Message }");
            }
        }
    }
}
