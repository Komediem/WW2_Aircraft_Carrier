using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class SaveData
{
    [Header("Script Links")]

    [Header("Lists")]
    public List<Unit> unitsPossessed;

    public void Load()
    {
        if (Reserve.Instance != null)
        {
            Reserve.Instance.units = unitsPossessed;
        }
    }

    public void Save()
    {
        if(Reserve.Instance != null)
        {
            unitsPossessed = Reserve.Instance.units;
        }
    }
}

