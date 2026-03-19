using System.Collections.Generic;

namespace Portfolio.Client.Models;

// ── PROJECT ──────────────────────────────────────────────────────────────────

public class Project
{
    public string Id             { get; set; } = Guid.NewGuid().ToString();
    public string Title          { get; set; } = string.Empty;
    public string Engine         { get; set; } = string.Empty;
    public string Description    { get; set; } = string.Empty;
    public string Category       { get; set; } = "Game Systems";
    public string GithubUrl      { get; set; } = string.Empty;
    public string ImageUrl          { get; set; } = string.Empty;
    public string VideoUrl          { get; set; } = string.Empty;
    public bool   UseVideoThumbnail { get; set; } = false;
    public string GameUrl           { get; set; } = string.Empty;
    public List<string> DesignPatterns { get; set; } = new();
    public List<string> Features       { get; set; } = new();
    public string TechStack      { get; set; } = string.Empty;
}

// ── SITE CONTENT ─────────────────────────────────────────────────────────────

public class SiteContent
{
    public string HeroStatus       { get; set; } = "Available for Remote Work";
    public string HeroEyebrow      { get; set; } = "Game Developer & Tools Engineer";
    public string HeroHeadline1    { get; set; } = "BUILDING";
    public string HeroHeadline2    { get; set; } = "GAMES";
    public string HeroHeadline3    { get; set; } = "& SYSTEMS";
    public string HeroSubtitle     { get; set; } = "Unity · Godot · C# · C++";
    public string HeroDescription  { get; set; } = "Specializing in Tools Engineering and modular systems architecture. 3+ years bridging technical complexity and creative vision.";
    public string HeroCtaPrimary   { get; set; } = "Initialize Contact";
    public string HeroCtaSecondary { get; set; } = "View Experience →";
    public List<StatItem>         HeroStats       { get; set; } = new() {
        new() { Value="3+", Label="Years Exp" }, new() { Value="20+", Label="Tech Interviews" }, new() { Value="2", Label="Engines" } };
    public List<string>           MarqueeItems    { get; set; } = new() {
        "Unity 3D","Godot Engine","C Sharp","GDScript","Level Editor","DOTween","Firebase","Play Store ASO","Git & SourceTree","LevelPlay SDK" };
    public string AboutHeadline    { get; set; } = "THE BUILDER";
    public string AboutSubheadline { get; set; } = "BEHIND THE BUILD";
    public List<string>           AboutParagraphs { get; set; } = new() {
        "My biggest strength is <strong>Tooling</strong>. I've developed custom <strong>Dynamic Level Editors</strong> that allow designers to iterate without dev intervention — drastically cutting time-to-market.",
        "I'm deeply familiar with the <strong>full game lifecycle</strong>: from physics prototypes to Play Store publishing and ASO optimization." };
    public List<string>           AboutTags       { get; set; } = new() { "Unity","Godot","C#","C++","GDScript","Editor Scripting" };
    public List<ExpertiseBar>     ExpertiseBars   { get; set; } = new() {
        new() { Name="Tooling & Editor Scripting", Pct=96 },
        new() { Name="Unity / C#",                 Pct=94 },
        new() { Name="Godot / GDScript",            Pct=88 },
        new() { Name="Systems Architecture",        Pct=90 },
        new() { Name="Analytics & SDK Integration", Pct=82 },
        new() { Name="Git / Version Control",       Pct=92 } };
    public List<HighlightStat>    Highlights      { get; set; } = new() {
        new() { Number="3",   Suffix="+", Label="Years of Game Dev Experience" },
        new() { Number="20",  Suffix="+", Label="Technical Interviews Conducted" },
        new() { Number="100", Suffix="%", Label="Remote-Ready Workflow" },
        new() { Number="∞",   Suffix="",  Label="Passion for Process Optimization" } };
    public List<JobItem>          Jobs            { get; set; } = new() {
        new() { Date="2022 — Present", Company="Game Studio",    Role="Senior Game Developer",
                Desc="Built custom Dynamic Level Editors enabling designers to iterate independently, reducing dev intervention and cutting time-to-market.",
                Tags=new(){"Unity","C#","Editor Scripting","DOTween"} },
        new() { Date="2021 — 2022",   Company="Indie / Contract",Role="Game Developer",
                Desc="Developed full-cycle mobile games from physics prototyping through Play Store publishing and ASO. Integrated Firebase and ByteBrew.",
                Tags=new(){"Godot","GDScript","Firebase","ASO"} },
        new() { Date="2021",          Company="Early Career",    Role="Junior Developer",
                Desc="Established foundations in game systems architecture, version control, and cross-functional team collaboration.",
                Tags=new(){"C++","Git","SourceTree"} } };
    public List<DeliverableItem>  Deliverables    { get; set; } = new() {
        new() { Label="01 — Level Editor", Title="Dynamic Level Editor System",
                Desc="Custom Unity tooling enabling non-dev designers to build and publish levels without engineering bottlenecks." },
        new() { Label="02 — Analytics",    Title="Data Pipeline Integration",
                Desc="Full integration of LevelPlay, Firebase, and ByteBrew for real-time retention tracking." },
        new() { Label="03 — Publishing",   Title="Play Store Publishing & ASO",
                Desc="End-to-end game deployment with App Store Optimization for organic acquisition growth." } };
    public string ContactTitle    { get; set; } = "LET'S BUILD TOGETHER";
    public string ContactSub      { get; set; } = "Currently seeking remote opportunities where I can optimize production pipelines through tools engineering.";
    public string ContactEmail    { get; set; } = "hello@example.com";
    public string ContactLinkedIn { get; set; } = "https://linkedin.com";
    public string ContactGithub   { get; set; } = "https://github.com";
    public string ContactResume   { get; set; } = "#";
}

// ── VALUE TYPES ───────────────────────────────────────────────────────────────

public class StatItem
{
    public string Value { get; set; } = "";
    public string Label { get; set; } = "";
}

public class ExpertiseBar
{
    public string Name { get; set; } = "";
    public int    Pct  { get; set; } = 80;
}

public class HighlightStat
{
    public string Number { get; set; } = "";
    public string Suffix { get; set; } = "";
    public string Label  { get; set; } = "";
}

public class JobItem
{
    public string       Date    { get; set; } = "";
    public string       Company { get; set; } = "";
    public string       Role    { get; set; } = "";
    public string       Desc    { get; set; } = "";
    public List<string> Tags    { get; set; } = new();
}

public class DeliverableItem
{
    public string Label { get; set; } = "";
    public string Title { get; set; } = "";
    public string Desc  { get; set; } = "";
}
