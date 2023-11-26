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

                    //Event Trigger when card is clicked

                    EventTrigger trigger = card.GetComponent<EventTrigger>();
                    EventTrigger.Entry entry = new EventTrigger.Entry();
                    entry.eventID = EventTriggerType.BeginDrag;
                    entry.callback.AddListener((data) => { SpawnPlane(unit); });
                    trigger.triggers.Add(entry);

                    //Event Trigger during the drag

                    EventTrigger trigger2 = card.GetComponent<EventTrigger>();
                    EventTrigger.Entry entry2 = new EventTrigger.Entry();
                    entry2.eventID = EventTriggerType.Drag;
                    entry2.callback.AddListener((data) => { DragPlane(); });
                    trigger2.triggers.Add(entry2);

                    //Event Trigger after releasing the button of drag

                    EventTrigger trigger3 = card.GetComponent<EventTrigger>();
                    EventTrigger.Entry entry3 = new EventTrigger.Entry();
                    entry3.eventID = EventTriggerType.EndDrag;
                    entry3.callback.AddListener((data) => { DragFinished(unit); });
                    trigger3.triggers.Add(entry3);
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

    public void SpawnPlane(Unit unit)
    {
        planeSpawned = Instantiate(unit.unitModel, mousePosition, Quaternion.identity);
    }

    public void DragPlane()
    {
        if(planeSpawned != null)
        planeSpawned.transform.position = mousePosition;
    }

    public void DragFinished(Unit unit)
    {
        foreach(GameObject pos in alliedPositions)
        {
            Vector3 vectorPosition = pos.transform.position;

            if(mousePosition == vectorPosition)
            {
                print("Set on position");

                planeSpawned.transform.position = vectorPosition;
                planeSpawned.transform.parent = pos.transform;

                pos.GetComponent<AlliedPosition>().unit = unit;
            }

            else
            {
                print("No position");

                planeSpawned.SetActive(false);
            }
        }
    }
}
