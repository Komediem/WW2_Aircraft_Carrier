using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class SaveData
{
    [Header("Script Links")]
    public PlayerData playerDatas;

    [Header("Lists")]
    public List<Unit> unitsPossessed;

    public void Load()
    {
            playerDatas.units = unitsPossessed;
    }

    public void Save()
    {
            unitsPossessed = playerDatas.units;
    }
}

