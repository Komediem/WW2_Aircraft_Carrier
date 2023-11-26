using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlliedPosition : MonoBehaviour
{
    public bool isOccuped;

    public Bounds bounds;

    public Unit unit;

    public void OnDrawGizmosSelected()
    {
        var r = GetComponent<Renderer>();
        if (r == null)
            return;
        bounds = r.bounds;
        Gizmos.matrix = Matrix4x4.identity;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(bounds.center, bounds.extents * 2);
    }
}
