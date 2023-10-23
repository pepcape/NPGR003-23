using CommandLine;
using SixLabors.ImageSharp.ColorSpaces.Conversion;

namespace _03_SFC;

public class Options
{
  [Option('i', "input", Required = true, Default = "", HelpText = "Input file-name.")]
  public string FileName { get; set; } = string.Empty;

  [Option('m', "method", Required = false, Default = 0, HelpText = "Pixel order method (-1 for the best one).")]
  public int Method { get; set; } = 0;

  [Option('p', "predictor", Required = false, Default = 1, HelpText = "Predictor order.")]
  public int Predictor { get; set; } = 1;
}

internal class Program
{
  static void Main(string[] args)
  {

    Parser.Default.ParseArguments<Options>(args)
      .WithParsed<Options>(o =>
      {
        if (string.IsNullOrEmpty(o.FileName) ||
            !File.Exists(o.FileName))
        {
          Console.WriteLine($"Invalid image '{o.FileName}'.");
          return;
        }

        Image<Rgba32> image;
        try
        {
          var im = Image.Load(o.FileName);
          image = im.CloneAs<Rgba32>();
        }
        catch (Exception e)
        {
          Console.WriteLine(e);
          return;
        }
        int width  = image.Width;
        int height = image.Height;

        // We can proceed (image, width, height are valid).

        // Image processing: Predictor, PredictiveEncoder, EntropyCalculator.
        EntropyCalculator entropyCalculator = new();
        IPredictor predictor;
        switch (o.Predictor)
        {
          case 1:
          default:
            predictor = new LinearPredictor1();
            break;
        }
        PredictiveEncoder encoder = new(predictor, entropyCalculator);

        // What to do with the individual pixel?
        Action<int, int> pixelAction = (x, y) =>
        {
          if (x < 0 || x >= width ||
              y < 0 || y >= height)
          {
            Console.WriteLine($"Invalid image access[{x},{y}].");
            return;
          }

          var pixel = image[x, y];

          // Color channel processing: default behavior is to compute gray value (Y).
          var YCRCB = ColorSpaceConverter.ToYCbCr(pixel);
          int gray = (int)Math.Round(YCRCB.Y);

          // Pass the value to the encoder.
          encoder.Put(gray);
        };

        // Pixel order method.
        IPixelOrder sfc;
        switch (o.Method)
        {
          case 0:
          case -1:
            sfc = new ScanLine(pixelAction);
            break;

          default:
            Console.WriteLine($"Invalid method {o.Method}.");
            return;
        }

        // Pass the image.
        sfc.Pass(width, height);

        // Compute and print the entropy.
        long entropy = entropyCalculator.Entropy();
        Console.WriteLine($"{entropy}");
        Console.WriteLine($"Image: {o.FileName}[{width}x{height}]");
        Console.WriteLine($"Order: ScanLine");
        Console.WriteLine(FormattableString.Invariant($"Average entropy: {entropy/(double)(width * height):f2} bits per pixel"));
      });
  }
}
