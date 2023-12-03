#version 330 core

layout (location = 0) in vec3 vPos;
layout (location = 1) in vec3 vColor;
layout (location = 2) in vec2 vTxt;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

out vec3 fColor;
out vec2 fTxt;
out vec4 fWorld;

void main()
{
    // World-space coordinates.
    fWorld = model * vec4(vPos, 1.0);

    // Model- and then the view-transform.
    gl_Position = projection * view * fWorld;

    // Setting the colors/texture-coordinates on the vertices will mean they get correctly divided out amongst the fragments.
    fColor = vColor;
    fTxt = vTxt;
}
