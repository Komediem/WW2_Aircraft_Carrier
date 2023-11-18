using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Workshop : MonoBehaviour
{
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

            button.GetComponent<Button>().onClick.AddListener(delegate { Reserve.Instance.BuyUnit(unit); });
        }
    }

    public void GetNewPlans(Unit unit)
    {
        unit.plansCurrent ++;

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
