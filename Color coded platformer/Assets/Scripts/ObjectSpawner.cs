using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] GameObject objectToSpawn;
    [Tooltip("Time between each shot")]
    [SerializeField] float shootInterval;
    [Tooltip("Helps keep track of time between shots. Sets back to 0 after each shot")]
    [SerializeField] float shootTime;
    [Tooltip("Change this to limit the amount of objects from this cannon")]
    [SerializeField] int maxObjects = 0;
    // List of colored cannonballs to shoot (set this in the Inspector)
    [Tooltip("List of cannonball colors")]
    public List<string> colorsToShoot = new List<string>();
    [SerializeField] private int colorIndex = 0;
    public List<int> colorPositionOffset = new List<int>();
    [SerializeField] private int colorPositionOffsetIndex = 0;
    public List<GameObject> objectsSpawned = new List<GameObject>();
    [SerializeField] private Color objectColor;
    void FixedUpdate()
    {
        shootTime += Time.deltaTime;
        objectsSpawned = objectsSpawned.Where(x => x != null).ToList();
        // Check current number of Waddlers if maxWaddlers is set
        if (maxObjects > 0 && objectsSpawned.Count >= maxObjects)
            return;

        if (shootTime > shootInterval)
        {
            shootTime = 0;

            // Instantiate a new Waddler
            GameObject newObject = Instantiate(objectToSpawn, transform.position + (new Vector3(Mathf.Cos((transform.eulerAngles.z + 90) * Mathf.Deg2Rad), Mathf.Sin((transform.eulerAngles.z + 90) * Mathf.Deg2Rad), 0) * 1.25f), Quaternion.identity, gameObject.transform);
            objectsSpawned.Add(newObject);
            //Waddler waddlerScript = newWaddler.GetComponent<Waddler>();
            
            // Determine the color and layer
            if(colorsToShoot.Count != 0){
                if (colorsToShoot[colorIndex] == "Red" || colorsToShoot[colorIndex] == "red")
                {
                    //newWaddler.GetComponent<ColorPicker>().selectedColor = ColorOptions.Red;
                    objectColor = Color.red;
                    newObject.gameObject.layer = LayerMask.NameToLayer("Red");
                    newObject.GetComponent<SpriteRenderer>().color = Color.red;
                    //Debug.Log($"Waddler Cannon waddler color should be red");
                }
                else if (colorsToShoot[colorIndex] == "Green" || colorsToShoot[colorIndex] == "green")
                {
                    //newWaddler.GetComponent<ColorPicker>().selectedColor = ColorOptions.Green;
                    objectColor = Color.green;
                    newObject.gameObject.layer = LayerMask.NameToLayer("Green");
                    newObject.GetComponent<SpriteRenderer>().color = Color.green;
                    //Debug.Log($"Waddler Cannon waddler color should be green");
                }
                else if (colorsToShoot[colorIndex] == "Blue" || colorsToShoot[colorIndex] == "blue")
                {
                    //newWaddler.GetComponent<ColorPicker>().selectedColor = ColorOptions.Blue;
                    objectColor = Color.blue;
                    newObject.gameObject.layer = LayerMask.NameToLayer("Blue");
                    newObject.GetComponent<SpriteRenderer>().color = Color.blue;
                    //Debug.Log($"Waddler Cannon waddler color should be blue");
                }
                else
                {
                    //newWaddler.GetComponent<ColorPicker>().selectedColor = ColorOptions.White;
                    objectColor = Color.white;
                    newObject.GetComponent<SpriteRenderer>().color = Color.white;
                    newObject.gameObject.layer = LayerMask.NameToLayer("Default");
                    //Debug.Log($"Waddler Cannon waddler color should be white");
                }
                //newWaddler.GetComponent<SpriteRenderer>().color = waddlerColor;
                
                //newWaddler.GetComponent<ColorPicker>().ChangeObjectColor(colorsToShoot[colorIndex]);
                Debug.Log($"Object Spawner Cannon object color should be {objectColor}");
                //newWaddler.GetComponent<ColorPicker>().ChangeObjectColorToRed();
                // Cycle through the colors
                colorIndex = (colorIndex + 1) % colorsToShoot.Count;
            }
            //Set spawned object color position change offset
            if(colorPositionOffset.Count != 0){
                newObject.GetComponent<ColorPositionChange>().SetOffset(colorPositionOffset[colorPositionOffsetIndex]);
                colorPositionOffsetIndex = (colorPositionOffsetIndex + 1) % colorPositionOffset.Count;
            }
        }
    }

    // Get all child objects with the "Waddler" tag
    /*
    private int GetChildObjectsCount()
    {
        int count = 0;
        foreach (Transform child in transform)
        {
            if (child.CompareTag(objectToSpawn.tag))
            {
                count++;
            }
        }
        return count;
    }
    */
}
