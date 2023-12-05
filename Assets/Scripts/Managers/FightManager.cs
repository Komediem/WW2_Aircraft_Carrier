using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class FightManager : MonoBehaviour
{
    [SerializeField] private GameObject unitFightCard;
    [SerializeField] private GameObject content;


    [SerializeField] private List <GameObject> alliedPositions;
    [SerializeField] private GameObject alliedPositionsParent;
    [SerializeField] private GameObject selectedPos;

    private bool posHit;
    private AlliedPosition posHitScript;

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
        RaycastHit hit;


        //Raycast to follow and get the mousePosition
        if (plane.Raycast(ray, out distance))
        {
            mousePosition = ray.GetPoint(distance);
        }

        //Check if raycast hit a position
        if (Physics.Raycast(ray, out hit))
        {
            if(hit.collider.GetComponent<AlliedPosition>() != null)
            {
                posHit = true;
                posHitScript = hit.collider.GetComponent<AlliedPosition>();

                if(!posHitScript.isOccuped)
                posHitScript.gameObject.GetComponent<Renderer>().material.color = Color.green;
            }
        }


        else
        {
            if(posHitScript != null && !posHitScript.isOccuped)
            posHitScript.gameObject.GetComponent<Renderer>().material.color = Color.white;

            posHit = false;
            posHitScript = null;
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

    public void SetupPosition()
    {
        for (int i = 0; i < alliedPositionsParent.transform.childCount; i++)
        {
            alliedPositions.Add(alliedPositionsParent.transform.GetChild(i).gameObject);
        }

        foreach(GameObject child in alliedPositions)
        {
            //Event Trigger to remove present plane

            EventTrigger trigger = child.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((data) => { RemoveUnit(); });
            trigger.triggers.Add(entry);
        }

    }

    public void SpawnPlane(Unit unit)
    {
        if(!unit.isInFight)
        planeSpawned = Instantiate(unit.unitModel, mousePosition, Quaternion.identity);
    }

    public void DragPlane()
    {
        if(planeSpawned != null)
        planeSpawned.transform.position = mousePosition;
    }

    public void DragFinished(Unit unit)
    {
        if (posHit)
        {
            planeSpawned.transform.position = posHitScript.position;
            planeSpawned.transform.parent = posHitScript.transform;

            posHitScript.unit = unit;
            posHitScript.isOccuped = true;

            unit.isInFight = true;
            posHitScript.PositionBlocked();
            unit.unitFeedbacks.CheckUnitInFight();

            posHitScript.associatedDatas = unit.unitFeedbacks;

            planeSpawned = null;
        }

        else
        {
            Destroy(planeSpawned);
        }
    }

    public void RemoveUnit()
    {
        if (posHitScript.isOccuped)
        {
            posHitScript.PositionFree();
            posHitScript.DestroyPlane();

            posHitScript.associatedDatas.unit.isInFight = false;
            posHitScript.isOccuped = false;
            posHitScript.associatedDatas.CheckUnitInFight();

            posHitScript.unit = null;
        }
    }
}
