using System.Text.Json;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<FormOptions>(options =>
{
    // Allow larger uploads (e.g. video files)
    options.MultipartBodyLengthLimit = 1024L * 1024 * 200; // 200 MB
});

builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();
app.UseCors();
app.UseHttpsRedirection();
app.UseStaticFiles();

// ── PROJECTS ─────────────────────────────────────────────────────────────────

app.MapGet("/api/projects", async (IWebHostEnvironment env) =>
{
    var opts = GetJsonOpts();
    var path = GetProjectsPath(env);
    if (!File.Exists(path))
    {
        var seed = new List<ProjectData> { new() {
            Id = Guid.NewGuid().ToString(), Title = "Dynamic Level Editor",
            Engine = "Unity", Category = "Tooling",
            Description = "Custom editor tooling enabling non-dev designers to build and iterate levels." }};
        await File.WriteAllTextAsync(path, JsonSerializer.Serialize(seed, opts));
        return Results.Ok(seed);
    }
    var json = await File.ReadAllTextAsync(path);
    return Results.Ok(JsonSerializer.Deserialize<List<ProjectData>>(json, opts) ?? new());
});

app.MapPost("/api/projects", async (List<ProjectData> projects, IWebHostEnvironment env) =>
{
    await File.WriteAllTextAsync(GetProjectsPath(env), JsonSerializer.Serialize(projects, GetJsonOpts()));
    return Results.Ok();
});

// ── SITE CONTENT ─────────────────────────────────────────────────────────────

app.MapGet("/api/content", async (IWebHostEnvironment env) =>
{
    var opts = GetJsonOpts();
    var path = GetContentPath(env);
    if (!File.Exists(path)) return Results.Ok(new SiteContentData());
    var json = await File.ReadAllTextAsync(path);
    return Results.Ok(JsonSerializer.Deserialize<SiteContentData>(json, opts) ?? new());
});

app.MapPost("/api/content", async (SiteContentData content, IWebHostEnvironment env) =>
{
    await File.WriteAllTextAsync(GetContentPath(env), JsonSerializer.Serialize(content, GetJsonOpts()));
    return Results.Ok();
});

// ── FILE UPLOAD ───────────────────────────────────────────────────────────────

app.MapPost("/api/upload", async (IFormFile file, IWebHostEnvironment env) =>
{
    if (file == null || file.Length == 0) return Results.BadRequest("No file.");
    var webRoot  = env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
    var folder   = Path.Combine(webRoot, "uploads");
    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
    var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
    await using var stream = new FileStream(Path.Combine(folder, fileName), FileMode.Create);
    await file.CopyToAsync(stream);
    return Results.Ok(new { Url = $"/uploads/{fileName}" });
}).DisableAntiforgery();

app.MapDelete("/api/upload", (string filePath, IWebHostEnvironment env) =>
{
    var physical = Path.Combine(
        env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"),
        filePath.TrimStart('/'));
    if (File.Exists(physical)) { File.Delete(physical); return Results.Ok(); }
    return Results.NotFound();
});

app.Run();

// ── HELPERS (regular static methods — not top-level fields) ──────────────────

static string GetProjectsPath(IWebHostEnvironment env) =>
    Path.Combine(env.ContentRootPath, "projects.json");

static string GetContentPath(IWebHostEnvironment env) =>
    Path.Combine(env.ContentRootPath, "site-content.json");

static JsonSerializerOptions GetJsonOpts() =>
    new() { PropertyNameCaseInsensitive = true, WriteIndented = true };

// ── DTOs ─────────────────────────────────────────────────────────────────────

