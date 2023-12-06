using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPosition : MonoBehaviour
{
    public bool isOccuped;
    public Unit unit;
    public Vector3 position;

    private void Start()
    {
        position = this.transform.position;
    }
}