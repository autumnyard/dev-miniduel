# dev-miniduel

## Goal
Small project aiming to provide an example of how to properly set boundaries on a Unity project.

1. The game logic is in its own C# project, in a visual studio solution that builds a `.dll library`.
2. Then, the Unity project uses this dll to provide an interface wrapping it.

This can furtherly be expanded to use that same `.dll library` containing the game in other engines like Unreal, Godot or Game Maker.

## Sections

### Game
the folder `/Miniduel` includes 
- the visual studio solution: `/Miniduel/Miniduel.sln`
- the project with the Game logic: `Miniduel/Miniduel/Miniduel.csproj`
- another project with a console program to test this: `/Miniduel/Miniduel/MiniduelRunner.csproj`

### Unity
> incoming...