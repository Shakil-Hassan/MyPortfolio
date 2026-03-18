using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text.Json;

[McpServerToolType]
public class PortfolioTools
{
    private readonly string _basePath = Path.Combine("..", "Portfolio.API");

    [McpServerTool, Description("Get all projects from the portfolio")]
    public string GetProjects()
    {
        var path = Path.Combine(_basePath, "projects.json");
        return File.ReadAllText(path);
    }

    [McpServerTool, Description("Get site content including personal info, jobs, and skills")]
    public string GetSiteContent()
    {
        var path = Path.Combine(_basePath, "site-content.json");
        return File.ReadAllText(path);
    }

    [McpServerTool, Description("Get list of uploaded images from the portfolio")]
    public string GetUploadedImages()
    {
        var uploadsPath = Path.Combine(_basePath, "wwwroot", "uploads");
        var files = Directory.GetFiles(uploadsPath)
            .Select(Path.GetFileName)
            .ToArray();
        return JsonSerializer.Serialize(files);
    }
}