using UnityEngine;

public class MirrorTool : MonoBehaviour
{
    public GameObject clonePrefab;
    private GameObject activeClone;

    public void ActivateMirror(Transform playerTransform)
    {
        if (activeClone == null)
        {
            activeClone = Instantiate(clonePrefab, playerTransform.position + Vector3.up * 2f, Quaternion.identity);
        }
    }
}
