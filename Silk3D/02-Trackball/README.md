# `02-Trackball`
3D cube is examined using mouse in the "trackball" mode.

## Keyboard
**Esc**
Quits the application.

**F1**
Help is printed to the console window.

## Mouse
**Left button**
Rotating of the object in front of the viewer. Trackball mode is implemented.

**Wheel**
Scale of the current object.

# Notes
* matrix transformations
  - concatenation of three matrices (uniforms) in the vertex shader:
    - **model transform** places an object into the **world space**
    - **view transform** takes care of scene visibility
    - **projection transform** - 2D "orthographics" projection is used
* minimal set of OpenGL shaders
  - **vertex shader** - performs "model-view-transform"
  - **fragment shader** - assigns color interpolated from vertices, no textures are used
* **window resize** handling
  - the defined scene region (diameter = 2) is always displayed
