using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using IdentityService.Domain.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace IdentityService.Infrastructure.Utilities;
public class AwsStorage
{
    private readonly IConfiguration _configuration;
    private readonly IAmazonS3 _client;

    public AwsStorage(IConfiguration configuration)
    {
        _configuration = configuration;
        _client = new AmazonS3Client(_configuration["AwsS3:AccessKeyId"],
            _configuration["AwsS3:SecretAccessKey"],
            RegionEndpoint.GetBySystemName(_configuration["AwsS3:Region"]));
    }

    public async Task<AwsResponse> UploadFileAsync(IFormFile file)
    {
        var name = "Transactions_" + Guid.NewGuid() + Path.GetExtension(file.FileName);
        try
        {
            await using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var uploadRequest = new TransferUtilityUploadRequest { InputStream = memoryStream, Key = name, BucketName = _configuration["AwsS3:BucketName"] };
            using var fileTransferUtility = new TransferUtility(_client);
            await fileTransferUtility.UploadAsync(uploadRequest);
            return new AwsResponse { Error = false, FileID = uploadRequest.Key };
        }
        catch (Exception ex)
        {
            return new AwsResponse { Error = false, FileID = name, Message = ex.Message };
        }
    }

    // new function for all files in one bucket
    public async Task<AwsResponse> UploadFileAsync(IFormFile file, string directory)
    {
        var name = $"{directory}/{Guid.NewGuid()}_{file.FileName}";
        var bucketName = _configuration["AwsS3:Bucket"];
        try
        {
            await using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var uploadRequest = new TransferUtilityUploadRequest { InputStream = memoryStream, Key = name, BucketName = bucketName, CannedACL = S3CannedACL.Private };
            using var fileTransferUtility = new TransferUtility(_client);
            await fileTransferUtility.UploadAsync(uploadRequest);
            return new AwsResponse { Error = false, FileID = uploadRequest.Key };
        }
        catch (Exception ex)
        {
            return new AwsResponse { Error = true, FileID = name, Message = ex.Message };
        }
    }
}