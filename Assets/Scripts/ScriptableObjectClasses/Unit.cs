using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Unit", menuName = "Unit")]
public class Unit : ScriptableObject
{
    public string unitName;

    [Header("Life")]
    public int baseLife;
    public int upgradeLife;
    [HideInInspector] public int currentLife;

    [Header("Attack")]
    public int baseAttack;
    public int upgradeAttack;
    [HideInInspector] public int currentAttack;

    [Header("Speed")]
    public int baseSpeed;
    public int upgradeSpeed;
    [HideInInspector] public int currentSpeed;

    [Header("Upgrade Cost")]
    public int baseUpgradeCostLevel;
    public int upgradeCostLevel;
    [HideInInspector] public int currentUpgradeCostLevel;

    public int moneyCost;

    [Header("Level")]
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
