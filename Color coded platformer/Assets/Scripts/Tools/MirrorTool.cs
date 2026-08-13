using UnityEngine;

public class MirrorTool : MonoBehaviour
{
    public GameObject clonePrefab;
    private GameObject activeClone;

    public void ActivateMirror(Transform playerTransform)
    {
        if (activeClone == null)
        {
            // Spawn clone 2 units above the player
            activeClone = Instantiate(clonePrefab, playerTransform.position + Vector3.up * 2f, Quaternion.identity);
            
            // Assign it the same input actions so it mimics the player (Double Cherry logic)
            // Assumes Clone has a script that reads from PlayerInputActions
        }
    }
}
