using Microsoft.AspNetCore.Components.Forms;

namespace Portfolio.Client.Services;

public interface IFileUploadService
{
    Task<string> UploadFileAsync(IBrowserFile file);
    Task DeleteFileAsync(string fileUrl);
}