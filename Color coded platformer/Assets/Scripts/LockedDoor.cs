using UnityEngine;

public class LockedDoor : ColorPicker
{
    protected override void Awake()
    {
        // Ensure the Door's primary GameObject stays on Default so it's always physically solid
        changeLayerMask = false;
        gameObject.layer = LayerMask.NameToLayer("Default"); 

        ChangeObjectColor(this.SelectedColor);
    }

    public override void ChangeObjectColor(GameColor color)
    {
        if (sr == null)
            sr = GetComponent<SpriteRenderer>();

        if (sr)
        {
            switch (color)
            {
                case GameColor.Red: sr.color = Color.red; break;
                case GameColor.Blue: sr.color = Color.blue; break;
                case GameColor.Yellow: sr.color = Color.yellow; break;
                case GameColor.Green: sr.color = Color.green; break;
                case GameColor.Orange: sr.color = new Color(1f, 0.5f, 0f); break;
                case GameColor.Purple: sr.color = new Color(0.5f, 0f, 0.5f); break;
                case GameColor.Black: sr.color = Color.black; break;
                case GameColor.White: sr.color = Color.white; break;
                case GameColor.Default: sr.color = Color.white; break;
                case GameColor.ColorPositionChange: break;
                case GameColor.None: break;
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
            
            Destroy(collision.gameObject); // Destroy Key
            Destroy(gameObject);           // Destroy Door
        }
    }
}
