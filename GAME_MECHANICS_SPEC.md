# 🎮 Color-Coded Platformer: Mechanics & Tech Spec

**To the Next Arena Agent:**
You are building this Unity project entirely from scratch. This document outlines the intended **functionality, rules, and technical design** of every mechanic in the game so you know exactly what to build.

---

## 1. The Core Color Mechanic
* **Color Enum System:** All colors must be managed by a centralized script containing a global `ColorState` enum (Red, Blue, Yellow, Orange, Green, Purple, White, Black, Default, ColorPositionChange, None). 
* **Materials & Visuals:** 
  * **Default:** Uses the sprite's regular, un-tinted material.
  * **White:** Uses a specific white-tinted material.
  * **Other Colors:** You must swap an object's actual Unity `Material`, NOT just tint the `SpriteRenderer` vertex color.
* **The Layer Matrix Rule:** The Player and objects change their `gameObject.layer` to match their color. The Unity Physics2D Collision Matrix dictates the interactions exactly like this:
  * **Default (Baseline):** Interacts with EVERYTHING.
  * **White:** Interacts with ALL colors, EXCEPT Black.
  * **Black:** Interacts with NO colors, EXCEPT Default.
  * **Primary/Secondary:** Interacts with itself and its derived colors (Red hits Red/Orange/Purple), plus Default and White. It ignores other primaries/secondaries and Black.
* **Ground Collision:** The player's `GroundCollisionCheck` should ONLY kick in when the player is falling (`linearVelocityY <= 0`). Rising through semi-solid platforms should not trigger ground logic.

## 2. Entities & Tools
### 🧑‍🚀 The Player
* **Flipping:** Use `transform.rotation = Quaternion.Euler(0, 180, 0)` for Left, and `Euler(0, 0, 0)` for Right.
* **Ladders & Fences (Braid-Style):** Generate a ladder/fence Sprite stacked on top of itself based on a float/int value. When grabbing a ladder/fence, the Player switches to a **Kinematic** physics state. This allows vertical climbing while ignoring gravity. While climbing, the player completely ignores collisions with Semi-Solid platforms and Clouds, but can still be killed by Cannonballs.
* **Locked Doors & Keys:** 
  * Locked doors must use exactly TWO `BoxCollider2D`s: one solid, one trigger to detect the key. 
  * Keys flip automatically by being parented to the holder's `HoldPoint` Transform.

### ⚔️ Weapons & Tools
* **The Mirror:** Creates a clone of Auburn that shares his inputs.
* **The Ring:** Creates an aura. Objects inside the aura change color temporarily.
* **Boomerang:** A thrown weapon whose color can be changed mid-flight by color switchers.
* **Grapple Hook:** Can only grapple onto objects of the exact same color.
* **Palette Knife (Scraper):** Strips the top layer of paint off objects/enemies to reveal their "underpainting" color.

### 💣 Cannons (Object-Oriented Architecture)
* **Aiming:** Cannons aim purely based on their `transform.rotation`.
* **Inheritance:** Create an abstract parent `BaseCannon.cs`, and derive specific children: `WaddlerCannon`, `ItemCannon`, `ProjectileCannon`, `CloudCannon`.

---

## 3. UI & Visual Telegraphing (Crucial for Puzzles)
To ensure the mechanics are clear to the player, implement these visual cues:
* **Position-Based Colors:** Use a UI Bar showing the horizontal/vertical color strips with an arrow tracking Auburn's coordinates. Objects near a border should have a subtle gradient/glow indicating the next color. The world background should have subtle "canvas grid lines" for the zones.
* **The Scraper:** Scrapeable objects have "chipped paint" sprite corners revealing the color underneath. They occasionally drop tiny particle flecks of the hidden color.
* **Color Conducting:** When Auburn changes color, all synced objects of that color must pulse with a glow/outline to show they are "listening" to his inputs.
* **Anti-Observation:** Quantum objects don't instantly vanish; they fade into a translucent "pencil sketch" over 0.5s when looked at, accompanied by a glassy audio shimmer. They pop back to solid paint when Auburn turns away.

---

## 4. World Progression & Story
* **Story Delivery:** Text fades in the environment dynamically (like *Ibb and Obb*). Auburn's ultimate goal is a mystery.
* **Progression:** Worlds 1 to 7 introduce new equipment and puzzle types sequentially.
* **Level Transitions:** Doors must transition using Unity `Scene` files directly.