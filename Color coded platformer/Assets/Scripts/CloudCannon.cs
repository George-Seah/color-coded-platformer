using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CloudCannon : MonoBehaviour
{
    [SerializeField] GameObject cloudPrefab;

    [Tooltip("How fast each cloud moves")]
    [SerializeField] float cloudSpeed = 5f;

    [Tooltip("Time between each shot")]
    [SerializeField] float shootInterval = 1f;

    float shootTime;

    [Tooltip("Limit number of clouds (0 = unlimited)")]
    [SerializeField] int maxClouds = 0;

    [Header("Cloud Colors (Order Matters)")]
    public List<GameColor> colorsToShoot = new();

    int colorIndex;

    public List<GameObject> cloudsSpawned = new();

    [Header("Rotation")]
    [SerializeField] bool tiltCloud;
    [SerializeField] float customTilt;

    void FixedUpdate()
    {
        shootTime += Time.deltaTime;
        cloudsSpawned = cloudsSpawned.Where(c => c != null).ToList();

        if (maxClouds > 0 && cloudsSpawned.Count >= maxClouds) return;
        if (shootTime < shootInterval) return;

        ShootCloud();
        shootTime = 0f;
    }

void ShootCloud()
{
    GameObject newCloudObj = Instantiate(cloudPrefab, transform.position, transform.rotation);
    Cloud cloud = newCloudObj.GetComponent<Cloud>();

    Physics2D.IgnoreCollision(newCloudObj.GetComponent<Collider2D>(), GetComponent<Collider2D>());

    if (!cloud)
    {
        Debug.LogError("Cloud prefab is missing Cloud component.");
        Destroy(newCloudObj);
        return;
    }

    cloud.SetDirection(transform, cloudSpeed, tiltCloud, customTilt);

    // === COLOR DECISION LOGIC ===

    // 1. If the cloud has position-based coloring, let it decide.
    if (newCloudObj.GetComponent<ColorPositionChange>() != null)
    {
        // Do nothing.
        // Do NOT advance colorIndex.
    }
    // 2. Otherwise, apply ColorPicker if present.
    else
    {
        ColorPicker picker = newCloudObj.GetComponent<ColorPicker>();

        if (picker != null && colorsToShoot.Count > 0)
        {
            picker.SelectedColor = colorsToShoot[colorIndex];

            colorIndex++;
            if (colorIndex % colorsToShoot.Count == 0)
                colorIndex = 0;
        }
    }

    cloudsSpawned.Add(newCloudObj);
}


}
