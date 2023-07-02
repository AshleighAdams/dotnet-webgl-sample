# .NET WebAssembly + WebGL

This is a minimum viable product for running WebGL, using dotnet's BlazorWebassembly and WaveEngine's OpenGLES with emscripten, based on @emepetres's wasm sample projects.

## Areas needing work

The trimming is too excessive by default, so this workaround is needed to fix deploys:

```xml
  <ItemGroup>
    <TrimmerRootAssembly Include="Microsoft.AspNetCore.Components.WebAssembly" />
  </ItemGroup>
```

