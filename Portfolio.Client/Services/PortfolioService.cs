using Portfolio.Client.Models;
using System.Net.Http.Json;

namespace Portfolio.Client.Services;

/// <summary>
/// PortfolioService — Client-side singleton.
/// Loads from the API on first use, writes back on every mutation.
/// Data survives page refreshes because it is stored as JSON on the server.
/// </summary>
public class PortfolioService
{
    private readonly HttpClient _http;
    private List<Project> _projects = new();
    private bool _loaded = false;

    public PortfolioService(HttpClient http)
    {
        _http = http;
    }

    // ── READ ────────────────────────────────────────────────────
    /// Call this once from any page that needs data (await it).
    public async Task EnsureLoadedAsync()
    {
        if (_loaded) return;
        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5)); // 5s timeout
            var loaded = await _http.GetFromJsonAsync<List<Project>>("/api/projects", cts.Token);
            if (loaded != null) _projects = loaded;
        }
        catch
        {
            // API unreachable or timed out — fall back to defaults
            SeedDefaults();
        }
        _loaded = true;
    }

    /// Synchronous accessor — safe AFTER EnsureLoadedAsync() has been awaited.
    public IReadOnlyList<Project> GetProjects() => _projects.AsReadOnly();

    // ── WRITE ───────────────────────────────────────────────────
    public async Task AddProjectAsync(Project project)
    {
        _projects.Add(project);
        await SaveAsync();
    }

    public async Task DeleteProjectAsync(string id)
    {
        _projects.RemoveAll(p => p.Id == id);
        await SaveAsync();
    }

    // Keep the old sync methods so Admin.razor compiles without changes.
    // They fire-and-forget the save — fine for this use-case.
    public void AddProject(Project project)
    {
        _projects.Add(project);
        _ = SaveAsync();
    }

    public void DeleteProject(string id)
    {
        _projects.RemoveAll(p => p.Id == id);
        _ = SaveAsync();
    }

    // ── PRIVATE ─────────────────────────────────────────────────
    private async Task SaveAsync()
    {
        try
        {
            await _http.PostAsJsonAsync("/api/projects", _projects);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"PortfolioService: save failed — {ex.Message}");
        }
    }

    private void SeedDefaults()
    {
        _projects.Add(new Project
        {
            Title       = "Dynamic Level Editor",
            Engine      = "Unity",
            Category    = "Tooling",
            Description = "Custom editor tooling enabling non-dev designers to build and iterate levels."
        });
    }
}