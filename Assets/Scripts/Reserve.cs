using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reserve : MonoBehaviour
{
    public static Reserve Instance;

    public List<Unit> units = new();

    [SerializeField] private GameObject unitCard;
    [SerializeField] private GameObject content;

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
        if(RessourceManager.Instance != null && RessourceManager.Instance.money >= unit.moneyCost)
        {
            Unit uniqueUnit = Instantiate(unit);
            units.Add(uniqueUnit);

            RessourceManager.Instance.money -= unit.moneyCost;
        }
    }

    /*public void RemoveUnitFromReserve(Unit unit)
    {
        if(units.Contains(unit))
        units.Remove(unit);
    }*/

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
        unit.upgradeCostLevel += (unit.level * unit.upgradeRatio) * unit.baseUpgradeCostLevel1;

        if(RessourceManager.Instance.money >= unit.upgradeCostLevel)
        {
            unit.level++;

            unit.life += unit.level * unit.baseLife * 0.2f;
            unit.attack += unit.level * unit.baseAttack * 0.2f;
            unit.speed += unit.level * unit.baseSpeed * 0.2f;

            RessourceManager.Instance.money -= Mathf.RoundToInt(unit.upgradeCostLevel);
        }
    }
}
