# 🧠 AI Agent Memory & Context Handoff

**To the Next Arena Agent:**
The user is transferring their session to you because this chat thread has become too long and computationally heavy. You are taking over development of **Color Coded Platformer**, a puzzle-platformer in Unity.

I am your predecessor. Below is the exact, compressed context of everything we accomplished, the current state of the repository, and the exact instructions for what you need to do next.

---

## 1. The Core Architecture (Already Built)
We recently completely overhauled the core codebase to fix legacy bugs. *Do not rebuild these systems; they are already integrated and working.*

*   **The Layer Matrix:** We created `PhysicsMatrixSetup.cs`. Unity's layers are mapped to specific colors. Red hits Red/Orange/Purple. White hits everything. Black hits nothing except Default.
*   **The Color Dictionary:** A global `ColorDictionary.cs` singleton exists in `Assets/Scripts/Managers/`. It maps the `GameColor` enum to specific Unity `Materials` (for the oil-painting shaders) and `Color` tints.
*   **The Pickers:** `BaseColorPicker.cs` and `DoorColorPicker.cs` automatically assign materials and physics layers to objects based on their enum.
*   **Object-Oriented Cannons:** We deleted the legacy cannons. We built `BaseCannon.cs` (which handles Unity Object Pooling, GameColor cycling, and PositionOffset cycling). It is inherited by `ProjectileCannon`, `CloudCannonDerived`, `WaddlerCannonDerived`, and `ItemCannon`.
*   **The Modular Player:** We upgraded `PlayerBehaviour.cs`. It now uses Coyote Time, Jump Buffers, and kinematic state for climbing Ladders. The child `GroundCollisionCheck` script was **deleted**; ground checking is now handled dynamically inside the Player script using `Physics2D.OverlapBox` and `Physics2D.GetLayerCollisionMask`.

---

## 2. High-Concept Puzzles & Tools (Already Built)
We built several new tools and mechanics. They are located in `Assets/Scripts/Mechanics/` and `Assets/Scripts/Tools/`.
*   **Anti-Observation:** Quantum objects that fade and lose colliders when looked at.
*   **Pigment Conservation:** Objects that can absorb color from one source and inject it into another.
*   **Color Conducting:** Platforms that mimic the Player's inputs/velocity when they share the same color.
*   **The Tools:** We built a Boomerang, a Grapple Hook, a Mirror Clone, and a Palette Knife (Scraper).

---

## 3. The Objective for Your Session
The user wants to finish this project by the end of the month. Your job is to focus heavily on **Implementation, Art, and Polish**.

Your immediate next steps are:
1.  **Read the Specs:** Look at `PROJECT_ROADMAP.md`, `LEVEL_MIGRATION_GUIDE.md`, and `GAME_MECHANICS_SPEC.md` in the workspace to understand the user's vision.
2.  **Migrate the Levels:** Follow the `LEVEL_MIGRATION_GUIDE`. Do not delete the user's 15 existing scenes. Open `Scene 1`, strip off the old legacy scripts, and attach our new modular scripts (`BaseColorPicker`, `PlatformEffector2D`, etc.).
3.  **Build the Shaders:** Use the `ShaderGraph_Distortion_Guide.txt` to build the Translucent Magnifying Glass warp shader and the TV Static shader. Apply them to the materials in the Color Dictionary.
4.  **Save System:** Replace the legacy `ESave.cs` with a clean JSON/PlayerPrefs save system.

---

**CRITICAL RULE:** Do NOT use `GameObject.Find` or obsolete `FindObjectOfType` calls. Rely on the Singletons we built or `Object.FindAnyObjectByType`. Ensure Unity's URP `Opaque Texture` setting is enabled for the glass shaders to work!
