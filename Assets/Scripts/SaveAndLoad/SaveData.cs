using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class SaveData
{
    [Header("Script Links")]
    public Reserve reserve;

    [Header("Lists")]
    public List<Unit> unitsPossessed;

    public void Load()
    {
        if (reserve != null)
        {
            reserve.units = unitsPossessed;
        }
    }

    public void Save()
    {
        if(reserve != null)
        {
            unitsPossessed = reserve.units;
        }
    }
}

