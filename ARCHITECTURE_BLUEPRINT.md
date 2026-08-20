# 🚀 Color-Coded Platformer: Architectural Blueprint

**To the Next Arena Agent:** 
You are building this Unity codebase from scratch. Follow these exact programming instructions to establish a highly performant, modular, production-ready architecture.

---

## 🏗️ Phase 1: Core Architecture & Data
1. **Central Color Dictionary:** Create a centralized Singleton script housing the Color enums mapped to specific Unity `Materials`, `ParticleSystems`, and `Colors`. 
2. **BaseColorPicker Component:** Create a `BaseColorPicker.cs` class. It exposes a `ColorState` enum in the Inspector. On `Awake()`, it fetches the mapped Material, applies it, and updates the `gameObject.layer`. 
   * Create `DoorColorPicker.cs` that inherits from `BaseColorPicker`, overriding the layer assignment so it only modifies the Trigger Collider's layer, keeping the Solid Collider on `Default`.
3. **Event-Driven Color Changers & Particle Sprites (CRUCIAL):** 
   * Color changers must maintain a `List<GameObject>` of the objects they affect.
   * **Dynamic Texture Sheet:** On start, the script accesses its `ParticleSystem.textureSheetAnimation` module. It loops through the `List<GameObject>`, grabs their `SpriteRenderer.sprite`, and uses `textureSheetAnimation.AddSprite(sprite)`. This forces the Color Changer to emit particles that physically look like the objects it affects!
4. **Object Pooling:** Cannons must use `UnityEngine.Pool`.
5. **Save System:** Write a clean JSON serializer or PlayerPrefs manager. DITCH `ESave`.

## 📐 Phase 2: Modular Player Controller & Physics Rules
1. **Separation of Concerns:** Build the player using separate MonoBehaviours: `PlayerInput.cs`, `PlayerMovement.cs`, `PlayerColorState.cs`, and `PlayerInteraction.cs`.
2. **Dynamic Ground Checking:**
   * Do NOT use standard collision events for grounding. Use `Physics2D.OverlapCircle`.
   * **Color Masking:** The `LayerMask` for the ground check must be dynamically generated based on the Player's current color.
   * **Semi-Solid Snapping Fix:** The ground check MUST only be evaluated if `rb.linearVelocityY <= 0.01f`. 
3. **Ladders & Kinematic State:**
   * When climbing, set `rb.bodyType = RigidbodyType2D.Kinematic` and `rb.linearVelocity = Vector2.zero`.
   * Temporarily ignore the `SemiSolid` layer so they can pass through platforms and clouds. Revert to `Dynamic` when leaving the ladder.
4. **Clouds & Platform Riding:**
   * Clouds are moved via `rb.linearVelocity` (Kinematic). 
   * When landing on a Cloud, use `player.transform.SetParent(cloud.transform)`.
5. **Directionally Solid Platforms:** Add a `PlatformEffector2D` component, check `Use One Way`, and ensure the platform's `BoxCollider2D` has `Used By Effector` checked.

## 🎮 Phase 3: Gameplay Feel
1. **Jump Physics:** Ditch the decaying jump force loop. Use an initial `rb.AddForce`. Implement a higher gravity multiplier when falling, and cut the Y-velocity by half if the jump button is released early. Implement Coyote Time and Jump Buffers.
2. **Waddler AI:** Use `Physics2D.Raycast` for ledge and wall detection to drive turnaround logic.