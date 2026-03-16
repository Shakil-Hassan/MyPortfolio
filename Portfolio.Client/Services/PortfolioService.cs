using Portfolio.Client.Models;

namespace Portfolio.Client.Services;

public class PortfolioService
{
    private List<Project> _projects = new();

    public PortfolioService()
    {
        _projects.Add(new Project { 
            Title = "Dynamic Level Editor", 
            Engine = "Unity", 
            Category = "Tooling",
            Description = "Custom editor tooling enabling non-dev designers to build and iterate levels." 
        });
    }

    public IReadOnlyList<Project> GetProjects() => _projects.AsReadOnly();
    public void AddProject(Project project) => _projects.Add(project);
    public void DeleteProject(string id) => _projects.RemoveAll(p => p.Id == id);
}