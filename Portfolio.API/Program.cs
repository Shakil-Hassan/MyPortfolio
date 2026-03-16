using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// 1. Configure CORS to allow the Client (Port 5227) to talk to this API
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy => 
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

app.UseCors();
app.UseHttpsRedirection();

// 2. IMPORTANT: This allows the browser to view the uploaded images
app.UseStaticFiles(); 

// --- API ENDPOINTS ---

app.MapPost("/api/upload", async (IFormFile file, IWebHostEnvironment env) =>
{
    if (file == null || file.Length == 0) return Results.BadRequest("No file.");

    var webRoot = env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
    var folderPath = Path.Combine(webRoot, "uploads");
    if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
    var fullPath = Path.Combine(folderPath, fileName);

    using var stream = new FileStream(fullPath, FileMode.Create);
    await file.CopyToAsync(stream);

    return Results.Ok(new { Url = $"/uploads/{fileName}" });
}).DisableAntiforgery();

app.MapDelete("/api/upload", (string filePath, IWebHostEnvironment env) =>
{
    var webRoot = env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
    var physicalPath = Path.Combine(webRoot, filePath.TrimStart('/'));
    if (File.Exists(physicalPath)) File.Delete(physicalPath);
    return Results.Ok();
});

app.Run();