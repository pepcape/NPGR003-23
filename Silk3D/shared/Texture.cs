using System;
using System.Numerics;
using Silk.NET.OpenGL;
using SilkHDR;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
// ReSharper disable AssignNullToNotNullAttribute

namespace Util;

public class Texture : DefaultConfigSection, IDisposable
{
	// OpenGL binding.
	protected uint _handle = uint.MaxValue;

	private static GL? _gl;

	// Descriptive parameters.

	/// <summary>
	/// Full path to the image file.
	/// </summary>
	public string fileName = "";

	/// <summary>
	/// Key of the image.
	/// </summary>
	public string name = "";

	/// <summary>
	/// image description for debugging.
	/// </summary>
	public string descr;

	public int Width { get; protected set; }

	public int Height { get; protected set; }

	public bool IsValid()
	{
		return !string.IsNullOrEmpty(name) &&
					 _handle != uint.MaxValue;
	}

	// Constructors.
	public Texture(int width, int height, string nam = "")
	{
		name = nam;
		Width = width;
		Height = height;
		descr = "Will ba generated";
	}

	public Texture(string filename = "", string nam = "")
	{
		name = nam;
		fileName = filename;
		Width = 0;
		Height = 0;
		descr = fileName;
	}

	public unsafe Texture(GL gl, Span<byte> data, int width, int height)
	{
		// Saving the GL instance.
		_gl = gl;

		// Generating the OpenGL handle;
		_handle = gl.GenTexture();
		Bind(gl);

		Width = width;
		Height = height;

		// We want the ability to create a texture using data generated from code as well.
		fixed (void* d = &data[0])
		{
			// Setting the data of a texture.
			gl.TexImage2D(
				TextureTarget.Texture2D,
				0,
				(int)InternalFormat.Rgba,
				(uint)width, (uint)height, 0,
				PixelFormat.Rgba,
				PixelType.UnsignedByte,
				d);
		}

		descr = "bin";
		SetParameters(gl);
	}

	public unsafe void OpenglTextureFromFile(GL gl)
	{
		// Saving the GL instance.
		_gl = gl;

		if (_handle != uint.MaxValue)
			gl.DeleteTexture(_handle);

		_handle = gl.GenTexture();
		Bind(gl);

		if (string.IsNullOrEmpty(fileName))
		{
			// Generated texture.
			GenerateTexture(gl);
			descr = "gen";
		}
		else
		{
			// Texture read from a disk file.
			Width = Height = 0;

			if (fileName.ToLower().EndsWith(".hdr"))
			{
				// Loading a HDR image using FloatImage.
				// PFM and Radiance HDR formats should be implemented.
				FloatImage? fi = FloatImage.FromFile(fileName);
				if (fi == null)
					throw new Exception($"Invalid HDR file {fileName}");

				Width = fi.Width;
				Height = fi.Height;

				fixed (float* d = fi.Data)
				{
					// Setting the data of a texture.
					gl.TexImage2D(
						TextureTarget.Texture2D, 0,
						(int)InternalFormat.Rgb16f,
						(uint)fi.Width, (uint)fi.Height, 0,
						PixelFormat.Rgb,
						PixelType.Float,
						d);
				}

				descr = "hdr";
			}
			else
			{
				// Loading a LDR image using ImageSharp.
				using (var img = Image.Load<Rgba32>(fileName))
				{
					Width = img.Width;
					Height = img.Height;

					// Reserve enough memory from the gpu for the whole image
					gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba8, (uint)img.Width, (uint)img.Height, 0,
						PixelFormat.Rgba, PixelType.UnsignedByte, null);

					img.ProcessPixelRows(accessor =>
					{
						// ImageSharp 2 does not store images in contiguous memory by default, so we must send the image row by row
						for (int y = 0; y < accessor.Height; y++)
						{
							fixed (void* data = accessor.GetRowSpan(y))
							{
								// Loading the actual image.
								gl.TexSubImage2D(TextureTarget.Texture2D, 0, 0, y, (uint)accessor.Width, 1, PixelFormat.Rgba,
									PixelType.UnsignedByte, data);
							}
						}
					});
				}

				descr = "ldr";
			}
		}

