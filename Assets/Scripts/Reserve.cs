using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reserve : MonoBehaviour
{
    public static Reserve Instance;

    public List<Unit> units;

    [SerializeField] private GameObject unitCard;
    public GameObject content;

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddUnitToReserve(Unit unit)
    {
        Unit uniqueUnit = Instantiate(unit);
        units.Add(uniqueUnit);
    }

    public void BuyUnit(Unit unit)
    {
        if(RessourceManager.Instance != null && RessourceManager.Instance.money >= unit.moneyCost && unit.isUnlocked)
        {
            Unit uniqueUnit = Instantiate(unit);
            units.Add(uniqueUnit);

            RessourceManager.Instance.money -= unit.moneyCost;
        }
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

        foreach (Unit unit in units)
        {
            card = Instantiate(unitCard, content.transform);

            card.GetComponent<UnitDatas>().unit = unit;
        }
    }

    public void UpgradeUnit(Unit unit)
    {
        if(unit.level == 1 && RessourceManager.Instance.money >= unit.upgradeCostLevel)
        {
            RessourceManager.Instance.money -= unit.currentUpgradeCostLevel;

            unit.level++;

            unit.currentLife = unit.baseLife + unit.upgradeLife;
            unit.currentAttack = unit.baseAttack + unit.upgradeAttack;
            unit.currentSpeed = unit.baseSpeed + unit.upgradeSpeed;
            unit.currentUpgradeCostLevel = unit.baseUpgradeCostLevel + unit.upgradeCostLevel;
        }

        else if(RessourceManager.Instance.money >= unit.upgradeCostLevel)
        {
            RessourceManager.Instance.money -= unit.currentUpgradeCostLevel;

            unit.level++;

            unit.currentLife += unit.upgradeLife;
            unit.currentAttack += unit.upgradeAttack;
            unit.currentSpeed += unit.upgradeSpeed;
            unit.currentUpgradeCostLevel += unit.upgradeCostLevel;
        }
    }
}
