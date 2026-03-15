namespace Portfolio.Client.Models;

public class DevProfile
{
    public string Name { get; set; } = "Shakil Hassan"; 
    public string Role { get; set; } = "Game Developer & Systems Engineer"; 
    public int YearsExperience { get; set; } = 3;
    public int TechnicalInterviews { get; set; } = 20; 
    public List<string> CoreLanguages { get; set; } = new() { "C#", "C++", "GDScript" }; 
    public List<string> Engines { get; set; } = new() { "Unity", "Godot" };
}