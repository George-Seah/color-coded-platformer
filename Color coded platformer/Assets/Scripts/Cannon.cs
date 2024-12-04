using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class Cannon : MonoBehaviour
{
    [SerializeField] GameObject cannonBall;
    [Tooltip("How fast each cannonball moves")]
    [SerializeField] float ballSpeed;
    [Tooltip("Time between each shot")]
    [SerializeField] float shootInterval;
    [Tooltip("Helps keep track of time between shots. Sets back to 0 after each shot")]
    [SerializeField] float shootTime;
    // List of colored cannonballs to shoot (set this in the Inspector)
    [Tooltip("List of cannonball colors")]
    public List<string> colorsToShoot = new List<string>();
    [SerializeField] private int colorIndex = 0;
    private Color ballColor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        shootTime += Time.deltaTime;
        if(shootTime > shootInterval){
            cannonBall.gameObject.GetComponent<CannonBall>().speed = ballSpeed;
            if(colorsToShoot[colorIndex] == "Red"){
                ballColor = Color.red;
                cannonBall.gameObject.layer = LayerMask.NameToLayer("Red");
            }
            else if(colorsToShoot[colorIndex] == "Green"){
                ballColor = Color.green;
                cannonBall.gameObject.layer = LayerMask.NameToLayer("Green");
            }
            else if(colorsToShoot[colorIndex] == "Blue"){
                ballColor = Color.blue;
                cannonBall.gameObject.layer = LayerMask.NameToLayer("Blue");
            }
            else{
                //If the index is anything not red, green or blue, it will be the default settings
                ballColor = Color.white;
                cannonBall.gameObject.layer = LayerMask.NameToLayer("Default");
            }
            cannonBall.gameObject.GetComponent<SpriteRenderer>().color = ballColor;
            Instantiate(cannonBall.gameObject, transform.position, transform.rotation);
            
            if(colorIndex < colorsToShoot.Count - 1){
                colorIndex ++;
                
            }
            else{
                colorIndex = 0;
            }
            /*
            else if(colorIndex > colorsToShoot.Count){
                colorIndex = 0;
            }
            */
            shootTime = 0;
        }
        
    }
}
