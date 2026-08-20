# 🗺️ Color-Coded Platformer: End-of-Month Roadmap & Checklist

Today is **August 15**. You have roughly **16 days** until the end of the month. Because all of the heavy, complex backend coding (Cannons, Physics Matrix, Player Controller, Enemy AI) is completely finished, your remaining workload is almost entirely focused on **Implementation, Art, and Polish**.

Here is the exact master checklist of everything left to do, categorized by what is realistic to finish by August 31st.

---

## 🟩 Phase 1: Level Migration & Save System (Days 1 - 5)
*This is the most critical phase. Getting the game fully playable from start to finish.*

- [ ] **The Save System:** Write a simple JSON or PlayerPrefs script to replace `ESave.cs`. It needs to track unlocked levels and collected Stars.
- [ ] **Migrate Levels 1 - 5:** Open the first 5 scenes. Strip the old scripts and attach the new `BaseColorPicker`, `DoorColorPicker`, and `ColorChanger` scripts.
- [ ] **Migrate Levels 6 - 10:** Upgrade the middle levels. Implement the new `PlatformEffector2D` for one-way platforms and parent clouds.
- [ ] **Migrate Levels 11 - 15:** Upgrade the final levels. Hook up the new Object-Oriented Cannons and test the Waddler key-stealing mechanic.
- [ ] **Hook up the UI:** Ensure the `CanvasScript` properly loads the Unity Scenes from the Level Select menu.

## 🟦 Phase 2: Shaders & Post-Processing (Days 6 - 10)
*Getting the "Oil Painting / Arthouse" aesthetic fully locked in.*

- [ ] **URP Configuration:** Ensure the Universal Render Pipeline is active and `Opaque Texture` is enabled.
- [ ] **Build the Base Shaders:** Follow the guide to create the `SolidPaintShader`, `TranslucentGlassShader`, and `TVStaticShader`.
- [ ] **Assign Materials:** Map all the new Shader Materials into the `ColorDictionary`.
- [ ] **Post-Processing Volume:** Add a Global Volume to the scenes. Configure **Bloom**, **Color Grading/Tonemapping**, and your custom **Kuwahara / Oil Paint Grain** filters.
- [ ] **Visual Telegraphing:** Add the "chipped paint" sprites for the Scraper, and set up the UI tracking bar for the Position-Based colors.

## 🟪 Phase 3: High-Concept Puzzle Integration (Days 11 - 14)
*Injecting the new mechanics we built into the levels.*

- [ ] **Action-Determined Color:** Setup the obstacle courses that force Auburn to Jump/Fall to change colors.
- [ ] **Pigment Conservation:** Place the `PigmentVessel` objects in a puzzle room to test the Color Economy.
- [ ] **Color Conducting:** Setup a puzzle where Auburn jumping causes Red platforms to move.
- [ ] **Anti-Observation:** Place the quantum bridges and test the camera/facing logic.
- [ ] **The Tools:** Distribute the Mirror, Ring, Boomerang, Grapple Hook, and Scraper across the levels.

## 🟧 Phase 4: Audio, Art & Polish (Days 15 - 16)
*This is the final coat of varnish.*

- [ ] **Audio:** Add ambient soundscapes (wind/birds), UI clicks, and satisfying mechanical sounds (Scraper tearing paint, quantum objects shattering).
- [ ] **Lore Integration:** Place the `TextTriggerZone` scripts in the background to fade the story text in and out as Auburn walks.

---

### ⚠️ What Might NOT Be Realistic By End of Month
You mentioned wanting to **"learn to paint so I can paint backgrounds"** and **"learn to rig a 2D skeleton in Unity."**
*   *Reality Check:* Fully illustrating 15 levels of custom arthouse backgrounds AND learning 2D skeletal animation from scratch is a massive undertaking. Learning 2D rigging alone can take a week to make it look smooth.
*   *Recommendation:* Use placeholder/simple block art for the player and environment for now. Let the **Shader Graphs and Post-Processing** carry the aesthetic weight for the prototype. Focus on making the puzzles *fun*. You can replace the sprites with rigged 2D skeletons in September once the game is fully playable!
