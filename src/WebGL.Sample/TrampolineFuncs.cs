using System.Runtime.InteropServices;


using Silk.NET.OpenGLES;

namespace WasmTest;

internal static class TrampolineFuncs
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void ClearColor_t(float r, float g, float b, float a);
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void Clear_t(ClearBufferMask mask);

	public static void LoadDelegates()
	{
		// this somehow allows our AOT'd build to call the OpenGL functions
		Marshal.GetDelegateForFunctionPointer(new nint(1), typeof(Clear_t));
	}
}
