using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reserve : MonoBehaviour
{
    public static Reserve Instance;

    public List<ScriptableObject> units = new();

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddUnitToReserve(ScriptableObject unit)
    {
        units.Add(unit);
    }

    public void RemoveUnitFromReserve(ScriptableObject unit)
    {
        if(units.Contains(unit))
        units.Remove(unit);
    }
}
