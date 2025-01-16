using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class CloudCannon : MonoBehaviour
{
    [SerializeField] GameObject cloud;
    [Tooltip("How fast each cloud moves")]
    [SerializeField] float cloudSpeed;
    [Tooltip("Time between each shot")]
    [SerializeField] float shootInterval;
    [Tooltip("Helps keep track of time between shots. Sets back to 0 after each shot")]
    [SerializeField] float shootTime;
    [Tooltip("Change this to limit the amount of clouds from this cannon")]
    [SerializeField] int maxClouds = 0;
    // List of colored clouds to shoot (set this in the Inspector)
    [Tooltip("List of cloud colors")]
    public List<string> colorsToShoot = new List<string>();
    public List<GameObject> cloudsSpawned = new List<GameObject>();
    [SerializeField] private int colorIndex = 0;
    private Color cloudColor;
    [Tooltip("If turned on, clouds can have a custom rotation set")]
    [SerializeField] bool tiltCloud;
    [Tooltip("If tiltCloud is true, you can change this to customize the rotations of clouds shot out of the cannon")]
    [SerializeField] float customTilt;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        shootTime += Time.deltaTime;
        cloudsSpawned = cloudsSpawned.Where(x => x != null).ToList();

        // Check current number of Waddlers if maxWaddlers is set
        if (maxClouds > 0 && cloudsSpawned.Count >= maxClouds)
            return;
        if(shootTime > shootInterval){
            //GameObject newWaddler = Instantiate(waddler, transform.position, Quaternion.identity, gameObject.transform);
            //Waddler waddlerScript = newWaddler.GetComponent<Waddler>();
            if(colorsToShoot.Count != 0){
                cloud.gameObject.GetComponent<Cloud>().speed = cloudSpeed;
                if(colorsToShoot[colorIndex] == "Red"){
                    cloudColor = Color.red;
                    cloud.gameObject.layer = LayerMask.NameToLayer("Red");
                }
                else if(colorsToShoot[colorIndex] == "Green"){
                    cloudColor = Color.green;
                    cloud.gameObject.layer = LayerMask.NameToLayer("Green");
                }
                else if(colorsToShoot[colorIndex] == "Blue"){
                    cloudColor = Color.blue;
                    cloud.gameObject.layer = LayerMask.NameToLayer("Blue");
                }
                else{
                    //If the index is anything not red, green or blue, it will be the default settings
                    cloudColor = Color.white;
                    cloud.gameObject.layer = LayerMask.NameToLayer("Default");
                }
                cloud.gameObject.GetComponent<SpriteRenderer>().color = cloudColor;
            }
            GameObject newCloud = Instantiate(cloud.gameObject, transform.position, transform.rotation);
            cloudsSpawned.Add(newCloud);
            //transform.eulerAngles.z
            newCloud.GetComponent<Cloud>().SetDirection(transform, cloudSpeed, tiltCloud, customTilt);
            if(colorsToShoot.Count != 0){
                if(colorIndex < colorsToShoot.Count - 1){
                    colorIndex ++;
                    
                }
                else{
                    colorIndex = 0;
                }
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
