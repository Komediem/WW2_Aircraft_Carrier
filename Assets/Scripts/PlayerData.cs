using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;

    public float money;
    public float detachedPieces;

    public List<Unit> units;
    public List<Formations> formations;

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        money = PlayerPrefs.GetFloat("Money");
        detachedPieces = PlayerPrefs.GetFloat("DetachedPieces");

        DontDestroyOnLoad(gameObject);
    }

    public void OnDestroy()
    {
        PlayerPrefs.SetFloat("Money", money);
        PlayerPrefs.SetFloat("DetachedPieces", detachedPieces);
        PlayerPrefs.Save();
    }
}
