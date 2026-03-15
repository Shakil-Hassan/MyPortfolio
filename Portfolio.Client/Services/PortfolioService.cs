using Portfolio.Client.Models;

namespace Portfolio.Client.Services;

public class PortfolioService
{
    private List<Project> _projects = new();

    public PortfolioService()
    {
        // Seed initial data based on your experience
        _projects.Add(new Project { 
            Title = "Dynamic Level Editor", 
            Engine = "Unity", 
            Description = "Custom editor tooling enabling non-dev designers to build, iterate, and publish levels without engineering bottlenecks." 
        });
        _projects.Add(new Project { 
            Title = "Battle Tank 3D", 
            Engine = "Unity", 
            Description = "3D battle game featuring AI aim prediction, custom generic object pooling, and a robust state machine for enemy behavior." 
        });
        _projects.Add(new Project { 
            Title = "Data Pipeline Integration", 
            Engine = "Godot", 
            Description = "Full integration of LevelPlay and Firebase for real-time retention tracking and data-led design decisions." 
        });
    }

    public IReadOnlyList<Project> GetProjects() => _projects.AsReadOnly();

    public void AddProject(Project project)
    {
        _projects.Add(project);
    }

    public void DeleteProject(string id)
    {
        var project = _projects.FirstOrDefault(p => p.Id == id);
        if (project != null)
        {
            _projects.Remove(project);
        }
    }
}