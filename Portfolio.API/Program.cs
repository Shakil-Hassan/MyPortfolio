using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

app.UseCors();
app.UseHttpsRedirection();
app.UseStaticFiles();

// ── PERSISTENCE HELPERS ─────────────────────────────────────────────────────

static string GetProjectsFilePath(IWebHostEnvironment env) =>
    Path.Combine(env.ContentRootPath, "projects.json");

static async Task<List<ProjectData>> ReadProjectsAsync(string path)
{
    if (!File.Exists(path)) return new List<ProjectData>();
    var json = await File.ReadAllTextAsync(path);
    return JsonSerializer.Deserialize<List<ProjectData>>(json,
        new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
        ?? new List<ProjectData>();
}

static async Task WriteProjectsAsync(string path, List<ProjectData> projects)
{
    var json = JsonSerializer.Serialize(projects, new JsonSerializerOptions { WriteIndented = true });
    await File.WriteAllTextAsync(path, json);
}

// ── PROJECT ENDPOINTS ───────────────────────────────────────────────────────

app.MapGet("/api/projects", async (IWebHostEnvironment env) =>
{
    var path = GetProjectsFilePath(env);
    var projects = await ReadProjectsAsync(path);

    if (projects.Count == 0)
    {
        projects.Add(new ProjectData
        {
            Id          = Guid.NewGuid().ToString(),
            Title       = "Dynamic Level Editor",
            Engine      = "Unity",
            Category    = "Tooling",
            Description = "Custom editor tooling enabling non-dev designers to build and iterate levels.",
            ImageUrl    = string.Empty
        });
        await WriteProjectsAsync(path, projects);
    }

    return Results.Ok(projects);
});

app.MapPost("/api/projects", async (List<ProjectData> projects, IWebHostEnvironment env) =>
{
    var path = GetProjectsFilePath(env);
    await WriteProjectsAsync(path, projects);
    return Results.Ok();
});

// ── FILE UPLOAD ENDPOINTS ───────────────────────────────────────────────────

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

    if (File.Exists(physicalPath))
    {
        File.Delete(physicalPath);
        return Results.Ok();
    }
    return Results.NotFound("File not found on disk.");
});

app.Run();

// ── LOCAL DTO — mirrors Portfolio.Client.Models.Project ────────────────────
// The API doesn't reference the Client project, so we define the shape here.
// Property names must match exactly (case-insensitive JSON handles the rest).
public class ProjectData
{
    public string Id          { get; set; } = Guid.NewGuid().ToString();
    public string Title       { get; set; } = string.Empty;
    public string Engine      { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category    { get; set; } = "Game Systems";
    public string GithubUrl   { get; set; } = string.Empty;
    public string ImageUrl    { get; set; } = string.Empty;
    public string GameUrl     { get; set; } = string.Empty;
    public string TechStack   { get; set; } = string.Empty;
    public List<string> DesignPatterns { get; set; } = new();
    public List<string> Features       { get; set; } = new();
}