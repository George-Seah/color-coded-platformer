using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class PhysicsMatrixSetup
{
    static PhysicsMatrixSetup()
    {
        EditorApplication.delayCall += SetupCollisionMatrix;
    }

    static void SetupCollisionMatrix()
    {
        string[] allLayers = { "Default", "White", "Black", "Red", "Blue", "Yellow", "Orange", "Green", "Purple", "SemiSolid" };
        
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty layers = tagManager.FindProperty("layers");
        
        foreach (string layerName in allLayers)
        {
            bool found = false;
            for (int i = 8; i < layers.arraySize; i++)
            {
                if (layers.GetArrayElementAtIndex(i).stringValue == layerName) { found = true; break; }
            }
            if (!found)
            {
                for (int i = 8; i < layers.arraySize; i++)
                {
                    if (string.IsNullOrEmpty(layers.GetArrayElementAtIndex(i).stringValue))
                    {
                        layers.GetArrayElementAtIndex(i).stringValue = layerName;
                        tagManager.ApplyModifiedProperties();
                        break;
                    }
                }
            }
        }

        // Apply Collision Rules
        int defaultL = LayerMask.NameToLayer("Default");
        int whiteL = LayerMask.NameToLayer("White");
        int blackL = LayerMask.NameToLayer("Black");
        int semiSolidL = LayerMask.NameToLayer("SemiSolid");

        if (defaultL == -1 || whiteL == -1 || blackL == -1) return;

        Physics2D.IgnoreLayerCollision(blackL, whiteL, true);
        string[] primarySecondary = { "Red", "Blue", "Yellow", "Orange", "Green", "Purple" };

        foreach (string col in primarySecondary)
        {
            int colL = LayerMask.NameToLayer(col);
            if (colL != -1)
            {
                Physics2D.IgnoreLayerCollision(blackL, colL, true);
                Physics2D.IgnoreLayerCollision(whiteL, colL, false);

                foreach (string otherCol in primarySecondary)
                {
                    int otherColL = LayerMask.NameToLayer(otherCol);
                    if (otherColL != -1 && col != otherCol)
                    {
                        bool related = false;
                        if (col == "Red" && (otherCol == "Orange" || otherCol == "Purple")) related = true;
                        else if (col == "Blue" && (otherCol == "Purple" || otherCol == "Green")) related = true;
                        else if (col == "Yellow" && (otherCol == "Orange" || otherCol == "Green")) related = true;
                        else if (col == "Orange" && (otherCol == "Red" || otherCol == "Yellow")) related = true;
                        else if (col == "Purple" && (otherCol == "Red" || otherCol == "Blue")) related = true;
                        else if (col == "Green" && (otherCol == "Blue" || otherCol == "Yellow")) related = true;
                        
                        Physics2D.IgnoreLayerCollision(colL, otherColL, !related);
                    }
                }
            }
        }
    }
}
