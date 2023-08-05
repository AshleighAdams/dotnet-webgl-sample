using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading.Tasks;

using Silk.NET.OpenGLES;

[assembly: SupportedOSPlatform("browser")]

namespace WebGL.Sample;

public static class Test
{
	public static Uri? BaseAddress { get; internal set; }
	private static MeshDemo? Demo { get; set;  }
	[UnmanagedCallersOnly]
	public static int Frame(double time, nint userData)
	{
		ArgumentNullException.ThrowIfNull(Demo);

		Demo.Render();

		return 1;
	}

	private static int CanvasWidth { get; set; }
	private static int CanvasHeight { get; set; }
	public static void CanvasResized(int width, int height)
	{
		CanvasWidth = width;
		CanvasHeight = height;
		Demo?.CanvasResized(CanvasWidth, CanvasHeight);
	}

	public async static Task Main(string[] args)
	{
		Console.WriteLine($"Hello from dotnet!");

		var display = EGL.GetDisplay(IntPtr.Zero);
		if (display == IntPtr.Zero)
			throw new Exception("Display was null");

		if (!EGL.Initialize(display, out int major, out int minor))
			throw new Exception("Initialize() returned false.");

		int[] attributeList = new int[]
		{
			EGL.EGL_RED_SIZE  , 8,
			EGL.EGL_GREEN_SIZE, 8,
			EGL.EGL_BLUE_SIZE , 8,
			EGL.EGL_DEPTH_SIZE, 24,
			EGL.EGL_STENCIL_SIZE, 8,
			EGL.EGL_SURFACE_TYPE, EGL.EGL_WINDOW_BIT,
			EGL.EGL_RENDERABLE_TYPE, EGL.EGL_OPENGL_ES3_BIT,
			EGL.EGL_SAMPLES, 16, //MSAA, 16 samples
			EGL.EGL_NONE
		};

		var config = IntPtr.Zero;
		var numConfig = IntPtr.Zero;
		if (!EGL.ChooseConfig(display, attributeList, ref config, (IntPtr)1, ref numConfig))
			throw new Exception("ChoseConfig() failed");
		if (numConfig == IntPtr.Zero)
			throw new Exception("ChoseConfig() returned no configs");

		if (!EGL.BindApi(EGL.EGL_OPENGL_ES_API))
			throw new Exception("BindApi() failed");

		int[] ctxAttribs = new int[] { EGL.EGL_CONTEXT_CLIENT_VERSION, 3, EGL.EGL_NONE };
		var context = EGL.CreateContext(display, config, (IntPtr)EGL.EGL_NO_CONTEXT, ctxAttribs);
		if (context == IntPtr.Zero)
			throw new Exception("CreateContext() failed");

		// now create the surface
		var surface = EGL.CreateWindowSurface(display, config, IntPtr.Zero, IntPtr.Zero);
		if (surface == IntPtr.Zero)
			throw new Exception("CreateWindowSurface() failed");

		if (!EGL.MakeCurrent(display, surface, surface, context))
			throw new Exception("MakeCurrent() failed");

		//_ = EGL.DestroyContext(display, context);
		//_ = EGL.DestroySurface(display, surface);
		//_ = EGL.Terminate(display);

		TrampolineFuncs.ApplyWorkaroundFixingInvocations();

		var gl = GL.GetApi(EGL.GetProcAddress);

		Interop.Initialize();
		ArgumentNullException.ThrowIfNull(BaseAddress);

		Demo = await MeshDemo.LoadAsync(gl, BaseAddress);
		Demo?.CanvasResized(CanvasWidth, CanvasHeight);

		unsafe
		{
			Emscripten.RequestAnimationFrameLoop((delegate* unmanaged<double, nint, int>)&Frame, nint.Zero);
		}
	}
}
