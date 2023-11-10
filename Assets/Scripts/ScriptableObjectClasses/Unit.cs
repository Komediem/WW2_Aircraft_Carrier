using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Unit", menuName = "Unit")]
public class Unit : ScriptableObject
{
    public string unitName;

    [Header("Statistics")]
    public int baseLife;
    [HideInInspector] public float life;

    public int baseAttack;
    [HideInInspector] public float attack;  

    public int baseSpeed;
    [HideInInspector] public float speed;

    public int baseUpgradeCostLevel1;
    [HideInInspector] public float upgradeCostLevel;

    public int moneyCost;

    public float upgradeRatio;

    public int levelMax;
    public int level;

    public Sprite unitIcon;


    [Header("Enumerations")]
    public unitType UnitType;
    public enum unitType
    {
        Fighter,
        AssaultPlane,
        Bomber,
        AntiAerial
    }
    public rarity Rarity;
    public enum rarity
    {
        Common,
        Rare,
        Epique,
        Legendary
    }
}
