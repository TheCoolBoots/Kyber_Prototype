Thank you for purchasing AQUAS 2020!
The current version is 1.1.1!

AQUAS 2020 comes with a variety of new features and is superior to AQUAS 1.5. Apart from adding powerful features, ease of use and compatibility has been increased, while the requirement of the post processing stack has been removed entirely.

ATTENTION:
The minimum Unity version for AQUAS 2020 is Unity 2018.1.

For video tutorials and the latest manual please visit the following link:

https://dogmaticgames.wordpress.com/products/aquas-2020/tutorials/

Have Fun!

Changelog:

v1.1.1
- Removed an unneeded script
- Fixed an issue that caused Editor crashes when internal jobs tried to write to virtual addresses they don't have appropriate access to.
- Fixed a frustum culling issue that caused dynamic meshes to be culled in builds only.
- Fixed the water setup that caused the water to be rendered black on Android devices.

v1.1
- Added infinite water with dynamic mesh to produce physical waves close to the camera
- Added ripple effects
- Improved caustics and fixed removed grab pass from caustics shader to prevent crashing the build
- Added the possibility to set layer masks for underwater effects and depth presampling, which improves performance
- Adjusted all features to work with both static and dynamic meshes
- Improved realtime reflections by selecting out certain cameras that shouldn't reflect anything
- Added color option to shallow water shader to allow the rendering of dirty water
- Added additional presets to the setup wizard for shallow water

v1.0.1
- Fixed a light attenuation issue with point lights shining on the water
- Changed the scripting define Symbol from AQUAS_PRESENT to AQUAS_2020_PRESENT to avoid conflicts with 3rd party integrations.
- Changed hideflags for runtime objects to avoid them getting accidentially saved