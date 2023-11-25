using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FightManager : MonoBehaviour
{
    [SerializeField] private GameObject unitFightCard;
    [SerializeField] private GameObject content;


    [SerializeField] private List<GameObject> alliedPositions;
    [SerializeField] private GameObject alliedPositionsParent;
    [SerializeField] private GameObject selectedPos;

    public Vector3 mousePosition;
    Plane plane = new Plane(Vector3.up, 0);

    private GameObject planeSpawned;

    public FightPhase fightPhase;
    public enum FightPhase
    {
        UnitSelection,
        FirstPlayer,
        SecondPlayer,
    }

    void Start()
    {
        SetupPosition();

        fightPhase = FightPhase.UnitSelection;

        CheckPhase();
    }
    void Update()
    {
        float distance;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out distance))
        {
            mousePosition = ray.GetPoint(distance);
        }
    }

    public void CheckPhase()
    {
        switch(fightPhase)
        {
            case FightPhase.UnitSelection:

                SelectionPhase();
                ShowUnitChoice();

                break;

            case FightPhase.FirstPlayer:



                break;

            case FightPhase.SecondPlayer:



                break;
        }
    }

    public void SelectionPhase()
    {
        print("test");
    }

    public void ShowUnitChoice()
    {
        GameObject card;

        foreach (Transform child in content.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        if(Reserve.Instance != null)
        {
            if (Reserve.Instance.units.Count > 0)
            {
                foreach (Unit unit in Reserve.Instance.units)
                {
                    card = Instantiate(unitFightCard, content.transform);

                    card.GetComponent<UnitDatas>().unit = unit;

                    EventTrigger trigger = card.GetComponent<EventTrigger>();
                    EventTrigger.Entry entry = new EventTrigger.Entry();
                    entry.eventID = EventTriggerType.Drag;
                    entry.callback.AddListener((data) => { DragPlane(unit); });
                    trigger.triggers.Add(entry);
                }
            }
        }
    }

    public void SelectPos(GameObject obj)
    {
        if(selectedPos == null)
        {
            selectedPos = obj;
            obj.GetComponent<MeshRenderer>().material.color = Color.red;
        }

        else if(selectedPos != null)
        {
            selectedPos.GetComponent<MeshRenderer>().material.color = Color.white;

            selectedPos = obj;
            obj.GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }

    public void SetupPosition()
    {
        for (int i = 0; i < alliedPositionsParent.transform.childCount; i++)
        {
            alliedPositions.Add(alliedPositionsParent.transform.GetChild(i).gameObject);
        }

        foreach(GameObject child in alliedPositions)
        {
            EventTrigger trigger = child.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((data) => { SelectPos(child); });
            trigger.triggers.Add(entry);
        }
    }

    public void DragPlane(Unit unit)
    {
        Instantiate(unit.unitModel, mousePosition, Quaternion.identity);
    }
}
