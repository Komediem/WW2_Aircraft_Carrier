using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UnitDatas : MonoBehaviour
{
    public Unit unit;

    [Header("Text Feedback")]
    [SerializeField] private TextMeshProUGUI unitName;
    [SerializeField] private Image unitImage;

    public void Start()
    {
        ShowUnitData();
    }

    public void ShowUnitData()
    {
        unitName.text = unit.unitName;
        unitImage.sprite = unit.unitIcon;
    }
}
