//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
	.Does(() =>
{
	CleanDirectories("../**/bin");
	CleanDirectories("./**/obj");
});

Task("Restore")
	.Does(() =>
{
	NuGetRestore("./addin/addin.sln");
});

Task("Build")
	.IsDependentOn("Restore")
	.Does(() =>
{
	var msbuildSettings = new MSBuildSettings()
		.SetConfiguration(configuration)
		.UseToolVersion(MSBuildToolVersion.VS2017);

	MSBuild("./addin/addin.sln", msbuildSettings);
});

Task("Package")
	.IsDependentOn("Build")
	.Does(() =>
{
	DeleteFiles("**/bin/**/*.nupkg");
	if(configuration != "Release") throw new Exception("Packages must be made in release mode");

	var version = Argument("PackageVersion", "1.0.0");
	Information("Packaging version " + version);

	MSBuild("./addin/addin.sln", settings => settings
		.SetConfiguration(configuration)
		.WithTarget("pack")
		.WithProperty("NoBuild", "True")
		.WithProperty("PackageVersion", version)
	);
});

Task("Test")
	.Does(() => 
{
	CakeExecuteScript("./addin-test.cake", new CakeSettings {
		ArgumentCustomization = a => a
			.Append("--verbosity=diagnostic")
			.Append("--nuget_useinprocessclient=true")
	});
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
	.IsDependentOn("Package");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
