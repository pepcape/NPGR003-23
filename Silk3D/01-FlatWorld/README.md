# `01-FlatWorld`
You can place 2D objects (triangles) on the plane. Elementary
interaction using keyboard, matrix transformations.

# Notes
* matrix transformations
  - concatenation
  - **model transform** places an obect into the **world space**
  - **view transform** takes care of scene visibility (2D "orthographics" projection
	is used here)
* minimal set of OpenGL shaders
  - **vertex shader** - performs "model-view-transform"
  - **fragment shader** - assigns color interpolated from vertices, no textures are used
