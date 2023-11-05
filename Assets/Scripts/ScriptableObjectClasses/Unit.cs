using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Unit", menuName = "Unit")]
public class Unit : ScriptableObject
{
    public string unitName;

    [Header("Statistics")]
    public int life;
    public int attack;  
    public int speed;
    public int moneyCost;

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
        Legendary
    }
}