		SetParameters(gl);
	}

	public unsafe void GenerateTexture(GL gl)
	{
		// Saving the GL instance.
		_gl = gl;

		fileName = "";
		name = $"gen-{Width}x{Height}";

		// Generated texture data.
		float widHalf = Width * 0.5f;
		float heiHalf = Height * 0.5f;
		float[] p = new float[Width * Height * 3];
		const float scale = 0.1f;
		const float amplitude = 1.0f;

		fixed (float* d = p)
		{
			// Compute HDR image data.
			float* ptr = d;
			for (int y = 0; y < Height; y++)
			{
				float ay = scale * (y - heiHalf);
				for (int x = 0; x < Width; x++)
				{
					float ax = scale * (x - widHalf);
					float radius2 = ay * ay + ax * ax + 1.0E-6f;
					float value = amplitude * (float)Math.Sin(radius2) / radius2;
					*ptr++ = Math.Abs(value * ax);
					*ptr++ = Math.Abs(value * ay);
					*ptr++ = Math.Abs(value);
				}
			}

			// Setting the data of a texture.
			gl.TexImage2D(
				TextureTarget.Texture2D, 0,
				(int)InternalFormat.Rgb16f,
				(uint)Width, (uint)Height, 0,
				PixelFormat.Rgb,
				PixelType.Float,
				d);
		}
	}

	private void SetParameters(GL gl)
	{
		// Setting some texture parameters so the texture behaves as expected.
		gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)GLEnum.ClampToEdge);
		gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)GLEnum.ClampToEdge);

		gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)GLEnum.Linear);
		gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)GLEnum.Linear);
		//gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, 0);
		//gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, 8);
		// Generating MipMaps.
		//gl.GenerateMipmap(TextureTarget.Texture2D);
	}

	public void Bind(GL gl, TextureUnit textureSlot = TextureUnit.Texture0)
	{
		// When we bind a texture we can choose which texture-slot we can bind it to.
		gl.ActiveTexture(textureSlot);
		gl.BindTexture(TextureTarget.Texture2D, _handle);
	}

	public void Dispose()
	{
		// In order to dispose we need to delete the OpenGL handle for the texure.
		_gl?.DeleteTexture(_handle);
		_handle = uint.MaxValue;
	}

	// ConfigSection

	/// <summary>
	/// Pre-processing of the section.
	/// </summary>
	/// <param name="line">The config line which initiated the object read.</param>
	/// <param name="provider">Superior object which is in charge of reading the config...</param>
	/// <returns>Itself or a proxy-reader.</returns>
	public override IConfigSection Prologue(string line, Options? provider)
	{
		name = string.Empty;

		if (provider is SilkOptions)
			SilkOptions.images.Add(name, this);

		return this;
	}

	/// <summary>
	/// Parse a key-value pair.
	/// </summary>
	/// <param name="key">Key string (non-empty, trimmed).</param>
	/// <param name="value">Value string (non-null, trimmed).</param>
	/// <param name="line">Raw line.</param>
	/// <param name="finished">Set to true if the object was completed.</param>
	/// <returns>True if recognized.</returns>
	public override bool HandleKeyValue(string key, string value, string line, out bool finished)
	{
		finished = false;

		switch (key)
		{
			case "name":
				// name = <string>
				SilkOptions.images.Remove(name);
				name = Options.UnwrapString(value);
				SilkOptions.images[name] = this;
				SilkOptions.currentImageName = name;
				return true;

			case "file":
				// file = <string>
				fileName = Options.UnwrapString(value);
				descr = fileName;
				return true;
		}

		return false;
	}
}
