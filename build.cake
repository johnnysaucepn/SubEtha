#tool "nuget:?package=xunit.runner.console&version=2.4.1"
#addin "nuget:?package=Cake.Incubator&version=5.1.0"
#tool "nuget:?package=coverlet.console&version=1.7.1"
#addin "nuget:?package=Cake.Coverlet&version=2.4.2"
#tool "nuget:?package=Codecov&version=1.10.0"
#addin "nuget:?package=Cake.Codecov&version=0.8.0"

var configuration = "Release";
var buildNumber = AppVeyor.IsRunningOnAppVeyor ? AppVeyor.Environment.Build.Number : 0;

var target = Argument("target", "Build");

Task("Build")
    .Does(() =>
    {
        DotNetCoreRestore("src/SubEtha.sln");
        DotNetCoreBuild("src/SubEtha.sln", new DotNetCoreBuildSettings
        { 
            Configuration = configuration
        });        
    });

Task("Package")
    .Does(() =>
    {
        var packageApp = new Action<string, string>((projectFile, zipName) =>
        {
            var projectDetails = ParseProject(projectFile, configuration);
            var files = GetFiles(projectDetails.OutputPath + "/**/*.*");
            Zip(projectDetails.OutputPath, zipName, files);
        });

        packageApp("src/Matrix/Howatworks.Matrix.Site/Howatworks.Matrix.Site.csproj", "Howatworks.Matrix.Site.zip");
        packageApp("src/Thumb.Assistant/Howatworks.Thumb.Assistant.Console/Howatworks.Thumb.Assistant.Console.csproj", "Howatworks.Thumb.Assistant.Console.zip");
        packageApp("src/Thumb.Matrix/Howatworks.Thumb.Matrix.Console/Howatworks.Thumb.Matrix.Console.csproj", "Howatworks.Thumb.Matrix.Console.zip");        

    });

Task("NuGetPush")
    .Does(() =>
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
    .IsDependentOn("Build")
    .Does(() =>
    {
        var testDirectory = Directory(@"./TestResults/");
        var coverletDirectory = Directory(@"./CoverageResults/");

        CleanDirectory(testDirectory);
        CleanDirectory(coverletDirectory);

        var coverletReport = File(@"Coverage.cobertura.xml");

        var testSettings = new DotNetCoreTestSettings
        {
            NoBuild = true,
            ResultsDirectory = testDirectory
        };

        foreach(var project in GetFiles("src/**/*Test.csproj"))
        {
            var coverletSettings = new CoverletSettings
            {
                CollectCoverage = true,
                CoverletOutputFormat = CoverletOutputFormat.cobertura,
                CoverletOutputDirectory = coverletDirectory,
                CoverletOutputName = File($"Coverage.{project.GetFilenameWithoutExtension()}.cobertura.xml"),
            };

            DotNetCoreTest(project.FullPath, testSettings, coverletSettings);
        }

        if (AppVeyor.IsRunningOnAppVeyor)
        {
            Codecov(new CodecovSettings
            { 
                Files = GetFiles(coverletDirectory).Select(f => f.FullPath),
                NoColor = true
            });
        }
    });

RunTarget(target);
