using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Reserve : MonoBehaviour
{
    public static Reserve Instance;

    public List<Unit> units = new();

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddUnitToReserve(Unit unit)
    {
        units.Add(unit);
    }

    public void RemoveUnitFromReserve(Unit unit)
    {
        if(units.Contains(unit))
        units.Remove(unit);
    }
}
