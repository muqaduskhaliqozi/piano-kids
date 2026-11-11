# Package name
Cherry Red Snare Drum with Drumsticks

# Category
3D/Props/Interior

# Summary
A cherry-red snare drum and two separate hickory drumsticks with 2K PBR materials, colliders and URP prefabs.

# Description
A realistic static cherry-red snare drum with polished chrome hardware, coated heads, sixteen underside snare strands and two independently placeable hickory drumsticks. Three placeable items, two object types: the sticks intentionally share the same geometry and material design. No stand, drum kit, sounds, playing logic, rig, animation or additional LOD variants.

Geometry: complete kit 13,072 triangles; drum 11,984; each stick 544. Drum shell diameter 35.6 cm; overall hardware bounds 39.1 × 37.4 × 16.0 cm. Each stick is 40.5 × 1.4 × 1.4 cm. Drum pivot is base-centre, stick pivot at the handle butt. Standalone Blender uses metres/+Z up. Independent engine meshes use their engine axis conventions and real-world units.

Five PBR materials: cherry lacquer, chrome, coated membrane, hickory and plain rubber. Fifteen original 2048×2048 PNG maps: four base colours, four roughness, four metallic and three OpenGL normals. Rubber uses constants; chrome has no unnecessary normal map. Intentionally shared tile UVs occur on uniform hardware/rubber and between identical sticks/heads; unique UVs are not claimed for the entire kit. Directional woodgrain mapping and thin hidden skirt texel density are deliberate. AO is not baked; engine ORM AO is constant 1. Blender lacquer clearcoat and normal strength are approximated in native real-time materials.

Blender source preserves editable quads/triangles in 115 mechanically separate closed component objects. Export BLEND/FBX/OBJ are explicitly triangulated. Engine versions combine the drum components into one mesh with four material sections; each stick is a separate mesh. Three engine mesh files and three independently usable props are supplied; identical sticks are not advertised as distinct unique designs.

# Technical details
- Number of textures: 11 product PNGs; 1 optional 128px presentation cubemap, separately counted.
- Texture dimensions: product PNGs 2048 × 2048; presentation cubemap 128 × 128 per face.
- Polygon count of Snare Drum: 11,984 triangles. Each drumstick: 544 triangles.
- Minimum polygon count: 544 triangles per independent prop.
- Maximum polygon count: 11,984 triangles per independent prop.
- Number of meshes/prefabs: 3 visible mesh assets / 3 saved prefabs; 2 unique object designs. One auxiliary drum collision mesh is not a visible prop.
- Rigging: No
- Animation count: 0
- Animation type list: None
- UV mapping: Yes; 1 UV channel, intentional tile reuse.
- LOD information: 0 additional variants; no LODGroup.
- Types of materials and texture maps: 5 URP/Lit PBR materials, BaseColor, Metallic/Smoothness, tangent Normal; rubber constants.

# Documentation
Tested Unity 6000.6.1f1, URP 17.6.0 on Windows. Other versions, Built-in/HDRP and device platforms are untested. Import the Unity_URP.unitypackage into a URP project, then drag a file from Assets/Cherry_Red_Snare_Drum/Prefabs into an empty scene. Snare_Drum, Drumstick_01 and Drumstick_02 are real saved prefabs with identity roots, metres and assigned URP/Lit materials. The drum has one convex 16-sided cylinder approximation; each stick has one capsule approximation. Static colliders are supplied; no Rigidbody or interaction logic is included. Save your lighting and render-pipeline settings before adopting the optional product pipeline asset. Overview.unity is a demonstration only; its floor, lights, camera and 128px studio reflection cubemap are presentation aids.

Unity uses eleven 2K PNGs: four BaseColor, four MetallicSmoothness and three Normal_OpenGL. BaseColor is sRGB; packed metallic/smoothness is linear with metallic in R and smoothness=1−roughness in A. URP Smoothness source=Metallic Alpha, multiplier=1. Normal maps are imported as Normal Map; normal strength=.25. Hardware UV reuse is intentional. No runtime scripts or external assets are required beyond URP. Engine builtin/package shader dependencies are expected.

# Keywords
snare, drum, drumsticks, percussion, musical instrument, music, prop, realistic, PBR, URP, chrome, hickory, studio, stage

# AI description
OpenAI image generation was used for concept references. Tencent Hunyuan 3D V3.1 generated initial geometry. Codex-assisted Blender Python scripts rebuilt distorted hard surfaces, created clean editable topology and UVs, produced procedural PBR textures, and exported and checked the files. Drumsticks were reconstructed with economical lathed topology; their original Hunyuan FBX downloads failed. No hand-sculpted or hand-painted claim is made. Preview images render the delivered model.
