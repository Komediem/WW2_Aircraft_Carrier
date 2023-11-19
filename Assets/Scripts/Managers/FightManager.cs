using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour
{

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

    }
}