public class ProjectData
{
    public string Id          { get; set; } = Guid.NewGuid().ToString();
    public string Title       { get; set; } = string.Empty;
    public string Engine      { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category    { get; set; } = "Game Systems";
    public string YourRole    { get; set; } = string.Empty;
    public string GithubUrl   { get; set; } = string.Empty;
    public string ImageUrl           { get; set; } = string.Empty;
    public string VideoUrl           { get; set; } = string.Empty;
    public bool   UseVideoThumbnail  { get; set; } = false;
    public string GameplayVideoUrl   { get; set; } = string.Empty;
    public string GameUrl            { get; set; } = string.Empty;
    public string TechStack          { get; set; } = string.Empty;
    public List<string> DesignPatterns { get; set; } = new();
    public List<string> Features       { get; set; } = new();
    public List<string> ScreenshotUrls { get; set; } = new();
    public ShippedStatsData ShippedMetrics { get; set; } = new();
}

public class SiteContentData
{
    public string HeroStatus       { get; set; } = "Available for Remote Work";
    public string HeroEyebrow      { get; set; } = "Game Developer & Tools Engineer";
    public string HeroHeadline1    { get; set; } = "BUILDING";
    public string HeroHeadline2    { get; set; } = "GAMES";
    public string HeroHeadline3    { get; set; } = "& SYSTEMS";
    public string HeroSubtitle     { get; set; } = "Unity · Godot · C# · C++";
    public string HeroDescription  { get; set; } = "Specializing in Tools Engineering and modular systems architecture.";
    public string HeroCtaPrimary   { get; set; } = "Initialize Contact";
    public string HeroCtaSecondary { get; set; } = "View Experience →";
    public List<StatItemData>        HeroStats      { get; set; } = new() {
        new() { Value="3+", Label="Years Exp" }, new() { Value="20+", Label="Tech Interviews" }, new() { Value="2", Label="Engines" } };
    public List<string>              MarqueeItems   { get; set; } = new() {
        "Unity 3D","Godot Engine","C Sharp","GDScript","Level Editor","DOTween","Firebase","Play Store ASO","Git & SourceTree","LevelPlay SDK" };
    public string AboutHeadline    { get; set; } = "THE BUILDER";
    public string AboutSubheadline { get; set; } = "BEHIND THE BUILD";
    public List<string>              AboutParagraphs { get; set; } = new() {
        "My biggest strength is Tooling. I've developed custom Dynamic Level Editors that allow designers to iterate without dev intervention.",
        "I'm deeply familiar with the full game lifecycle: from physics prototypes to Play Store publishing and ASO optimization." };
    public List<string>              AboutTags      { get; set; } = new() { "Unity","Godot","C#","C++","GDScript","Editor Scripting" };
    public List<ExpertiseBarData>    ExpertiseBars  { get; set; } = new() {
        new(){Name="Tooling & Editor Scripting",Pct=96}, new(){Name="Unity / C#",Pct=94},
        new(){Name="Godot / GDScript",Pct=88}, new(){Name="Systems Architecture",Pct=90},
        new(){Name="Analytics & SDK Integration",Pct=82}, new(){Name="Git / Version Control",Pct=92} };
    public List<HighlightStatData>   Highlights     { get; set; } = new() {
        new(){Number="3",Suffix="+",Label="Years of Game Dev Experience"},
        new(){Number="20",Suffix="+",Label="Technical Interviews Conducted"},
        new(){Number="100",Suffix="%",Label="Remote-Ready Workflow"},
        new(){Number="∞",Suffix="",Label="Passion for Process Optimization"} };
    public List<JobItemData>         Jobs           { get; set; } = new() {
        new(){Date="2022 — Present",Company="Game Studio",Role="Senior Game Developer",
              Desc="Built custom Dynamic Level Editors enabling designers to iterate independently.",
              Tags=new(){"Unity","C#","Editor Scripting","DOTween"}},
        new(){Date="2021 — 2022",Company="Indie / Contract",Role="Game Developer",
              Desc="Developed full-cycle mobile games from physics prototyping through Play Store publishing.",
              Tags=new(){"Godot","GDScript","Firebase","ASO"}},
        new(){Date="2021",Company="Early Career",Role="Junior Developer",
              Desc="Established foundations in game systems architecture and version control.",
              Tags=new(){"C++","Git","SourceTree"}} };
    public List<DeliverableItemData> Deliverables   { get; set; } = new() {
        new(){Label="01 — Level Editor",Title="Dynamic Level Editor System",Desc="Custom Unity tooling enabling non-dev designers to build and publish levels."},
        new(){Label="02 — Analytics",Title="Data Pipeline Integration",Desc="Full integration of LevelPlay, Firebase, and ByteBrew."},
        new(){Label="03 — Publishing",Title="Play Store Publishing & ASO",Desc="End-to-end game deployment with App Store Optimization."} };
    public string ContactTitle    { get; set; } = "LET'S BUILD TOGETHER";
    public string ContactSub      { get; set; } = "Currently seeking remote opportunities where I can optimize production pipelines.";
    public string ContactEmail    { get; set; } = "hello@example.com";
    public string ContactLinkedIn { get; set; } = "https://linkedin.com";
    public string ContactGithub   { get; set; } = "https://github.com";
    public string ContactResume   { get; set; } = "#";
    public List<PublishedGameData>    PublishedGames { get; set; } = new() {
        new() { Title="Endless Runner", Platforms=new(){"iOS","Android"}, AppStoreUrl="https://apps.apple.com", PlayStoreUrl="https://play.google.com", Engine="Unity" },
        new() { Title="Puzzle Master", Platforms=new(){"iOS","Android"}, AppStoreUrl="https://apps.apple.com", PlayStoreUrl="https://play.google.com", Engine="Unity" } };
    public List<EngineBreakdownData>  EngineStats    { get; set; } = new() {
        new() { Engine="Unity", ProjectCount=12, Description="Primary engine for mobile & console games" },
        new() { Engine="Godot", ProjectCount=5, Description="Cross-platform indie projects" } };
    public List<TeamExperienceData>   TeamExperiences { get; set; } = new() {
        new() { Period="2022 — Present", Studio="Game Studio", TeamSize=8, Roles=new(){"Gameplay","Tools","Architecture"}, CollaborationTools=new(){"Git","Perforce","Jira"} },
        new() { Period="2021 — 2022", Studio="Indie / Contract", TeamSize=1, Roles=new(){"Full-Stack"}, CollaborationTools=new(){"Git","GitHub","Trello"} } };
}

public class StatItemData        { public string Value  { get; set; } = ""; public string Label  { get; set; } = ""; }
public class ExpertiseBarData    { public string Name   { get; set; } = ""; public int    Pct    { get; set; } = 80; }
public class HighlightStatData   { public string Number { get; set; } = ""; public string Suffix { get; set; } = ""; public string Label { get; set; } = ""; }
public class JobItemData         { public string Date   { get; set; } = ""; public string Company{ get; set; } = ""; public string Role  { get; set; } = "";
                                   public string Desc   { get; set; } = ""; public List<string> Tags { get; set; } = new(); }
public class DeliverableItemData { public string Label  { get; set; } = ""; public string Title  { get; set; } = ""; public string Desc  { get; set; } = ""; }
public class PublishedGameData   { public string Title  { get; set; } = ""; public string Engine { get; set; } = "Unity"; public List<string> Platforms { get; set; } = new();
                                   public string AppStoreUrl { get; set; } = ""; public string PlayStoreUrl { get; set; } = ""; public string ItchIoUrl { get; set; } = ""; public string SteamUrl { get; set; } = ""; }
public class EngineBreakdownData { public string Engine { get; set; } = ""; public int ProjectCount { get; set; } = 0; public string Description { get; set; } = ""; }
public class TeamExperienceData  { public string Period { get; set; } = ""; public string Studio { get; set; } = ""; public int TeamSize { get; set; } = 1;
                                   public List<string> Roles { get; set; } = new(); public List<string> CollaborationTools { get; set; } = new(); }
public class ShippedStatsData    { public bool   IsShipped      { get; set; } = false; public long   Downloads        { get; set; } = 0;
                                   public double Rating          { get; set; } = 0.0; public int    ReviewCount     { get; set; } = 0;
                                   public string CategoryRank    { get; set; } = ""; public int    DailyActiveUsers { get; set; } = 0;
                                   public int    Retention7Day   { get; set; } = 0; public List<string> Platforms { get; set; } = new(); }
