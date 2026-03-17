using Portfolio.Client.Models;
using System.Net.Http.Json;

namespace Portfolio.Client.Services;

public class PortfolioService
{
    private readonly HttpClient _http;

    private List<Project>  _projects = new();
    private SiteContent    _content  = new();
    private bool _projectsLoaded = false;
    private bool _contentLoaded  = false;

    public PortfolioService(HttpClient http) => _http = http;

    // ── Load both on startup ────────────────────────────────────
    public async Task EnsureLoadedAsync()
    {
        await EnsureProjectsLoadedAsync();
        await EnsureContentLoadedAsync();
    }

    // ── PROJECTS ────────────────────────────────────────────────
    private async Task EnsureProjectsLoadedAsync()
    {
        if (_projectsLoaded) return;
        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var loaded = await _http.GetFromJsonAsync<List<Project>>("/api/projects", cts.Token);
            if (loaded != null) _projects = loaded;
        }
        catch
        {
            _projects.Add(new Project { Title = "Dynamic Level Editor", Engine = "Unity",
                Category = "Tooling", Description = "Custom editor tooling enabling non-dev designers to build and iterate levels." });
        }
        _projectsLoaded = true;
    }

    public IReadOnlyList<Project> GetProjects() => _projects.AsReadOnly();

    public void AddProject(Project p)    { _projects.Add(p); _ = SaveProjectsAsync(); }
    public void DeleteProject(string id) { _projects.RemoveAll(p => p.Id == id); _ = SaveProjectsAsync(); }

    private async Task SaveProjectsAsync()
    {
        try { await _http.PostAsJsonAsync("/api/projects", _projects); }
        catch (Exception ex) { Console.WriteLine($"Projects save failed: {ex.Message}"); }
    }

    // ── SITE CONTENT ────────────────────────────────────────────
    private async Task EnsureContentLoadedAsync()
    {
        if (_contentLoaded) return;
        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var loaded = await _http.GetFromJsonAsync<SiteContent>("/api/content", cts.Token);
            if (loaded != null) _content = loaded;
        }
        catch { /* keep defaults */ }
        _contentLoaded = true;
    }

    public SiteContent GetContent() => _content;

    public async Task SaveContentAsync(SiteContent content)
    {
        _content = content;
        try { await _http.PostAsJsonAsync("/api/content", content); }
        catch (Exception ex) { Console.WriteLine($"Content save failed: {ex.Message}"); }
    }
}
