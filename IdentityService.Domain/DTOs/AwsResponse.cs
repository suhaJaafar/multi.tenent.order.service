namespace IdentityService.Domain.DTOs;
public class AwsResponse
{
    public string FileID { get; set; }
    public bool Error { get; set; }
    public string Message { get; set; }
}