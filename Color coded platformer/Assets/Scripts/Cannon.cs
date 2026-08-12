using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class Cannon : MonoBehaviour
{
    [SerializeField] private GameObject cannonBall;
    [Tooltip("How fast each cannonball moves")]
    [SerializeField] private float ballSpeed;
    [Tooltip("Time between each shot")]
    [SerializeField] private float shootInterval;
    [Tooltip("Helps keep track of time between shots. Sets back to 0 after each shot")]
    [SerializeField] private float shootTime;
    [Tooltip("List of cannonball colors")]
    public List<string> colorsToShoot = new List<string>();
    [SerializeField] private int colorIndex = 0;
    public List<int> colorPositionOffset = new List<int>();
    [SerializeField] private int colorPositionOffsetIndex = 0;
    private Color ballColor;

    [Tooltip("List of scripts to add to cannonballs")]

    void FixedUpdate()
    {
        shootTime += Time.deltaTime;
        if (shootTime > shootInterval)
        {
            Fire(ballSpeed);
            shootTime = 0;
        }
    }

    // Fire function (retained as per your request)
    public void Fire(float ballSpeed)
    {
        // Instantiate the cannonball
        GameObject instantiatedBall = Instantiate(cannonBall, transform.position, transform.rotation);

        // Set ball speed
        instantiatedBall.GetComponent<CannonBall>().speed = ballSpeed;

        // Set cannonball color and layer
        if(colorsToShoot.Count != 0){
            if (colorsToShoot[colorIndex] == "Red")
            {
                ballColor = Color.red;
                instantiatedBall.layer = LayerMask.NameToLayer("Red");
                Debug.Log($"instantiatedBall should be red.");
            }
            else if (colorsToShoot[colorIndex] == "Green")
            {
                ballColor = Color.green;
                instantiatedBall.layer = LayerMask.NameToLayer("Green");
            }
            else if (colorsToShoot[colorIndex] == "Blue")
            {
                ballColor = Color.blue;
                instantiatedBall.layer = LayerMask.NameToLayer("Blue");
            }
            else
            {
                ballColor = Color.white;
                instantiatedBall.layer = LayerMask.NameToLayer("Default");
            }
            // Apply color to the SpriteRenderer
            instantiatedBall.GetComponent<SpriteRenderer>().color = ballColor;
            // Update the color index
            colorIndex = (colorIndex + 1) % colorsToShoot.Count;
        }
        //Set cannonball color position change offset
        if(colorPositionOffset.Count != 0){
            instantiatedBall.GetComponent<ColorPositionChange>().SetOffset(colorPositionOffset[colorPositionOffsetIndex]);
            colorPositionOffsetIndex = (colorPositionOffsetIndex + 1) % colorPositionOffset.Count;
        }
    }
}
