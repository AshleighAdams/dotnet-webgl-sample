#version 300 es

layout(location = 0) in highp vec2 in_xy;
layout(location = 1) in highp vec3 in_rgb;

out highp vec3 color;

// GLSL uses the reverse order to a System.Numerics.Matrix3x2
uniform mat2x3 viewprojection;

void main()
{
	gl_Position = vec4(vec3(in_xy, 1.0) * viewprojection, 0.0, 1.0);
	color = in_rgb;
}
