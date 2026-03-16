using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;

namespace Portfolio.Client.Services;

public class WasmFileUploadService : IFileUploadService
{
    private readonly HttpClient _httpClient;

    public WasmFileUploadService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> UploadFileAsync(IBrowserFile file)
    {
        using var content = new MultipartFormDataContent();
        var fileContent = new StreamContent(file.OpenReadStream(maxAllowedSize: 1024 * 1024 * 10));
        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
        
        content.Add(fileContent, "file", file.Name);

        var response = await _httpClient.PostAsync("/api/upload", content);
        
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<UploadResult>();
            if (result != null && !string.IsNullOrEmpty(result.Url))
            {
                // Returns the FULL URL (e.g., http://localhost:5087/uploads/image.jpg)
                return _httpClient.BaseAddress + result.Url.TrimStart('/');
            }
        }
        return string.Empty;
    }

    public async Task DeleteFileAsync(string fileUrl)
    {
        await _httpClient.DeleteAsync($"/api/upload?filePath={Uri.EscapeDataString(fileUrl)}");
    }

    private class UploadResult { public string Url { get; set; } = string.Empty; }
}