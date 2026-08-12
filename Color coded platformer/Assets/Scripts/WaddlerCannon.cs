using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class WaddlerCannon : MonoBehaviour
{
    [SerializeField] GameObject waddler;
    [Tooltip("How fast each waddler moves")]
    [SerializeField] float waddlerSpeed;
    [Tooltip("Time between each shot")]
    [SerializeField] float shootInterval;
    [Tooltip("Helps keep track of time between shots. Sets back to 0 after each shot")]
    [SerializeField] float shootTime;
    [Tooltip("Changes the force at which waddlers come out of cannon")]
    [SerializeField] float blastForce;
    [Tooltip("Changes the direction waddlers when out of cannon")]
    [SerializeField] bool waddlerFacesRight;
    [Tooltip("Change this to limit the amount of waddlers from this cannon")]
    [SerializeField] int maxWaddlers = 0;
    // List of colored cannonballs to shoot (set this in the Inspector)
    [Tooltip("List of cannonball colors")]
    public List<string> colorsToShoot = new List<string>();
    [SerializeField] int colorIndex = 0;
    [SerializeField] Color waddlerColor;

    void FixedUpdate()
    {
        shootTime += Time.deltaTime;

        // Check current number of Waddlers if maxWaddlers is set
        if (maxWaddlers > 0 && GetChildWaddlersCount() >= maxWaddlers)
            return;

        if (shootTime > shootInterval)
        {
            shootTime = 0;

            // Instantiate a new Waddler
            GameObject newWaddler = Instantiate(waddler, transform.position, Quaternion.identity, gameObject.transform);
            Waddler waddlerScript = newWaddler.GetComponent<Waddler>();

            // Launch based on cannon's rotation
            waddlerScript.Launch(transform, blastForce, waddlerFacesRight);

            // Set the Waddler's speed
            waddlerScript.SetSpeed(waddlerSpeed);
            if(colorsToShoot.Count != 0){
                // Determine the color and layer
                if (colorsToShoot[colorIndex] == "Red" || colorsToShoot[colorIndex] == "red")
                {
                    //newWaddler.GetComponent<ColorPicker>().selectedColor = ColorOptions.Red;
                    waddlerColor = Color.red;
                    newWaddler.gameObject.layer = LayerMask.NameToLayer("Red");
                    newWaddler.GetComponent<SpriteRenderer>().color = Color.red;
                    Debug.Log($"Waddler Cannon waddler color should be red");
                }
                else if (colorsToShoot[colorIndex] == "Green" || colorsToShoot[colorIndex] == "green")
                {
                    //newWaddler.GetComponent<ColorPicker>().selectedColor = ColorOptions.Green;
                    waddlerColor = Color.green;
                    newWaddler.gameObject.layer = LayerMask.NameToLayer("Green");
                    newWaddler.GetComponent<SpriteRenderer>().color = Color.green;
                    Debug.Log($"Waddler Cannon waddler color should be green");
                }
                else if (colorsToShoot[colorIndex] == "Blue" || colorsToShoot[colorIndex] == "blue")
                {
                    //newWaddler.GetComponent<ColorPicker>().selectedColor = ColorOptions.Blue;
                    waddlerColor = Color.blue;
                    newWaddler.gameObject.layer = LayerMask.NameToLayer("Blue");
                    newWaddler.GetComponent<SpriteRenderer>().color = Color.blue;
                    Debug.Log($"Waddler Cannon waddler color should be blue");
                }
                else
                {
                    //newWaddler.GetComponent<ColorPicker>().selectedColor = ColorOptions.White;
                    waddlerColor = Color.white;
                    newWaddler.GetComponent<SpriteRenderer>().color = Color.white;
                    newWaddler.gameObject.layer = LayerMask.NameToLayer("Default");
                    Debug.Log($"Waddler Cannon waddler color should be white");
                }

                Debug.Log($"Waddler Cannon waddler color should be {waddlerColor}");
            }


            // Cycle through the colors
            if(colorsToShoot.Count != 0){
                if(colorIndex < colorsToShoot.Count - 1){
                    colorIndex ++;
                    
                }
                else{
                    colorIndex = 0;
                }
            }

        }
    }

    // Get all child objects with the "Waddler" tag
    int GetChildWaddlersCount()
    {
        int count = 0;
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Waddler"))
            {
                count++;
            }
        }
        return count;
    }
}
