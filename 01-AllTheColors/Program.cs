using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace _01_AllTheColors;

internal class Program
{
  // Constants for image dimensions and tile size
  private const int Width = 600;
  private const int Height = 450;
  private const int TileSize = 10;

  // Constants for tile colors
  private static readonly Rgba32 BlueColor = new Rgba32(0x20, 0x20, 0xFF); // Blue (#2020ff)
  private static readonly Rgba32 RedColor = new Rgba32(0xFF, 0x20, 0x20); // Red (#ff2020)

  // Constant for the output filename
  private const string OutputFilename = "checkerboard.png";

  static void Main (string[] args)
  {
    // Create a new image with the specified dimensions
    using (var image = new Image<Rgba32>(Width, Height))
    {
      for (int y = 0; y < Height; y++)
      {
        for (int x = 0; x < Width; x++)
        {
          // Determine the tile color based on position
          Rgba32 tileColor = ((x / TileSize) + (y / TileSize)) % 2 == 0
            ? BlueColor // Blue for even tiles
            : RedColor; // Red for odd tiles

          // Set the pixel color
          image[x, y] = tileColor;
        }
      }

      // Save the image to a file with the specified filename
      image.Save(OutputFilename);

      Console.WriteLine($"Image '{OutputFilename}' created successfully.");
    }
  }
}
