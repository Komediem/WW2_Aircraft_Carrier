using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    [SerializeField] private GameObject unitFightCard;
    [SerializeField] private GameObject content;

    public FightPhase fightPhase;
    public enum FightPhase
    {
        UnitSelection,
        FirstPlayer,
        SecondPlayer,
    }

    void Start()
    {
        fightPhase = FightPhase.UnitSelection;

        CheckPhase();
    }

    public void CheckPhase()
    {
        switch(fightPhase)
        {
            case FightPhase.UnitSelection:

                SelectionPhase();

                break;

            case FightPhase.FirstPlayer:



                break;

            case FightPhase.SecondPlayer:



                break;
        }
    }

    public void SelectionPhase()
    {
        GameObject card;

        foreach (Transform child in content.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (Unit unit in Reserve.Instance.units)
        {
            card = Instantiate(unitFightCard, content.transform);

            card.GetComponent<UnitDatas>().unit = unit;
        }
    }
}
