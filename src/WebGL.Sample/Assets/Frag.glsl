#version 300 es

in highp vec3 color;

layout(location = 0) out highp vec4 diffuse;

void main()
{
	diffuse = vec4(color, 1.0);
}
