# Task 08-Fireworks
Your task is to implement a simulation of fireworks using 3D particle system principle.
Individual particles (flying rockets, sparks, flares) should obey the laws of physics
(gravity and air resistance), but you do not have to worry about their mutual
interactions or collisions.

The entire 3D scene (fireworks) must be rendered interactively using the
[OpenGL library](https://www.opengl.org/) bound to the .NET through the
[Silk.NET library](https://github.com/dotnet/Silk.NET).

# Ideas
To be added later.

# Sil.NET framework
It is easy to use the [Silk.NET](https://github.com/dotnet/Silk.NET) in your C#
program, you just install the [Silk.NET NuGet package](https://www.nuget.org/packages/Silk.NET/).

You can view our sample projects in the
[Silk3D directory](../Silk3D/README.md) of our repository.

# Your solution
Please place your solution in a separate [solutions](solutions/README.md)
directory in the repository. You'll find short instructions there.

# Launch date
**Monday 11 December 2023**
(Don't work on the solution before this date)

# Deadline
See the shared [point table](https://docs.google.com/spreadsheets/d/1QLukOcSRPa5exOYW1eUfQWY2WoMjo1menbjQIU7Gvs4/edit?usp=sharing).

# Credit points
**Basic solution: 10 points**
* Working interactive fireworks simulation
  - The particles must move according to the laws of physics (so that no disturbing
	deviations are visible at first glance)
  - The particles must have a lifetime limit implemented so that the simulation
	system is not overloaded (the simulation must be able to run for several minutes)
* The entire 3D scene is interactively viewed by the user using the [Trackball system](../Silk3D/shared/Trackball.cs)

**Bonus points: up to 14+ more points**
* Animation of launch ramps
* Multiple rocket/particle types
* Multi-stage explosions
* Color changes
* Visualization of rocket trajectories
* Interactive fireworks control (mouse, keyboard)
* Advanced shading effects, etc.

## Use of AI assistant
It is possible to use an AI assistant, but you have to be critical and
test all its suggestions thoroughly.

# Images
Not yet.
