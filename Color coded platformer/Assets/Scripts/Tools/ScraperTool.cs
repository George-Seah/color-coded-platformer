using UnityEngine;

public class ScraperTool : MonoBehaviour
{
    public Transform hitPoint;
    public float scrapeRadius = 1.0f;
    public LayerMask scrapeableLayers;

    public void UseScraper()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(hitPoint.position, scrapeRadius, scrapeableLayers);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<ScrapeableObject>(out var target))
            {
                target.ScrapePaint();
            }
        }
    }
}

public class ScrapeableObject : MonoBehaviour
{
    public string underpaintingColorLayer;
    
    public void ScrapePaint()
    {
        int layerID = LayerMask.NameToLayer(underpaintingColorLayer);
        if (layerID != -1)
        {
            gameObject.layer = layerID;
        }
    }
}
