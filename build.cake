#tool "nuget:?package=xunit.runner.console&version=2.4.1"
#addin "nuget:?package=Cake.Incubator&version=5.1.0"
#tool "nuget:?package=coverlet.console&version=1.7.1"
#addin "nuget:?package=Cake.Coverlet&version=2.4.2"
#tool "nuget:?package=Codecov&version=1.10.0"
#addin "nuget:?package=Cake.Codecov&version=0.8.0"

var target = Argument("target", "Build");

public class BuildData
{
    public DirectoryPath TestResults;
    public DirectoryPath CoverageResults;
    public string Configuration;
    public int BuildNumber;
}

Setup<BuildData>(ctx => new BuildData()
    {
        TestResults = Directory(@"./TestResults/"),
        CoverageResults = Directory(@"./CoverageResults/"),
        Configuration = "Release",
        BuildNumber = AppVeyor.IsRunningOnAppVeyor ? AppVeyor.Environment.Build.Number : 0
    });

Task("Build")
    .Does<BuildData>(data =>
    {
        DotNetCoreRestore("src/SubEtha.sln");
        DotNetCoreBuild("src/SubEtha.sln", new DotNetCoreBuildSettings
        { 
            Configuration = data.Configuration,
            MSBuildSettings = new DotNetCoreMSBuildSettings
            {
                MaxCpuCount = 1
            }
        });        
    });

Task("Package")
    .Does<BuildData>(data =>
    {
        var packageApp = new Action<string, string>((projectFile, zipName) =>
        {
            var projectDetails = ParseProject(projectFile, data.Configuration);
            var files = GetFiles(projectDetails.OutputPath + "/**/*.*");
            Zip(projectDetails.OutputPath, zipName, files);
        });

        packageApp("src/Matrix/Howatworks.Matrix.Site/Howatworks.Matrix.Site.csproj", "Howatworks.Matrix.Site.zip");
        packageApp("src/Thumb.Assistant/Howatworks.Thumb.Assistant.Console/Howatworks.Thumb.Assistant.Console.csproj", "Howatworks.Thumb.Assistant.Console.zip");
        packageApp("src/Thumb.Matrix/Howatworks.Thumb.Matrix.Console/Howatworks.Thumb.Matrix.Console.csproj", "Howatworks.Thumb.Matrix.Console.zip");        

    });

Task("NuGetPush")
    .Does<BuildData>(data =>
    {
        if (!IsRunningOnWindows()) return;

        var source = "https://www.nuget.org/api/v2/package";
        var apiKey = Argument<string>("nugetapikey");

        var pushSettings = new NuGetPushSettings {Source = source, ApiKey = apiKey};

        // WARNING: this may publish more than we expect!
        var packages = GetFiles("./**/*.nupkg");

        NuGetPush(packages, pushSettings);
    });

Task("Test")
    .Does<BuildData>(data =>
    {
        CleanDirectory(data.TestResults);
        CleanDirectory(data.CoverageResults);

        var testSettings = new DotNetCoreTestSettings
        {
            NoBuild = true,
            OutputDirectory = data.TestResults,
            ResultsDirectory = data.TestResults
        };

        foreach(var project in GetFiles("src/**/*Test.csproj"))
        {
            var coverletSettings = new CoverletSettings
            {
                CollectCoverage = true,
                CoverletOutputFormat = CoverletOutputFormat.cobertura,
                CoverletOutputDirectory = data.CoverageResults,
                CoverletOutputName = File($"Coverage.{project.GetFilenameWithoutExtension()}.cobertura.xml"),
            };

            DotNetCoreTest(project.FullPath, testSettings, coverletSettings);
        }

        RunTarget("PublishCoverage");
    });

Task("PublishCoverage")
    .Does<BuildData>(data =>
    {
        var coverageFiles = GetFiles($"{data.CoverageResults}/*.*");

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
