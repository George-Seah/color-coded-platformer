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
        if(shootTime > shootInterval){
            waddler.gameObject.GetComponent<Waddler>().SetSpeed(waddlerSpeed);
            if(colorsToShoot[colorIndex] == "Red"){
                waddlerColor = Color.red;
                waddler.gameObject.layer = LayerMask.NameToLayer("Red");
            }
            else if(colorsToShoot[colorIndex] == "Green"){
                waddlerColor = Color.green;
                waddler.gameObject.layer = LayerMask.NameToLayer("Green");
            }
            else if(colorsToShoot[colorIndex] == "Blue"){
                waddlerColor = Color.blue;
                waddler.gameObject.layer = LayerMask.NameToLayer("Blue");
            }
            else{
                //If the index is anything not red, green or blue, it will be the default settings
                waddlerColor = Color.white;
                waddler.gameObject.layer = LayerMask.NameToLayer("Default");
            }
            waddler.gameObject.GetComponent<SpriteRenderer>().color = waddlerColor;
            //waddler.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(waddler.GetComponent<Rigidbody2D>().linearVelocity.x, blastForce);
            Instantiate(waddler.gameObject, transform.position, transform.rotation);
            //waddler.gameObject.GetComponent<Waddler>().Launch(transform.rotation, blastForce);
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
