# 🧪 Color-Coded Platformer: Mechanics Prototyping Guide

**To the Next Arena Agent:**
Since this is a fresh build, your first task is to write the backend logic and create isolated **Test Rooms/Scenes** to ensure the new mechanics work perfectly. 

---

## 1. Prototype Rooms to Build

* **Room 1 - Cannons & Inheritance:** Create the `BaseCannon` script and its derivatives. Test aiming them by rotating their Transforms.
* **Room 2 - Color Pickers, Changers & Particle Sprites:** Implement `BaseColorPicker.cs` and `DoorColorPicker.cs`. Test `ColorChanger` triggers—verify their Particle Systems dynamically grab the Sprites of the objects in their `List<GameObject>` via `textureSheetAnimation.AddSprite()`.
* **Room 3 - Position-Based Colors:** Test `PositionBasedColorChanger` using BOTH horizontal (X) and vertical (Y) slicing. Implement the UI tracking bar and gradient boundary previews.
* **Room 4 - Action-Determined Color:** Auburn's color changes based on actions (Jumping = Yellow, Falling = Blue, Landing = Red). Build a 2-room obstacle course requiring these actions to pass colored gates.
* **Room 5 - Color Conducting (Broadcast):** Add an Inspector Enum to toggle between *Continuous Mode* (platforms match player velocity) and *Event Mode* (platforms move incrementally on jump). Test the "Sync Glow" visual telegraph.
* **Room 6 - Pigment Conservation:** Test Color Economy. The player absorbs color from Object A (turning A White) and transfers it to Object B (turning B Red).
* **Room 7 - Anti-Observation:** Quantum objects. A bridge fades into a sketch and loses its collider if looked at. Use `Vector2.Dot` for facing direction and Viewport bounds for camera vision.
* **Room 8 - The Mirror (Clone):** Test picking up the Mirror. It spawns a clone that shares player inputs. Design a puzzle where the clone must be springboarded off the main player's head.
* **Room 9 - Platforms & Clouds:** Ensure that jumping UP through a `PlatformEffector2D` doesn't trigger the ground check early. Ensure jumping onto a moving Cloud properly parents the player without breaking ground checks.
* **Room 10 - Ladders, Fences & Semi-Solids:** Test the stacked-sprite Ladder/Fence script. Ensure climbing it sets the Player to `Kinematic`, perfectly ignores the new Semi-Solid layer, but still registers getting hit by a Dynamic Cannonball.

---

## 2. Aesthetics Note
The user plans to add Kuwahara filters, dot grain, bloom, and oil painting styles later. **These are purely aesthetic.** Do NOT tie game physics to post-processing. Use the explicit Unity `Materials` mapped in the Color Dictionary so shaders can be applied to those materials later.