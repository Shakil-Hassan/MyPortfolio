using System.Collections.Generic;

namespace Portfolio.Client.Models;

public class Project
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; } = string.Empty;
    public string Engine { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = "Game Systems";
    public string GithubUrl { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string GameUrl { get; set; } = string.Empty;
    public List<string> DesignPatterns { get; set; } = new(); 
    public List<string> Features { get; set; } = new(); 
    public string TechStack { get; set; } = string.Empty;
}