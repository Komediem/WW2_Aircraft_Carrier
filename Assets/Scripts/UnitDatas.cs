using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private TextMeshProUGUI unitLevel;

    [Header("Unit Type Image")]
    [SerializeField] private Sprite fighterIcon;
    [SerializeField] private Sprite assaultPlaneIcon;
    [SerializeField] private Sprite bomberIcon;
    [SerializeField] private Sprite antiAerialIcon;

    [SerializeField] private Button upgrade;

    [Header("Image Feedback")]
    [SerializeField] private Image unitImage;
    [SerializeField] private Image unitType;
    [SerializeField] private Image cardBackground;

    public void Start()
    {
        ShowUnitData();
        DisplayUnitType();

        if(upgrade != null)
        {
            upgrade.onClick.AddListener(delegate { Reserve.Instance.UpgradeUnit(unit); });
            upgrade.onClick.AddListener(delegate { ShowUnitData(); });
        }
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

        if(unitLevel != null)
        unitLevel.text = unit.level.ToString();
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
        if(cardBackground != null)
        {
            if (unit.Rarity == Unit.rarity.Common)
            {
                //Color if common
            }

            else if (unit.Rarity == Unit.rarity.Rare)
            {
                //Color if rare
            }

            else if (unit.Rarity == Unit.rarity.Legendary)
            {
                //Color if legendary
            }
        }
    }
}
