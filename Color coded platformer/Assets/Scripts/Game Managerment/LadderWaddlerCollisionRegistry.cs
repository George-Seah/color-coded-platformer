using UnityEngine;
using System.Collections.Generic;

public static class LadderWaddlerCollisionRegistry
{
    public static readonly List<Collider2D> WaddlerColliders = new List<Collider2D>();
    public static readonly List<Collider2D> LadderColliders = new List<Collider2D>();

    public static void RegisterWaddler(Collider2D col)
    {
        WaddlerColliders.Add(col);
        foreach (var ladderCol in LadderColliders)
        {
            Physics2D.IgnoreCollision(col, ladderCol, true);
        }
    }

    public static void UnregisterWaddler(Collider2D col)
    {
        WaddlerColliders.Remove(col);
    }

    public static void RegisterLadder(Collider2D col)
    {
        LadderColliders.Add(col);
        foreach (var waddlerCol in WaddlerColliders)
        {
            Physics2D.IgnoreCollision(waddlerCol, col, true);
        }
    }

    public static void UnregisterLadder(Collider2D col)
    {
        LadderColliders.Remove(col);
    }
}
