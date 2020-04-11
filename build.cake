﻿#tool "nuget:?package=xunit.runner.console&version=2.4.1"
#addin "nuget:?package=Cake.Incubator&version=5.1.0"
#tool "nuget:?package=coverlet.console&version=1.7.1"
#addin "nuget:?package=Cake.Coverlet&version=2.4.2"
#tool "nuget:?package=Codecov&version=1.10.0"
#addin "nuget:?package=Cake.Codecov&version=0.8.0"

var target = Argument("target", "Build");

public class BuildData
{
    public DirectoryPath TestResultsDirectory;
    public DirectoryPath CoverageResultsDirectory;
    public DirectoryPath AppDirectory;
    public string Configuration;
    public int BuildNumber;
}

Setup<BuildData>(ctx => new BuildData()
    {
        TestResultsDirectory = Directory(@"./TestResults/"),
        CoverageResultsDirectory = Directory(@"./CoverageResults/"),
        AppDirectory = Directory(@"./PublishedApps/"),
        Configuration = "Release",
        BuildNumber = AppVeyor.IsRunningOnAppVeyor ? AppVeyor.Environment.Build.Number : 0
    });

Task("Build")
    .Does<BuildData>(data =>
    {
        DotNetCoreBuild("src/SubEtha.sln", new DotNetCoreBuildSettings
        { 
            Configuration = data.Configuration,
            MSBuildSettings = new DotNetCoreMSBuildSettings
            {
                MaxCpuCount = 1
            }
        });        
    });

Task("PublishApps")
    .Does<BuildData>(data =>
    {
        CleanDirectory(data.AppDirectory);

        DotNetCorePublish("./src/Matrix/Howatworks.Matrix.Site/Howatworks.Matrix.Site.csproj");
        DotNetCorePublish("./src/Thumb.Assistant/Howatworks.Thumb.Assistant.Console/Howatworks.Thumb.Assistant.Console.csproj");
        DotNetCorePublish("./src/Thumb.Matrix/Howatworks.Thumb.Matrix.Console/Howatworks.Thumb.Matrix.Console.csproj");
    });

Task("NuGetPush")
    .Does<BuildData>(data =>
    {
        if (!IsRunningOnWindows()) return;

        var source = "https://www.nuget.org/api/v2/package";
        var apiKey = Argument<string>("nugetapikey");

        var pushSettings = new NuGetPushSettings {Source = source, ApiKey = apiKey};

        // WARNING: this may publish more than we expect!
        var packages = GetFiles("./**/Howatworks.*.nupkg");

        NuGetPush(packages, pushSettings);
    });

Task("Test")
    .Does<BuildData>(data =>
    {
        CleanDirectory(data.TestResultsDirectory);
        CleanDirectory(data.CoverageResultsDirectory);

        var testSettings = new DotNetCoreTestSettings
        {
            NoBuild = true,
            ResultsDirectory = data.TestResultsDirectory
        };

        foreach(var project in GetFiles("src/**/*Test.csproj"))
        {
            var coverletSettings = new CoverletSettings
            {
                CollectCoverage = true,
                CoverletOutputFormat = CoverletOutputFormat.cobertura,
                CoverletOutputDirectory = data.CoverageResultsDirectory,
                CoverletOutputName = File($"Coverage.{project.GetFilenameWithoutExtension()}.cobertura.xml"),
            };

            DotNetCoreTest(project.FullPath, testSettings, coverletSettings);
        }
    });

Task("PublishCoverage")
    .Does<BuildData>(data =>
    {
        var coverageFiles = GetFiles($"{data.CoverageResultsDirectory}/*.*");

        if (AppVeyor.IsRunningOnAppVeyor)
        {
            Codecov(new CodecovSettings
            { 
                Files = coverageFiles.Select(f => f.FullPath),
                NoColor = true,
                Required = true
            });
        }
    });

RunTarget(target);
