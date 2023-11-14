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
    public int upgradeLife;
    [HideInInspector] public int currentLife;

    public int baseAttack;
    public int upgradeAttack;
    [HideInInspector] public int currentAttack;  

    public int baseSpeed;
    public int upgradeSpeed;
    [HideInInspector] public int currentSpeed;

    public int baseUpgradeCostLevel;
    public int upgradeCostLevel;
    [HideInInspector] public int currentUpgradeCostLevel;

    public int moneyCost;

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
