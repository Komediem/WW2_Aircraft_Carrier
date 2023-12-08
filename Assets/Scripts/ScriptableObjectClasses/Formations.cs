using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Formation", menuName = "Formations")]
public class Formations : ScriptableObject
{
    public GameObject formationPrefab;
    public string formationName;
    public Sprite formationImage;
    public string formationDescription;
}
