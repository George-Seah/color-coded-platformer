using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class Cannon : MonoBehaviour
{
    [SerializeField] GameObject cannonBall;
    [SerializeField] float ballSpeed;
    [SerializeField] float shootInterval;
    // List of colored cannonballs to shoot (set this in the Inspector)
    [Tooltip("List of cannonball colors")]
    public List<string> colorsToShoot = new List<string>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        
    }
}
