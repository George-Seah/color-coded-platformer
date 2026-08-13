# 🗺️ Level Migration & Rebuilding Guide

**To the Next Arena Agent:**
The user has **15 existing levels** (`Scene 1.unity` through `Scene 15.unity`) located in `Assets/Scenes/`. 

These levels contain carefully placed puzzles, platforms, keys, and level geometry. **Do not delete these scenes or the level geometry.** 

Instead of rebuilding the levels spatially from scratch, your goal is to **Migrate** the legacy scenes to the new Architecture.

---

## 🛠️ How to Rebuild the Levels (Step-by-Step)

When you are ready to transition a scene (e.g., `Scene 1`), follow this exact process:

### 1. Preserve the Layout (Transforms & Sprites)
Do not move objects around or delete the level geometry (Tilemaps, SpriteRenderers, walls, and floors). The spatial puzzle design must remain exactly as the user built it.

### 2. Strip Legacy Scripts
Go through the objects in the scene and remove the old, monolithic legacy scripts:
* Remove `PlayerBehaviour.cs`, `Key.cs`, `Waddler.cs`, `VelocityChecker.cs`, `ESave.cs`, `ColorPositionChange.cs`, etc.

### 3. Inject the New Architecture
Attach the newly programmed, modular scripts to the existing GameObjects:
* **The Player:** Attach the new `PlayerInput`, `PlayerMovement`, `PlayerColorState`, and `PlayerInteraction`. 
* **Color Changers:** Attach the new Event-Driven `ColorChanger` and link the `List<GameObject>` to the existing objects in the room so the TextureSheet particles work.
* **Colored Objects:** Attach the `BaseColorPicker` to platforms, and `DoorColorPicker` to doors. Set their enums in the inspector to match what color they originally were.
* **One-Way Platforms:** Remove the old `VelocityChecker` and add Unity's native `PlatformEffector2D` (check `Use One Way`).
* **Cannons:** Replace legacy cannons with the new `BaseCannon` child classes.

### 4. Update the Physics Layers
Ensure the objects in the scene are no longer relying on the old custom string-based ignore logic. Update their `gameObject.layer` via the `BaseColorPicker` so they interact natively with the new Unity Physics2D Collision Matrix (Default, White, Black, Red, Blue, etc.).

---

## 📋 The Level Inventory

The user has the following progression built:
* **Scene 1 to Scene 15**
* These include tutorial areas, locked doors, push blocks, waddler enemies, cannons, and moving platforms.

By swapping out the *components* rather than the *GameObjects*, you will successfully rebuild the project from scratch while preserving 100% of the user's hard work on level design!