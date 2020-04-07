﻿#tool "nuget:?package=GitVersion.CommandLine"
#tool "nuget:?package=xunit.runner.console"
#addin "nuget:?package=Cake.Incubator"

var version = "0.0.1";
var revision = 0;
var configuration = "Release";
var build = 0;

var isAppVeyorBuild = AppVeyor.IsRunningOnAppVeyor;

var target = Argument("target", "Default");

Task("Version")
    .Does(() =>
    {
        var gitVersion = GitVersion(new GitVersionSettings {
            UpdateAssemblyInfo = false,
            EnvironmentVariables = new Dictionary<string, string>{ ["MSBUILDSINGLELOADCONTEXT"] ="1" },
            OutputType = GitVersionOutput.BuildServer
        });

        /*if (AppVeyor.IsRunningOnAppVeyor)
        {
            var buildNumber = AppVeyor.Environment.Build.Number;
            AppVeyor.UpdateBuildVersion(gitVersion.InformationalVersion);
        }*/

    });

Task("Build")
    .IsDependentOn("Version")
    .Does(() =>
    {
        DotNetCoreRestore("src/SubEtha.sln");
        DotNetCoreBuild("src/SubEtha.sln", new DotNetCoreBuildSettings { 
            Configuration = configuration,
            EnvironmentVariables = new Dictionary<string, string>{ ["MSBUILDSINGLELOADCONTEXT"] ="1" }
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
        var testSettings = new DotNetCoreTestSettings {

            OutputDirectory = "./TestResults"
        };

        CreateDirectory(testSettings.OutputDirectory);
        foreach(var project in GetFiles("**/bin/**/*Test.csproj"))
        {
            DotNetCoreTest(project.FullPath, testSettings);
        }
    });

Task("Default")
    .IsDependentOn("Build");

RunTarget(target);
