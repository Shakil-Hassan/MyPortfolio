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
        // Allow reasonably large uploads (e.g. short gameplay clips)
        const long maxUploadSize = 1024L * 1024 * 100; // 100 MB
        var fileContent = new StreamContent(file.OpenReadStream(maxAllowedSize: maxUploadSize));
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
        if (string.IsNullOrEmpty(fileUrl)) return;

        // Convert "http://localhost:5087/uploads/file.jpg" 
        // to just "/uploads/file.jpg"
        var uri = new Uri(fileUrl);
        var relativePath = uri.AbsolutePath; 

        var encodedPath = Uri.EscapeDataString(relativePath);
        var response = await _httpClient.DeleteAsync($"/api/upload?filePath={encodedPath}");
        if (!response.IsSuccessStatusCode)
        {
            // Log error if the physical file couldn't be removed
            var error = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"System Warning: Physical file deletion failed. {error}");
        }
    }

    private class UploadResult { public string Url { get; set; } = string.Empty; }
}