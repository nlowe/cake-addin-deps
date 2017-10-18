#addin nuget:?package=Cake.DummyAddin&loaddependencies=true

var task = Argument("task", "Default");

Task("Default")
    .Does(() => 
{
    Foo();
});

RunTarget(task)