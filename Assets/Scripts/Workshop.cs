using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workshop : MonoBehaviour
{
    private Reserve reserve;

    public List<ScriptableObject> units = new();

    private void Awake()
    {
        reserve = Reserve.Instance;
    }

    public void Test()
    {
        reserve.AddUnitToReserve(units[0]);
    }
}
