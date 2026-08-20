using UnityEngine;

// 1. CannonBall Cannon
public class ProjectileCannon : BaseCannon<CannonBall>
{
    protected override void InitializeProjectile(CannonBall spawnedObj)
    {
        Vector2 forwardDir = transform.right; 
        spawnedObj.Launch(forwardDir, shootSpeed, pool);
    }
}

// 2. Waddler Cannon
public class WaddlerCannonDerived : BaseCannon<Waddler>
{
    protected override void InitializeProjectile(Waddler spawnedObj)
    {
        Rigidbody2D rb = spawnedObj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = transform.right * shootSpeed;
        }
    }
}

// 3. Item/Key Cannon
public class ItemCannon : BaseCannon<Key>
{
    protected override void InitializeProjectile(Key spawnedObj)
    {
        Rigidbody2D rb = spawnedObj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.linearVelocity = transform.right * shootSpeed;
        }
    }
}

// 4. Cloud Cannon
public class CloudCannonDerived : BaseCannon<Cloud>
{
    protected override void InitializeProjectile(Cloud spawnedObj)
    {
        spawnedObj.speed = shootSpeed;
        Rigidbody2D rb = spawnedObj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = transform.right * shootSpeed;
        }
    }
}
