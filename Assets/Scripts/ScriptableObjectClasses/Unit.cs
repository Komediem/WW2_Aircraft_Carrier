using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Unit", menuName = "Unit")]
public class Unit : ScriptableObject
{
    public string unitName;

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

    public int moneyCost;

    [Header("Plans")]
    public int plansMax;
    public int plansCurrent;

    [Header("Level")]
    public int levelMax;
    public int level;

    public Sprite unitIcon;

    public bool isUnlocked;

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
