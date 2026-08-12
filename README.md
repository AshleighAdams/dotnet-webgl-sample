

# .NET WebGL sample

This is a minimum viable product for running WebGL, using dotnet's WebAssembly and Silk's OpenGLES bindings with emscripten.

Thanks to WaveEngine/@emepetres's wasm sample projects for help and original samples and references.

If you want to separate rendering and animation logic into different threads, you can refer to this fork: [XiaofengdiZhu/dotnet-webgl-mt-sample](https://github.com/XiaofengdiZhu/dotnet-webgl-mt-sample)

## Build and Run

1. Install the .NET WebAssembly workload:
   ```bash
   dotnet workload install wasm-tools
   ```
2. Run the project:
   ```bash
   dotnet run
   ```
