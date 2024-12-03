using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class Cannon : MonoBehaviour
{
    [SerializeField] GameObject cannonBall;
    [SerializeField] float ballSpeed;
    [SerializeField] float shootInterval;
    [SerializeField] float shootTime;
    // List of colored cannonballs to shoot (set this in the Inspector)
    [Tooltip("List of cannonball colors")]
    public List<string> colorsToShoot = new List<string>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        shootTime += Time.deltaTime;
        if(shootTime > shootInterval){
            Instantiate(cannonBall, transform.position, transform.rotation);
            cannonBall.GetComponent<CannonBall>().speed = ballSpeed;
            shootTime = 0;
        }
        
    }
}
