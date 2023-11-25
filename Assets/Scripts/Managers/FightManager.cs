using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Events;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class FightManager : MonoBehaviour
{
    [SerializeField] private GameObject unitFightCard;
    [SerializeField] private GameObject content;


    [SerializeField] private List<GameObject> alliedPositions;
    [SerializeField] private GameObject alliedPositionsParent;
    [SerializeField] private GameObject selectedPos;

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
                }
            }
        }
    }

    public void SelectPos(GameObject obj)
    {
        GameObject previousPos;

        selectedPos = obj;
        obj.GetComponent<MeshRenderer>().material.color = Color.red;
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
}
