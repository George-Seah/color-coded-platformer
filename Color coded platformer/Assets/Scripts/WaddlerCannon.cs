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
    [Tooltip("Changes the angle at which waddlers come out of cannon")]
    [SerializeField] float launchAngle = 45f;
    // List of colored cannonballs to shoot (set this in the Inspector)
    [Tooltip("List of cannonball colors")]
    public List<string> colorsToShoot = new List<string>();
    [SerializeField] private int colorIndex = 0;
    private Color waddlerColor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        shootTime += Time.deltaTime;
        if (shootTime > shootInterval)
        {
            shootTime = 0;
            GameObject newWaddler = Instantiate(waddler.gameObject, transform.position, Quaternion.identity);
            Waddler waddlerScript = newWaddler.GetComponent<Waddler>();
            waddlerScript.Launch(launchAngle, blastForce);



            // Set the Waddler properties
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

            // Launch the Waddler at a specific angle
            // Set your desired launch angle here


            // Cycle through the colors
            colorIndex = (colorIndex + 1) % colorsToShoot.Count;

        }
    }

}