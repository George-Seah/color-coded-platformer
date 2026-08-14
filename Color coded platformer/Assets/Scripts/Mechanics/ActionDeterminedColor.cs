using UnityEngine;

[RequireComponent(typeof(PlayerBehaviour))]
public class ActionDeterminedColor : MonoBehaviour
{
    private PlayerBehaviour player;

    void Awake()
    {
        player = GetComponent<PlayerBehaviour>();
    }

    void Update()
    {
        if (player.jumpPressed)
        {
            player.ColorChanged(GameColor.Yellow); 
        }
        else if (!player.readGrounded() && GetComponent<Rigidbody2D>().linearVelocityY < 0)
        {
            player.ColorChanged(GameColor.Blue); 
        }
    }
}
