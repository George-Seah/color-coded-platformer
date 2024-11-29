using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] Vector2 direction;
    [SerializeField] bool activated;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(activated){
            transform.position += (new Vector3(direction.x, direction.y, 0) * Time.deltaTime);
        }
    }
    public void activate(){
        if(activated){
            activated = false;
        }
        else{
            activated = true;
        }
    }
}
