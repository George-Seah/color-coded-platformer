using UnityEngine;
using UnityEngine.Pool;

public abstract class BaseCannon<T> : MonoBehaviour where T : MonoBehaviour
{
    public T prefabToShoot;
    public float fireInterval = 2f;
    public float shootSpeed = 10f;
    
    [Header("Color Cycling System")]
    [Tooltip("If populated, the cannon will cycle through these colors for each shot.")]
    public GameColor[] colorCycle;
    private int currentColorIndex = 0;

    [Header("Position Offset Cycling")]
    [Tooltip("If populated, the cannon will cycle through these positional offsets for ColorPositionChange for each shot.")]
    public int[] positionOffsetCycle;
    private int currentOffsetIndex = 0;

    private float timer;
    protected ObjectPool<T> pool;

    protected virtual void Awake()
    {
        pool = new ObjectPool<T>(
            createFunc: () => Instantiate(prefabToShoot),
            actionOnGet: (obj) => { 
                obj.gameObject.SetActive(true); 
                obj.transform.position = transform.position; 
                obj.transform.rotation = transform.rotation; 
            },
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj.gameObject),
            defaultCapacity: 10,
            maxSize: 30
        );
    }

    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer >= fireInterval)
        {
            Fire();
            timer = 0;
        }
    }

    protected virtual void Fire()
    {
        T spawned = pool.Get();
        
        // 1. Handle Color Cycling natively with GameColor enum
        if (colorCycle != null && colorCycle.Length > 0)
        {
            if (spawned.TryGetComponent<ColorPicker>(out var picker))
            {
                picker.SelectedColor = colorCycle[currentColorIndex];
            }
            else if (spawned.TryGetComponent<SpriteRenderer>(out var sr))
            {
                // Fallback if there is no ColorPicker attached directly to prefab
                GameColor targetColor = colorCycle[currentColorIndex];
                string layerName = targetColor == GameColor.Default ? "Default" : targetColor.ToString();
                int layerId = LayerMask.NameToLayer(layerName);
                if(layerId != -1) spawned.gameObject.layer = layerId;

                sr.color = GetUnityColor(targetColor);
            }
            currentColorIndex = (currentColorIndex + 1) % colorCycle.Length;
        }

        // 2. Handle Position Offset Cycling
        if (positionOffsetCycle != null && positionOffsetCycle.Length > 0)
        {
            if (spawned.TryGetComponent<ColorPositionChange>(out var posChanger))
            {
                posChanger.SetOffset(positionOffsetCycle[currentOffsetIndex]);
            }
            currentOffsetIndex = (currentOffsetIndex + 1) % positionOffsetCycle.Length;
        }

        InitializeProjectile(spawned);
    }

    Color GetUnityColor(GameColor color)
    {
        switch(color)
        {
            case GameColor.Red: return Color.red;
            case GameColor.Blue: return Color.blue;
            case GameColor.Yellow: return Color.yellow;
            case GameColor.Green: return Color.green;
            case GameColor.Orange: return new Color(1f, 0.5f, 0f);
            case GameColor.Purple: return new Color(0.5f, 0f, 0.5f);
            case GameColor.Black: return Color.black;
            case GameColor.White: return Color.white;
            default: return Color.white;
        }
    }

    // Children override this to define how the specific object moves
    protected abstract void InitializeProjectile(T spawnedObj);
}
