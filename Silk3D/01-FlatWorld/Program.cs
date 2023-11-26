using System;
using System.Diagnostics;
using CommandLine;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Silk.NET.Maths;
using System.Numerics;
using Util;

// ReSharper disable InconsistentNaming
// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace _01_FlatWorld;

public class Options
{
  [Option('w', "width", Required = false, Default = 800, HelpText = "Window width in pixels.")]
  public int WindowWidth { get; set; } = 800;

  [Option('h', "height", Required = false, Default = 600, HelpText = "Window height in pixels.")]
  public int WindowHeight { get; set; } = 600;
}

/// <summary>
/// Single object = sequence of primitives of the same type (point, line, triangle).
/// </summary>
struct Object
{
  /// <summary>
  /// Buffer id. Not used here as we have only one shared buffer.
  /// </summary>
  public uint BufferId { get; set; }

  public PrimitiveType Type { get; set; }

  /// <summary>
  /// Start of the object in the buffer (in indices).
  /// </summary>
  public int BufferOffset { get; set; }

  /// <summary>
  /// Number of indices (should be multiple of two for lines, multiple of three for triangles).
  /// </summary>
  public int Indices { get; set; }
}

internal class Program
{
  private static IWindow? window;
  private static GL? Gl;

  // Global 3D data buffer.
  private const int MAX_INDICES = 2048;
  private const int MAX_VERTICES = 1024;
  private const int VERTEX_SIZE = 6;

  private static List<uint> indexBuffer = new(MAX_INDICES);
  private static List<float> vertexBuffer = new(MAX_VERTICES * VERTEX_SIZE);

  private static BufferObject<float>? Vbo;
  private static BufferObject<uint>? Ebo;
  private static VertexArrayObject<float, uint>? Vao;

  // Shader program.
  private static ShaderProgram? ShaderPrg;

  // 2D objects - referring to the shared buffer.
  private static List<Object> Objects = new();

  //////////////////////////////////////////////////////
  // Application.

  private static string WindowTitle()
  {
    return $"01-FlatWorld - {Objects.Count} objects";
  }

  private static void Main(string[] args)
  {
    Parser.Default.ParseArguments<Options>(args)
      .WithParsed<Options>(o =>
      {
        WindowOptions options = WindowOptions.Default;
        options.Size = new Vector2D<int>(o.WindowWidth, o.WindowHeight);
        options.Title = WindowTitle();
        window = Window.Create(options);

        window.Load    += OnLoad;
        window.Render  += OnRender;
        window.Closing += OnClose;

        window.Run();
      });
  }

