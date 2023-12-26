using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPosition : MonoBehaviour
{
    public bool isOccuped;
    public Unit unit;
    public Vector3 position;

    [Header("Unit Datas")]
    public int baseLife;
    public int currentLife;
    public int baseAttack;
    public int currentAttack;
    public int baseSpeed;
    public int currentSpeed;

    public GameObject unitModel;

    private void Start()
    {
        position = this.transform.position;
    }
}
