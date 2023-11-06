using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

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
}
