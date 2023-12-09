using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Workshop : MonoBehaviour
{
    public Reserve reserve;

    public List<Unit> units = new();

    private UnitDatas unitDatas;

    [SerializeField] private GameObject craftUnitButton;
    [SerializeField] private GameObject content;

    public void Start()
    {
        foreach (Unit unit in units)
        {
            GameObject button = Instantiate(craftUnitButton, content.transform);

            unitDatas = button.GetComponent<UnitDatas>();
            unitDatas.unit = unit;

            SetStandardDatas(unit);

            button.GetComponent<Button>().onClick.AddListener(delegate { BuyUnit(unit); });
        }
    }

    public void BuyUnit(Unit unit)
    {
        if (RessourceManager.Instance != null && RessourceManager.Instance.money >= unit.moneyCost && unit.isUnlocked)
        {
            Unit uniqueUnit = Instantiate(unit);
            reserve.units.Add(uniqueUnit);

            RessourceManager.Instance.LoseMoney(unit.moneyCost);
        }
    }

    public void GetNewPlans(Unit unit)
    {
        unit.plansCurrent ++;

        if(unit.unitFeedbacks != null)
        unit.unitFeedbacks.CheckLocking();
        unit.unitFeedbacks.ShowUnitData();
    }

    public void SetStandardDatas(Unit unit)
    {
        unit.currentLife = unit.baseLife;
        unit.currentAttack = unit.baseAttack;
        unit.currentSpeed = unit.baseSpeed;
        unit.currentUpgradeCostLevel = unit.baseUpgradeCostLevel;
        unit.plansCurrent = 0;
        unit.isUnlocked = false;
        unit.level = 1;
    }
}
