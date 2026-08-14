using UnityEngine;

public class LockedDoor : ColorPicker
{
    protected override void Awake()
    {
        changeLayerMask = false;
        gameObject.layer = LayerMask.NameToLayer("Default"); 

        ChangeObjectColor(this.SelectedColor);
    }

    public override void ChangeObjectColor(GameColor color)
    {
        base.ChangeObjectColor(color);

        // For the door, we MUST keep the root GameObject layer as 'Default' so no one can phase through it.
        // We only change the layer of the child Trigger colliders.
        gameObject.layer = LayerMask.NameToLayer("Default");

        if (color != GameColor.None && color != GameColor.ColorPositionChange)
        {
            string layerName = color == GameColor.Default ? "Default" : color.ToString();
            int layerID = LayerMask.NameToLayer(layerName);
            
            if (layerID != -1) 
            {
                // Find all colliders in children
                BoxCollider2D[] childColliders = GetComponentsInChildren<BoxCollider2D>();
                foreach (var col in childColliders) 
                {
                    // If this is the trigger, assign the specific color layer to it
                    if (col.isTrigger && col.gameObject != this.gameObject) 
                    {
                        col.gameObject.layer = layerID;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string targetLayerName = SelectedColor == GameColor.Default ? "Default" : SelectedColor.ToString();
        int expectedLayer = LayerMask.NameToLayer(targetLayerName);

        if (collision.gameObject.CompareTag("Key") && (collision.gameObject.layer == expectedLayer || collision.gameObject.layer == LayerMask.NameToLayer("Default")))
        {
            if (collision.transform.parent != null && collision.transform.parent.CompareTag("Player"))
            {
                collision.transform.parent.GetComponent<PlayerBehaviour>().SetKey(null);
            }
            else if (collision.transform.parent != null && collision.transform.parent.gameObject.CompareTag("Waddler"))
            {
                collision.transform.parent.gameObject.GetComponent<Waddler>().SetKey(null);
            }
            
            Destroy(collision.gameObject); 
            Destroy(gameObject);           
        }
    }
}
