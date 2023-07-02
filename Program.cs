using System;
using System.Runtime.InteropServices;

using Microsoft.JSInterop.WebAssembly;
using Evergine.Bindings.OpenGL;
using Microsoft.JSInterop;

namespace WasmTest;

internal class MyRuntime : WebAssemblyJSRuntime
{
	public MyRuntime()
	{
	}
}

[System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA2101:Specify marshaling for P/Invoke string arguments", Justification = "Incorrect warning generated")]
internal static class EGL
{
	public const string LibEgl = "libEGL";
	public const int EGL_NONE = 0x3038;
	public const int EGL_RED_SIZE = 0x3024;
	public const int EGL_GREEN_SIZE = 0x3023;
	public const int EGL_BLUE_SIZE = 0x3022;
	public const int EGL_DEPTH_SIZE = 0x3025;
	public const int EGL_STENCIL_SIZE = 0x3026;
	public const int EGL_SURFACE_TYPE = 0x3033;
	public const int EGL_RENDERABLE_TYPE = 0x3040;
	public const int EGL_SAMPLES = 0x3031;
	public const int EGL_WINDOW_BIT = 0x0004;
	public const int EGL_OPENGL_ES2_BIT = 0x0004;
	public const int EGL_OPENGL_ES3_BIT = 0x00000040;
	public const int EGL_CONTEXT_CLIENT_VERSION = 0x3098;
	public const int EGL_NO_CONTEXT = 0x0;
	public const int EGL_NATIVE_VISUAL_ID = 0x302E;
	public const int EGL_OPENGL_ES_API = 0x30A0;

	[DllImport(LibEgl, EntryPoint = "eglGetProcAddress", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
	[DefaultDllImportSearchPaths(DllImportSearchPath.SafeDirectories)]
	public static extern IntPtr GetProcAddress(string proc);

	[DllImport(LibEgl, EntryPoint = "eglGetDisplay", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
	[DefaultDllImportSearchPaths(DllImportSearchPath.SafeDirectories)]
	public static extern IntPtr GetDisplay(IntPtr displayId);

	[DllImport(LibEgl, EntryPoint = "eglInitialize", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
	[DefaultDllImportSearchPaths(DllImportSearchPath.SafeDirectories)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool Initialize(IntPtr display, out int major, out int minor);


	[DllImport(LibEgl, EntryPoint = "eglChooseConfig", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
	[DefaultDllImportSearchPaths(DllImportSearchPath.SafeDirectories)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool ChooseConfig(IntPtr dpy, int[] attribList, ref IntPtr configs, IntPtr configSize/*fixed to 1*/, ref IntPtr numConfig);

	[DllImport(LibEgl, EntryPoint = "eglBindAPI", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
	[DefaultDllImportSearchPaths(DllImportSearchPath.SafeDirectories)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool BindApi(int api);

	[DllImport(LibEgl, EntryPoint = "eglCreateContext", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
	[DefaultDllImportSearchPaths(DllImportSearchPath.SafeDirectories)]
	public static extern IntPtr CreateContext(IntPtr/*EGLDisplay*/ display, IntPtr/*EGLConfig*/ config, IntPtr shareContext, int[] attribList);

	[DllImport(LibEgl, EntryPoint = "eglGetConfigAttrib", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
	[DefaultDllImportSearchPaths(DllImportSearchPath.SafeDirectories)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool GetConfigAttrib(IntPtr/*EGLDisplay*/ display, IntPtr/*EGLConfig*/ config, IntPtr attribute, ref IntPtr value);

	[DllImport(LibEgl, EntryPoint = "eglCreateWindowSurface", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
	[DefaultDllImportSearchPaths(DllImportSearchPath.SafeDirectories)]
	public static extern IntPtr CreateWindowSurface(IntPtr display, IntPtr config, IntPtr win, IntPtr attribList/*fixed to NULL*/);

	[DllImport(LibEgl, EntryPoint = "eglDestroySurface", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
	[DefaultDllImportSearchPaths(DllImportSearchPath.SafeDirectories)]
	public static extern int DestroySurface(IntPtr display, IntPtr surface);

	[DllImport(LibEgl, EntryPoint = "eglDestroyContext", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
	[DefaultDllImportSearchPaths(DllImportSearchPath.SafeDirectories)]
	public static extern int DestroyContext(IntPtr display, IntPtr ctx);

	[DllImport(LibEgl, EntryPoint = "eglMakeCurrent", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
	[DefaultDllImportSearchPaths(DllImportSearchPath.SafeDirectories)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool MakeCurrent(IntPtr display, IntPtr draw, IntPtr read, IntPtr ctx);

	[DllImport(LibEgl, EntryPoint = "eglTerminate", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
	[DefaultDllImportSearchPaths(DllImportSearchPath.SafeDirectories)]
	public static extern int Terminate(IntPtr display);

	[DllImport(LibEgl, EntryPoint = "eglSwapBuffers", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
	[DefaultDllImportSearchPaths(DllImportSearchPath.SafeDirectories)]
	public static extern int SwapBuffers(IntPtr display, IntPtr surface);

	[DllImport(LibEgl, EntryPoint = "eglSwapInterval", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
	[DefaultDllImportSearchPaths(DllImportSearchPath.SafeDirectories)]
	public static extern int SwapInterval(IntPtr display, int interval);
}

internal static class Emscripten
{
	[DllImport("emscripten", EntryPoint = "emscripten_request_animation_frame_loop")]
	[DefaultDllImportSearchPaths(DllImportSearchPath.SafeDirectories)]
	internal static extern unsafe void RequestAnimationFrameLoop(void* f, nint userDataPtr);
}

public static class Test
{
	[UnmanagedCallersOnly]
	public static int Frame(double time, nint userData)
	{
		GL.glClearColor(0.7f, 0.2f, 1.0f, 1.0f);
		GL.glClear((uint)(AttribMask.ColorBufferBit));

		return 1;
	}

	public static int Main(string[] args)
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

		GL.LoadAllFunctions(EGL.GetProcAddress);

		// https://github.com/emepetres/dotnet-wasm-sample/blob/main/src/jsinteraction/wasm/WebAssemblyRuntime.cs
		using var runtime = new MyRuntime();
		runtime.InvokeVoid("initialize");

		unsafe
		{
			Emscripten.RequestAnimationFrameLoop((delegate* unmanaged<double, nint, int>)&Frame, nint.Zero);
		}

		return 0;
	}
}
