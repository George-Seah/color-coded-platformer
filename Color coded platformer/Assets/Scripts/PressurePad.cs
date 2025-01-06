using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class PressurePad : MonoBehaviour
{
    // Event to be triggered when the pressure pad is activated (stepped on)
    [System.Serializable]
    public class PressurePadActivatedEvent : UnityEvent { }
    public PressurePadActivatedEvent onActivated;

    // Event to be triggered when the pressure pad is released (steps off)
    [System.Serializable]
    public class PressurePadReleasedEvent : UnityEvent { }
    public PressurePadReleasedEvent onReleased;

    // List of tags to detect (set this in the Inspector)
    [Tooltip("List of tags to detect")]
    public List<string> tagsToDetect = new List<string>();

    // Ensure the collider is set to trigger (no physical collisions)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object has one of the specified tags
        if (tagsToDetect.Contains(collision.tag))
        {
            // Trigger the event when an object with a matching tag steps on the pad
            onActivated.Invoke();
        }
    }
    void OnTriggerStay2D(Collider2D collision){
        // Check if the object has one of the specified tags
        if (tagsToDetect.Contains(collision.tag))
        {
            // Trigger the event when an object with a matching tag steps on the pad
            onActivated.Invoke();
        }
    }
    // Called when the object exits the trigger area (releases the pressure pad)
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Check if the object has one of the specified tags
        if (tagsToDetect.Contains(collision.tag))
        {
            // Trigger the event when an object with a matching tag steps off the pad
            onReleased.Invoke();
        }
    }
}