  private static void VaoPointers()
  {
    Debug.Assert(Vao != null);
    Vao.Bind();
    Vao.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, 6, 0);
    Vao.VertexAttributePointer(1, 3, VertexAttribPointerType.Float, 6, 3);
  }

  private static void OnLoad()
  {
    Debug.Assert(window != null);

    IInputContext input = window.CreateInput();
    for (int i = 0; i < input.Keyboards.Count; i++)
    {
      input.Keyboards[i].KeyDown += KeyDown;
      input.Keyboards[i].KeyUp   += KeyUp;
    }
    for (int i = 0; i < input.Mice.Count; i++)
    {
      input.Mice[i].MouseDown   += MouseDown;
      input.Mice[i].MouseUp     += MouseUp;
      input.Mice[i].MouseMove   += MouseMove;
      input.Mice[i].DoubleClick += MouseDoubleClick;
      input.Mice[i].Scroll      += MouseScroll;
    }

    Gl = GL.GetApi(window);

    //------------------------------------------------------
    // Render data.

    // Init: one triangle
    vertexBuffer.AddRange(new[]
    {
      // x,     y,     z,     R,     G,     B
      -0.5f, -0.5f,  0.0f,  1.0f,  0.2f,  0.2f,
       0.5f, -0.5f,  0.0f,  0.1f,  1.0f,  0.1f,
       0.0f,  0.6f,  0.0f,  0.3f,  0.3f,  1.0f,
    });
    indexBuffer.AddRange(new uint[]
    {
      0, 1, 2,
    });
    Objects.Add(new()
    {
      BufferId = 0,
      Type = PrimitiveType.Triangles,
      BufferOffset = 0,
      Indices = 3
    });

    Ebo = new BufferObject<uint>(Gl, indexBuffer.ToArray(), BufferTargetARB.ElementArrayBuffer);
    Vbo = new BufferObject<float>(Gl, vertexBuffer.ToArray(), BufferTargetARB.ArrayBuffer);
    Vao = new VertexArrayObject<float, uint>(Gl, Vbo, Ebo);
    VaoPointers();

    ShaderPrg = new ShaderProgram(Gl, "shader.vert", "shader.frag");

    // Main window.
    window.Title = WindowTitle();
  }

  private static unsafe void OnRender(double obj)
  {
    Debug.Assert(Gl != null);
    Debug.Assert(ShaderPrg != null);

    Gl.Clear((uint)ClearBufferMask.ColorBufferBit);

    // 1. Transformed raster image.
    VaoPointers();
    ShaderPrg.Use();

    // Draw the objects.
    foreach (var o in Objects)
    {
      // Uniforms.
      //ShaderPrg.TrySetUniform("exposure", SilkOptions.exposure);

      Gl.DrawElements(o.Type, (uint)o.Indices, DrawElementsType.UnsignedInt, (void*)(o.BufferOffset * sizeof(float)));
    }
  }

  private static void OnClose()
  {
    Vao?.Dispose();
    ShaderPrg?.Dispose();

    // Remember to dispose the textures.
    //DisposeTextures();
  }

  private static int shiftDown = 0;

  private static int ctrlDown = 0;

  private static void KeyDown(IKeyboard arg1, Key arg2, int arg3)
  {
    switch (arg2)
    {
      case Key.ShiftLeft:
      case Key.ShiftRight:
        shiftDown++;
        break;

      case Key.ControlLeft:
      case Key.ControlRight:
        ctrlDown++;
        break;

      /*
      case Key.Up:
        break;

      case Key.Down:
        break;

      case Key.Left:
        break;

      case Key.Right:
        break;

      case Key.Home:
        break;

      case Key.F:
        break;

      case Key.S:
        if (ctrlDown > 0)
        {
          // Ctrl+S.
        }
        break;

      case Key.F5:
        break;
      */

      case Key.Escape:
        window?.Close();
        break;
    }
  }

  private static void KeyUp(IKeyboard arg1, Key arg2, int arg3)
  {
    switch (arg2)
    {
      case Key.ShiftLeft:
      case Key.ShiftRight:
        shiftDown--;
        break;

      case Key.ControlLeft:
      case Key.ControlRight:
        ctrlDown--;
        break;
    }
  }

  private static void MouseDown(IMouse mouse, MouseButton btn)
  {
    if (btn == MouseButton.Left)
    {
      Util.Util.MessageInvariant($"Left button down: {mouse.Position}");
    }
  }

  private static void MouseUp(IMouse mouse, MouseButton btn)
  {
    if (btn == MouseButton.Left)
    {
      Util.Util.MessageInvariant($"Left button up: {mouse.Position}");
    }
  }

  private static void MouseMove(IMouse mouse, Vector2 xy)
  {
    if (mouse.IsButtonPressed(MouseButton.Left))
      Util.Util.MessageInvariant($"Mouse drag: {xy}");
  }

  private static void MouseDoubleClick(IMouse mouse, MouseButton btn, Vector2 xy)
  {
    if (btn == MouseButton.Left)
    {
      Util.Util.Message("Closed by double-click.", true);
      window?.Close();
    }
  }

  private static void MouseScroll(IMouse mouse, ScrollWheel wheel)
  {
    // wheel.Y is -1 or 1
    Util.Util.MessageInvariant($"Mouse scroll: {wheel.Y}");
  }
}
