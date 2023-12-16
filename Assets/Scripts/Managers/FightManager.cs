using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FightManager : MonoBehaviour
{
    public MissionCreator mission;

    [Header("Formation")]
    [SerializeField] private Formations currentFormation;
    [SerializeField] private GameObject currentFormationObject;
    [SerializeField] private GameObject formationCard;
    [SerializeField] private Transform formationPosition;
    [SerializeField] private Transform enemyFormationPosition;
    [SerializeField] private bool formationIsChoosed;

    [Header("Cards and Contents")]
    [SerializeField] private GameObject unitFightCard;
    [SerializeField] private GameObject unit3DDatas;
    [SerializeField] private GameObject content;
    [SerializeField] private List<GameObject> desactivateSelection = new();

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI buttonPhase;
    [SerializeField] private TextMeshProUGUI globalAlliedSpeedText;
    [SerializeField] private TextMeshProUGUI globalEnemySpeedText;
    [SerializeField] private string passToUnitText;
    [SerializeField] private string passToFightText;

    [Header("Position")]
    [SerializeField] private List <GameObject> alliedPositions;
    [SerializeField] private GameObject alliedPositionsParent;
    [SerializeField] private GameObject selectedPos;

    [Header("Local Variables")]
    private GameObject planeSpawned;
    private int enemySpawnCurrent;
    [SerializeField] private List<Unit> currentAlliedTeam = new();
    [SerializeField] private List<Unit> currentEnemyTeam = new();
    [SerializeField] private int globalAlliedSpeed;
    [SerializeField] private int globalEnemySpeed;

    private bool posHit;
    private AlliedPosition posHitScript;

    public Vector3 mousePosition;
    Plane plane = new Plane(Vector3.up, 0);

    public FightPhase fightPhase;
    public enum FightPhase
    {
        FormationSelection,
        UnitSelection,
        PlayerTurn,
        EnemyTurn,
    }

    void Start()
    {
        if(mission.missionFormationSelection == MissionCreator.MissionFormation.free)
        {
            fightPhase = FightPhase.FormationSelection;
            EnemyFormation();
        }

        else if (mission.missionFormationSelection == MissionCreator.MissionFormation.imposed)
        {
            currentFormation = mission.imposedFormation;
            fightPhase = FightPhase.UnitSelection;
            EnemyFormation();
        }

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

                /*
                if(!posHitScript.isOccuped)
                posHitScript.gameObject.GetComponent<Renderer>().material.color = Color.green;*/
            }
        }

        else
        {
            /*
            if(posHitScript != null && !posHitScript.isOccuped)
            posHitScript.gameObject.GetComponent<Renderer>().material.color = Color.white;*/

            posHit = false;
            posHitScript = null;
        }
    }


    public void ChooseTarget(Unit unit)
    {
        if(selectedPos != null && fightPhase == FightPhase.PlayerTurn && !selectedPos.GetComponent<AlliedPosition>().unit.havePlayed)
        {
            unit.currentLife -= selectedPos.GetComponent<AlliedPosition>().unit.currentAttack;
            selectedPos.GetComponent<AlliedPosition>().unit.havePlayed = true;
            selectedPos.GetComponent<Renderer>().material.color = Color.red;

            print(unit.currentLife);
            CheckUnitStats(unit);

            selectedPos = null;
        }
    }

    public void EnemyFormation()
    {
        if(!formationIsChoosed)
        {
            GameObject test = Instantiate(mission.enemyFormation.formationPrefab, enemyFormationPosition.position, Quaternion.identity);

            foreach (Transform enemyPosition in test.transform)
            {
                if(enemySpawnCurrent <= mission.enemyMissionPositions.Count - 1)
                {
                    enemyPosition.GetComponent<EnemyPosition>().unit = mission.enemyMissionPositions[enemySpawnCurrent].enemy.enemyUnit;
                    enemyPosition.GetComponent<EnemyPosition>().unit.level = mission.enemyMissionPositions[enemySpawnCurrent].enemy.enemyUnitLevel;

                    SetEnemiesDatas(enemyPosition.GetComponent<EnemyPosition>().unit);
                    GameObject currentEnemyPlane = Instantiate(mission.enemyMissionPositions[enemySpawnCurrent].enemy.enemyUnit.unitModel, enemyPosition.position, Quaternion.Euler(0, 90, 0), test.transform);

                    GameObject currentDatas = Instantiate(unit3DDatas, currentEnemyPlane.transform.position + new Vector3(0, 2, 0), Quaternion.Euler(0, 0, 30), currentEnemyPlane.transform);
                    currentDatas.transform.localScale = Vector3.one;
                    currentDatas.GetComponent<UnitDatas>().unit = enemyPosition.GetComponent<EnemyPosition>().unit;

                    globalEnemySpeed += enemyPosition.GetComponent<EnemyPosition>().unit.currentSpeed;
                    currentEnemyTeam.Add(enemyPosition.GetComponent<EnemyPosition>().unit);
                    enemySpawnCurrent++;

                    EventTrigger trigger = enemyPosition.GetComponent<EventTrigger>();
                    EventTrigger.Entry entry = new EventTrigger.Entry();
                    entry.eventID = EventTriggerType.PointerClick;
                    entry.callback.AddListener((data) => { ChooseTarget(enemyPosition.GetComponent<EnemyPosition>().unit); });
                    trigger.triggers.Add(entry);
                }
            }
        }
    }

    public void SetEnemiesDatas(Unit unit)
    {
        unit.currentSpeed = unit.baseSpeed;
        unit.currentAttack = unit.baseAttack;
        unit.currentLife = unit.baseLife;

        for(int i = 1; i < unit.level; i++)
        {
            unit.currentSpeed += unit.upgradeSpeed;
        }

        for (int i = 1; i < unit.level; i++)
        {
            unit.currentAttack += unit.upgradeAttack;
        }

        for (int i = 1; i < unit.level; i++)
        {
            unit.currentLife += unit.upgradeLife;
        }
    }

    private void CheckUnitStats(Unit unit)
    {
        if(unit.currentLife <= 0)
        {
            print("is Destroyed");
        }
    }

    public void SelectPos(GameObject obj)
    {
        if(fightPhase == FightPhase.PlayerTurn)
        {
            if (selectedPos == null)
            {
                selectedPos = obj;

                if(!selectedPos.GetComponent<AlliedPosition>().unit.havePlayed)
                {
                    obj.GetComponent<MeshRenderer>().material.color = Color.green;
                }

                else
                {
                    selectedPos = null;
                }
            }

            else if (selectedPos != null)
            {
                selectedPos.GetComponent<MeshRenderer>().material.color = Color.white;

                selectedPos = obj;

                if (!selectedPos.GetComponent<AlliedPosition>().unit.havePlayed)
                {
                    obj.GetComponent<MeshRenderer>().material.color = Color.green;
                }

                else
                {
                    selectedPos = null;
                }
            }
        }
    }

    public void FormationSelection()
    {
        GameObject card;

        foreach (Transform child in content.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        if (Reserve.Instance != null)
        {
            if (Reserve.Instance.formations.Count > 0)
            {
                foreach (Formations formation in Reserve.Instance.formations)
                {
                    card = Instantiate(formationCard, content.transform);

                    card.GetComponent<FormationDatas>().formation = formation;

                    //Event Trigger when card is clicked

                    EventTrigger trigger = card.GetComponent<EventTrigger>();
                    EventTrigger.Entry entry = new EventTrigger.Entry();
                    entry.eventID = EventTriggerType.PointerClick;
                    entry.callback.AddListener((data) => { FormationSelectionButton(formation); });
                    trigger.triggers.Add(entry);
                }
            }
        }
    }

    public void FormationSelectionButton(Formations formation)
    {
        if (formationIsChoosed)
        {
            Destroy(currentFormationObject);
            formationIsChoosed = false;
        }

        else if(!formationIsChoosed)
        {
            currentFormation = formation;
            currentFormationObject = Instantiate(currentFormation.formationPrefab, formationPosition.position, Quaternion.identity);
            alliedPositionsParent = currentFormationObject;

            if(alliedPositions.Count != 0)
            {
                alliedPositions.Clear();
            }

            SetupPosition();
            formationIsChoosed = true;
        }
    }

    public void SpawnPlane(Unit unit)
    {
        if(!unit.isInFight)
        {
            planeSpawned = Instantiate(unit.unitModel, mousePosition, Quaternion.identity);
            
            GameObject currentDatas = Instantiate(unit3DDatas, planeSpawned.transform.position + new Vector3(0, 2, 0), Quaternion.Euler(0, 0, 30), planeSpawned.transform);
            currentDatas.GetComponent<UnitDatas>().unit = unit;
        }
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

            globalAlliedSpeed += unit.currentSpeed;
            ResetSpeed();

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
        if (fightPhase == FightPhase.UnitSelection && posHitScript.isOccuped)
        {
            posHitScript.PositionFree();
            posHitScript.DestroyPlane();

            posHitScript.associatedDatas.unit.isInFight = false;
            posHitScript.isOccuped = false;
            posHitScript.associatedDatas.CheckUnitInFight();

            globalAlliedSpeed -= posHitScript.unit.currentSpeed;
            ResetSpeed();

            posHitScript.unit = null;
        }
    }



    #region Setup Utils

    public void ShowUnitChoice()
    {
        GameObject card;

        foreach (Transform child in content.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        if (Reserve.Instance != null)
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

        foreach (GameObject child in alliedPositions)
        {
            //Event Trigger to remove present plane

            EventTrigger trigger = child.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((data) => { RemoveUnit(); });
            trigger.triggers.Add(entry);

            EventTrigger trigger2 = child.GetComponent<EventTrigger>();
            EventTrigger.Entry entry2 = new EventTrigger.Entry();
            entry2.eventID = EventTriggerType.PointerClick;
            entry2.callback.AddListener((data) => { SelectPos(child); });
            trigger2.triggers.Add(entry2);
        }

    }

    #endregion



    #region Turn Utils

    private void CheckPhase()
    {
        switch (fightPhase)
        {
            case FightPhase.FormationSelection:

                enemySpawnCurrent = 0;
                FormationSelection();
                ResetSpeed();

                break;

            case FightPhase.UnitSelection:

                ShowUnitChoice();

                break;

            case FightPhase.PlayerTurn:



                break;

            case FightPhase.EnemyTurn:



                break;
        }
    }

    public void DefineFirstPlayer()
    {
        if (globalAlliedSpeed >= globalEnemySpeed)
        {
            fightPhase = FightPhase.PlayerTurn;
        }
        else
        {
            fightPhase = FightPhase.EnemyTurn;
        }
    }

    public void ResetSpeed()
    {
        globalAlliedSpeedText.text = globalAlliedSpeed.ToString();
        globalEnemySpeedText.text = globalEnemySpeed.ToString();
    }

    private void CheckEndTurn()
    {
        if (fightPhase == FightPhase.PlayerTurn)
        {
            foreach (Unit units in currentAlliedTeam)
            {
                if (units.havePlayed)
                {

                }
            }
        }

        if (fightPhase == FightPhase.EnemyTurn)
        {
            foreach (Unit units in currentEnemyTeam)
            {
                if (units.havePlayed)
                {

                }
            }
        }
    }

    public void PassToNextSelection()
    {
        foreach (Transform child in content.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        if (fightPhase == FightPhase.FormationSelection && formationIsChoosed)
        {
            fightPhase = FightPhase.UnitSelection;
            buttonPhase.text = "FIGHT";
        }

        else if (fightPhase == FightPhase.UnitSelection && formationIsChoosed)
        {
            foreach (GameObject unitPosition in alliedPositions)
            {
                currentAlliedTeam.Add(unitPosition.GetComponent<AlliedPosition>().unit);

                unitPosition.GetComponent<MeshRenderer>().material.color = Color.white;
            }

            foreach (GameObject desactivateElement in desactivateSelection)
            {
                desactivateElement.SetActive(false);
            }

            DefineFirstPlayer();
        }

        CheckPhase();
    }


    #endregion
}
