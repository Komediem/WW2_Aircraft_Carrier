using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlliedPosition : MonoBehaviour
{
    public bool isOccuped;
    public UnitDatas associatedDatas;

    public GameObject unitModel;

    public Bounds bounds;
    public Unit unit;

    public Vector3 position;

    private void Start()
    {
        position = this.transform.position;
    }

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

    public void PositionBlocked()
    {
        GetComponent<Renderer>().material.color = Color.red;
    }

    public void PositionFree()
    {
        GetComponent<Renderer>().material.color = Color.white;
    }

    public void DestroyPlane()
    {
        foreach(Transform children in transform)
        {
            Destroy(children.gameObject);
        }
    }
}
