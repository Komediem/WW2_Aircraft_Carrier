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

    [Header("Unit Type Image")]
    [SerializeField] private Sprite fighterIcon;
    [SerializeField] private Sprite assaultPlaneIcon;
    [SerializeField] private Sprite bomberIcon;
    [SerializeField] private Sprite antiAerialIcon;

    [Header("Image Feedback")]
    [SerializeField] private Image unitImage;
    [SerializeField] private Image unitType;

    public void Start()
    {
        ShowUnitData();
        DisplayUnitType();
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

    public void DisplayUnitType()
    {
        if (unitType != null)
        {
            if(unit.UnitType == Unit.unitType.Fighter)
            {
                unitType.sprite = fighterIcon;
            }

            else if(unit.UnitType == Unit.unitType.AssaultPlane)
            {
                unitType.sprite = assaultPlaneIcon;
            }

            else if(unit.UnitType == Unit.unitType.Bomber)
            {
                unitType.sprite = bomberIcon;
            }

            else if(unit.UnitType == Unit.unitType.AntiAerial)
            {
                unitType.sprite = antiAerialIcon;
            }
        }
    }

    public void DisplayBackgroundColor()
    {

    }
}
