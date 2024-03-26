using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
[CreateAssetMenu(fileName = "New Unit", menuName = "Unit")]
public class Unit : ScriptableObject
{
    [HideInInspector] public UnitDatas unitFeedbacks;
    public string unitName;

    public GameObject unitModel;
    public int moneyCost;

    public Sprite unitIcon;

    #region Life
    [Header("Life")]
    public int baseLife;
    public int upgradeLife;
    [HideInInspector] public int currentLife;
    #endregion

    #region Attack
    [Header("Attack")]
    public int baseAttack;
    public int upgradeAttack;
    [HideInInspector] public int currentAttack;
    #endregion

    #region Speed
    [Header("Speed")]
    public int baseSpeed;
    public int upgradeSpeed;
    [HideInInspector] public int currentSpeed;
    #endregion

    #region Upgrade Cost
    [Header("Upgrade Cost")]
    public int baseUpgradeCostLevel;
    public int upgradeCostLevel;
    [HideInInspector] public int currentUpgradeCostLevel;
    #endregion

    #region RestingTime
    [Header("Resting Time")]
    public int baseRestingTime;
    public int upgradeRestingTime;
    [HideInInspector] public float currentRestingTime;
    #endregion

    #region Timers

    public float baseRestTime;
    public float upgradeRestTime;

    #endregion

    #region Plans

    [Header("Plans")]
    public int plansMax;
    public int plansCurrent;

    #endregion

    #region Level

    [Header("Level")]
    public int levelMax;
    public int level;

    #endregion

    #region Booleans

    public bool isUnlocked;
    public bool isInFight;
    public bool isDestroyed;
    public bool havePlayed;

    #endregion

    #region Enumerations

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

    #endregion
}
