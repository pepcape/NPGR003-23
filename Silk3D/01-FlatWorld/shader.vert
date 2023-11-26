#version 330 core

layout (location = 0) in vec3 vPos;
layout (location = 1) in vec3 vColor;

out vec3 fColor;

void main()
{
    gl_Position = vec4(vPos, 1.0);
    // Setting the colors on the vertices will mean they get correctly divided out amongst the fragments.
    fColor = vColor;
}
