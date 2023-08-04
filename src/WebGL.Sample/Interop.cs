using System;
using System.Runtime.InteropServices.JavaScript;

using Silk.NET.OpenGLES;

namespace WebGL.Sample;

internal static partial class Interop
{
	[JSImport("initialize", "main.js")]
	public static partial void Initialize();

	[JSExport]
	public static void OnKeyDown(bool shift, bool ctrl, bool alt, bool repeat, int code)
	{
	}

	[JSExport]
	public static void OnKeyUp(bool shift, bool ctrl, bool alt, int code)
	{
	}

	[JSExport]
	public static void OnMouseMove(float x, float y)
	{
	}

	[JSExport]
	public static void OnMouseDown(bool shift, bool ctrl, bool alt, int button)
	{
	}

	[JSExport]
	public static void OnMouseUp(bool shift, bool ctrl, bool alt, int button)
	{
	}

	[JSExport]
	public static void OnCanvasResize(float width, float height, float devicePixelRatio)
	{
		Test.CanvasResized((int)width, (int)height);
	}

	[JSExport]
	public static void SetRootUri(string uri)
	{
		Test.BaseAddress = new Uri(uri);
	}

	[JSExport]
	public static void AddLocale(string locale)
	{
	}
}
