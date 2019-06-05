#tool "nuget:?package=xunit.runner.console"
#addin "nuget:?package=Cake.Incubator"

var version = "0.0.1";
var revision = 0;
var configuration = "Release";
var build = 0;

// TODO: add these as arguments, instead of interrogating environment variables
//if (BuildSystem.IsRunningOnJenkins)
//{
    version = Environment.GetEnvironmentVariable("version") ?? version;
    if (Environment.GetEnvironmentVariable("revision") != null ) revision = Convert.ToInt32(Environment.GetEnvironmentVariable("revision"));
    configuration = Environment.GetEnvironmentVariable("configuration") ?? configuration;
    if (Environment.GetEnvironmentVariable("build") != null ) build = Convert.ToInt32(Environment.GetEnvironmentVariable("build"));
//}


var target = Argument("target", "Default");

Task("Build")
    .Does(() =>
    {
        NuGetRestore("src/SubEtha.sln");
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

        packageApp("src/Howatworks.Matrix.Service/Howatworks.Matrix.Service.csproj", "Howatworks.Matrix.Service.zip");
        packageApp("src/Howatworks.Matrix.Site/Howatworks.Matrix.Site.csproj", "Howatworks.Matrix.Site.zip");
        packageApp("src/Howatworks.Thumb.Console/Howatworks.Thumb.Console.csproj", "Howatworks.Thumb.Console.zip");
        packageApp("src/Howatworks.Thumb.Tray/Howatworks.Thumb.Tray.csproj", "Howatworks.Thumb.Tray.zip");

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
        var testSettings = new XUnit2Settings {
            HtmlReport = true,
            OutputDirectory = "./TestResults"
    };

    CreateDirectory(testSettings.OutputDirectory);
        XUnit2(GetFiles("**/bin/**/*Test.dll"), testSettings);
    });

Task("Default")
    .IsDependentOn("Build")
    .IsDependentOn("Test")
    .IsDependentOn("Package");

RunTarget(target);
