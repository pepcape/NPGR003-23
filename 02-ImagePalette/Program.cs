using System;
using System.Xml;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace _02_ImagePalette;

class Program
{
  static void Main (string[] args)
  {
    // Input array of Rgba32 colors
    Rgba32[] colors = new Rgba32[]
    {
      new Rgba32(255, 0, 0),    // Red
      new Rgba32(0, 255, 0),    // Green
      new Rgba32(0, 0, 255),    // Blue
      new Rgba32(255, 255, 0),  // Yellow
      new Rgba32(255, 165, 0),  // Orange
      new Rgba32(80, 80, 80)    // Dark gray
    };

    // Define the output SVG file path
    string outputPath = "palette.svg";

    // Define the canvas size in pixels
    int imageWidth = 600;
    int imageHeight = 100;

    // Define the width and height of each color rectangle
    int rectWidth = imageWidth / colors.Length;
    int rectHeight = imageHeight;

    // Create an XML document to represent the SVG
    XmlDocument svgDoc = new XmlDocument();

    // Create the SVG root element
    XmlElement svgRoot = svgDoc.CreateElement("svg");
    svgRoot.SetAttribute("xmlns", "http://www.w3.org/2000/svg");
    svgRoot.SetAttribute("width", imageWidth.ToString());
    svgRoot.SetAttribute("height", imageHeight.ToString());
    svgDoc.AppendChild(svgRoot);

    // Create a group element to contain the color rectangles
    XmlElement group = svgDoc.CreateElement("g");
    svgRoot.AppendChild(group);

    for (int i = 0; i < colors.Length; i++)
    {
      // Create a rectangle element for each color
      XmlElement rect = svgDoc.CreateElement("rect");
      rect.SetAttribute("x", (i * rectWidth).ToString());
      rect.SetAttribute("y", "0");
      rect.SetAttribute("width", rectWidth.ToString());
      rect.SetAttribute("height", rectHeight.ToString());
      rect.SetAttribute("fill", $"#{colors[i].R:X2}{colors[i].G:X2}{colors[i].B:X2}");
      group.AppendChild(rect);
    }

    // Save the SVG document to a file
    svgDoc.Save(outputPath);

    Console.WriteLine($"SVG saved to {outputPath}");
  }
}
