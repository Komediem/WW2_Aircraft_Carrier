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
    [SerializeField] private TextMeshProUGUI unitLife;
    [SerializeField] private TextMeshProUGUI unitAttack;
    [SerializeField] private TextMeshProUGUI unitSpeed;
    [SerializeField] private TextMeshProUGUI unitCost;
    [SerializeField] private Image unitImage;

    public void Start()
    {
        ShowUnitData();
    }

    public void ShowUnitData()
    {
        if(unitName != null)
        unitName.text = unit.unitName;

        if(unitLife != null) 
        unitLife.text = unit.life.ToString();

        if(unitAttack != null) 
        unitAttack.text = unit.attack.ToString();

        if(unitSpeed != null)
        unitSpeed.text = unit.speed.ToString();

        if(unitImage != null) 
        unitImage.sprite = unit.unitIcon;

        if(unitCost != null)
        unitCost.text = unit.moneyCost.ToString();
    }
}
