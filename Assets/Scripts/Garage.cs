using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garage : MonoBehaviour
{
    public static Garage Instance;

    [SerializeField] private GameObject unitCard;
    public GameObject content;

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void Start()
    {
        CheckAllUnits();
    }

    public void CheckAllUnits()
    {
        GameObject card;

        foreach (Transform child in content.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (Unit unit in PlayerData.Instance.units)
        {
            card = Instantiate(unitCard, content.transform);

            card.GetComponent<UnitDatas>().unit = unit;
        }
    }

    public void UpgradeUnit(Unit unit)
    {
        if (unit.level == 1 && PlayerData.Instance.money >= unit.upgradeCostLevel)
        {
            RessourceManager.Instance.LoseMoney(unit.currentUpgradeCostLevel);

            unit.level++;

            unit.currentLife = unit.baseLife + unit.upgradeLife;
            unit.currentAttack = unit.baseAttack + unit.upgradeAttack;
            unit.currentSpeed = unit.baseSpeed + unit.upgradeSpeed;
            unit.currentUpgradeCostLevel = unit.baseUpgradeCostLevel + unit.upgradeCostLevel;
        }

        else if (PlayerData.Instance.money >= unit.upgradeCostLevel)
        {
            RessourceManager.Instance.LoseMoney(unit.currentUpgradeCostLevel);

            unit.level++;

            unit.currentLife += unit.upgradeLife;
            unit.currentAttack += unit.upgradeAttack;
            unit.currentSpeed += unit.upgradeSpeed;
            unit.currentUpgradeCostLevel += unit.upgradeCostLevel;
        }
    }
}
