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
    [SerializeField] private int colorIndex = 0;
    private Color waddlerColor;

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

            // Determine the color and layer
            if (colorsToShoot[colorIndex] == "Red")
            {
                waddlerColor = Color.red;
                newWaddler.layer = LayerMask.NameToLayer("Red");
            }
            else if (colorsToShoot[colorIndex] == "Green")
            {
                waddlerColor = Color.green;
                newWaddler.layer = LayerMask.NameToLayer("Green");
            }
            else if (colorsToShoot[colorIndex] == "Blue")
            {
                waddlerColor = Color.blue;
                newWaddler.layer = LayerMask.NameToLayer("Blue");
            }
            else
            {
                waddlerColor = Color.white;
                newWaddler.layer = LayerMask.NameToLayer("Default");
            }

            newWaddler.GetComponent<SpriteRenderer>().color = waddlerColor;

            // Cycle through the colors
            colorIndex = (colorIndex + 1) % colorsToShoot.Count;
        }
    }

    // Get all child objects with the "Waddler" tag
    private int GetChildWaddlersCount()
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
