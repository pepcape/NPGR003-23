# Task 07-Terrain
Your task is to implement fractal terrain generation using a "subdivision" algorithm. 3D scene will
be displayed in an interactive environment using
[OpenGL library](https://www.opengl.org/) bound to the .NET by the
[Silk.NET library](https://github.com/dotnet/Silk.NET).

# Ideas
You can find many pages dedicated to random terrain generation, for example
[this one on vterrain.org](http://vterrain.org/Elevation/Artificial/) or
[Fractal landscape on Wikipedia](https://en.wikipedia.org/wiki/Fractal_landscape).
One slightly outdated page -
[Terrific: Fast Terrain Rendering](https://www.cosc.brocku.ca/Offerings/4P98/gallery/projects/Winter2008/bg05he/)

I'd recommend simple and efficient
[Diamond-square algorithm](https://en.wikipedia.org/wiki/Diamond-square_algorithm), but
you can use any algorithm which is capable of creating terrains gradually, by increasing
the subdivision (recursion) depth.

# Sil.NET framework
It is easy to use the [Silk.NET](https://github.com/dotnet/Silk.NET) in your C#
program, you just install the [Silk.NET NuGet package](https://www.nuget.org/packages/Silk.NET/).

You can view our sample projects in the
[Silk3D directory](../Silk3D/README.md) of our repository.

# Your solution
Please place your solution in a separate [solutions](solutions/README.md)
directory in the repository. You'll find short instructions there.

# Launch date
**Monday 4 December 2023**
(Don't work on the solution before this date)

# Deadline
See the shared [point table](https://docs.google.com/spreadsheets/d/1QLukOcSRPa5exOYW1eUfQWY2WoMjo1menbjQIU7Gvs4/edit?usp=sharing).

# Credit points
**Basic solution: 10 points**
* Variable **Hausdorff dimension** coefficient (random relative amplitude)
* Variable level of recursion (interactive subdivide/updivide)
* Normal vectors for basic appearance
* Interactive terrain exploration (e.g. using the [Trackball class](../Silk3D/shared/Trackball.cs))

**Bonus points: up to 16+ more points**
* Vertex color according to elevation (2)
* Reasonable terrain texturing (2)
* Initialization from height-map texture (4)
* Infinite terrain concept ... "periodic terrain" (4)
* Hovercraft mode, interactive or scripted (3 to 8)

## Use of AI assistant
It is possible to use an AI assistant, but you have to be critical and
test all its suggestions thoroughly.

# Images
![Diamond-square algorithm](diamond-square-diagram.png)
Diagram of the diamond-square subdivision.

![Screenshot](terrain-screenshot.jpg)
Example of terrain visualization using vertex colors and simple shading.