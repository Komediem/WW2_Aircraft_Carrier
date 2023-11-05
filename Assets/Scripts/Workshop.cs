using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Workshop : MonoBehaviour
{
    public List<Unit> units = new();

    [SerializeField] private GameObject craftUnitButton;
    [SerializeField] private GameObject content;

    public void Start()
    {
        foreach (Unit unit in units)
        {
            GameObject button = Instantiate(craftUnitButton, content.transform);

            button.GetComponent<UnitDatas>().unit = unit;
            button.GetComponent<Button>().onClick.AddListener(delegate { Reserve.Instance.AddUnitToReserve(unit); });
        }
    }
}
