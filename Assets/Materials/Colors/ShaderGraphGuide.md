# 🎨 How to Build the "Solid Paint" Shader Graph

Since you want to use a Shader Graph (which is the best choice for adding your oil painting and Kuwahara effects later!), here is how to build the foundational "Paint Override" shader.

This shader will **keep the shape (Alpha/Silhouette)** of your sprite, but completely **replace the inside pixels** with a solid block of your chosen color. This is exactly how you differentiate a "White Painted Block" from a "Default Textured Block".

---

### Step 1: Create the Shader Graph
1. In the Unity Project window, right-click and select **Create -> Shader Graph -> URP -> Sprite Unlit Shader Graph** (or **Sprite Lit** if you want it to react to 2D lights).
2. Name it `SolidPaintShader`.
3. Double-click it to open the Shader Graph window.

### Step 2: Set up the Properties (Blackboard)
In the top-left "Blackboard" panel, add two properties:
1. Click `+` -> **Texture2D**. Name it `_MainTex` (Reference MUST be exactly `_MainTex` so the SpriteRenderer knows to pass the sprite into it).
2. Click `+` -> **Color**. Name it `PaintColor`. 
   - Click on `PaintColor` and in the Graph Inspector, change its "Mode" to **HDR** (so you can make it glow/bloom later if you want!).

### Step 3: Build the Nodes
Drag both `_MainTex` and `PaintColor` from the Blackboard onto the graph workspace.

1. Create a **Sample Texture 2D** node. 
   - Connect your `_MainTex` property to the `Texture` input of this node.
2. Create a **Multiply** node.
   - Connect the **A (Alpha)** output from the `Sample Texture 2D` node into the `A` input of the Multiply node.
   - Connect the **A (Alpha)** output of your `PaintColor` property into the `B` input of the Multiply node.
3. Connect everything to the Master Node (the big box on the right):
   - Connect the **RGB** output of your `PaintColor` property directly into the **Base Color** of the Master Node. *(This tells the shader: "Ignore the sprite's original colors, just use my paint color!")*
   - Connect the output of your **Multiply** node into the **Alpha** of the Master Node. *(This tells the shader: "Keep the shape and transparency of the original sprite!")*

### Step 4: Apply it to your Materials
1. Click **Save Asset** in the top left of the Shader Graph window.
2. Go to your `RedMaterial`, `WhiteMaterial`, `BlackMaterial`, etc.
3. In the Inspector, change their Shader from `Sprites/Default` to **Shader Graphs/SolidPaintShader**.
4. You will now see a "Paint Color" box on the material. Set it to Red, White, Black, etc.

Now, when a brown crate turns "White", it won't just look like a slightly brighter crate—it will look like a solid silhouette of pure white paint!
