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
    [SerializeField] private GameObject enemyFormation;
    [SerializeField] private Transform formationPosition;
    [SerializeField] private Transform enemyFormationPosition;
    [SerializeField] private bool formationIsChoosed;

    [Header("Cards and Contents")]
    [SerializeField] private GameObject unitFightCard;
    [SerializeField] private GameObject unit3DDatas;
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject blackBackground;
    [SerializeField] private GameObject missionRewardParent;
    [SerializeField] private GameObject missionRewardCardPrefab;
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
    [SerializeField] private int alliedUnitDestroyed;
    [SerializeField] private int globalEnemySpeed;
    [SerializeField] private int enemyUnitDestroyed;

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
        EndGame
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


    public void ChooseTarget(EnemyPosition enemyPos)
    {
        if(selectedPos != null && fightPhase == FightPhase.PlayerTurn && !selectedPos.GetComponent<AlliedPosition>().unit.havePlayed)
        {
            enemyPos.unit.currentLife -= selectedPos.GetComponent<AlliedPosition>().unit.currentAttack;
            selectedPos.GetComponent<AlliedPosition>().unit.havePlayed = true;
            selectedPos.GetComponent<Renderer>().material.color = Color.red;

            CheckUnitStats(enemyPos);
            CheckPlayerEndTurn();

            enemyPos.unitDatas.UpdateDatasInFight();
            selectedPos = null;
        }
    }

    private void CheckUnitStats(EnemyPosition enemyPos)
    {
        if(enemyPos.unit.currentLife <= 0)
        {
            enemyPos.unitModel.SetActive(false);
            enemyPos.unit.isDestroyed = true;
            enemyPos.GetComponent<Renderer>().material.color = Color.yellow;

            CheckVictory();
        }
    }

    private void CheckUnitStats(AlliedPosition alliedPos)
    {
        if(alliedPos.unit.currentLife <= 0)
        {
            alliedPos.unitModel.SetActive(false);
            alliedPos.unit.isDestroyed = true;
            alliedPos.GetComponent<Renderer>().material.color = Color.yellow;

            CheckVictory();
        }
    }

    public void SelectPos(GameObject obj)
    {
        if(fightPhase == FightPhase.PlayerTurn)
        {
            if (selectedPos == null)
            {
                selectedPos = obj;

                if(!selectedPos.GetComponent<AlliedPosition>().unit.havePlayed && !selectedPos.GetComponent<AlliedPosition>().unit.isDestroyed)
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

                if (!selectedPos.GetComponent<AlliedPosition>().unit.havePlayed && !selectedPos.GetComponent<AlliedPosition>().unit.isDestroyed)
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

            posHitScript.unitModel = planeSpawned;

            posHitScript.unit = unit;
            posHitScript.isOccuped = true;

            posHitScript.unit.havePlayed = false;
            posHitScript.unit.isDestroyed = false;

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



    #region Enemy Utils

    public void SetEnemiesDatas(EnemyPosition enemyPos)
    {
        enemyPos.unit.currentSpeed = enemyPos.unit.baseSpeed;
        enemyPos.unit.currentAttack = enemyPos.unit.baseAttack;
        enemyPos.unit.currentLife = enemyPos.unit.baseLife;

        for (int i = 1; i < enemyPos.unit.level; i++)
        {
            enemyPos.unit.currentSpeed += enemyPos.unit.upgradeSpeed;
        }

        for (int i = 1; i < enemyPos.unit.level; i++)
        {
            enemyPos.unit.currentAttack += enemyPos.unit.upgradeAttack;
        }

        for (int i = 1; i < enemyPos.unit.level; i++)
        {
            enemyPos.unit.currentLife += enemyPos.unit.upgradeLife;
        }
    }

    public void EnemyFormation()
    {
        if (!formationIsChoosed)
        {
            enemyFormation = Instantiate(mission.enemyFormation.formationPrefab, enemyFormationPosition.position, Quaternion.identity);

            foreach (Transform enemyPosition in enemyFormation.transform)
            {
                if (enemySpawnCurrent < mission.enemyMissionPositions.Count)
                {
                    Unit currentUnit = Instantiate(mission.enemyMissionPositions[enemySpawnCurrent].enemy.enemyUnit);
                    EnemyPosition enemyPosDatas = enemyPosition.GetComponent<EnemyPosition>();

                    currentUnit.level = mission.enemyMissionPositions[enemySpawnCurrent].enemy.enemyUnitLevel;

                    enemyPosDatas.unit = currentUnit;
                    enemyPosDatas.unit.level = currentUnit.level;

                    enemyPosDatas.unit.isDestroyed = false;
                    enemyPosDatas.unit.havePlayed = false;

                    SetEnemiesDatas(enemyPosDatas);
                    enemyPosDatas.unitModel = Instantiate(currentUnit.unitModel, enemyPosition.position, Quaternion.Euler(0, 90, 0), enemyFormation.transform);

                    GameObject currentDatas = Instantiate(unit3DDatas, enemyPosDatas.unitModel.transform.position + new Vector3(0, 2, 0), Quaternion.Euler(0, 0, 30), enemyPosDatas.unitModel.transform);
                    currentDatas.transform.localScale = Vector3.one;
                    currentDatas.GetComponent<UnitDatas>().unit = enemyPosDatas.unit;
                    enemyPosDatas.unitDatas = currentDatas.GetComponent<UnitDatas>();

                    globalEnemySpeed += enemyPosDatas.unit.currentSpeed;
                    currentEnemyTeam.Add(enemyPosDatas.unit);
                    enemySpawnCurrent++;

                    EventTrigger trigger = enemyPosition.GetComponent<EventTrigger>();
                    EventTrigger.Entry entry = new EventTrigger.Entry();
                    entry.eventID = EventTriggerType.PointerClick;
                    entry.callback.AddListener((data) => { ChooseTarget(enemyPosDatas); });
                    trigger.triggers.Add(entry);
                }
            }
        }
    }

    public void EnemyChooseTarget()
    {
        foreach(Transform enemyPosition in enemyFormation.transform)
        {
            EnemyPosition enemyPosDatas = enemyPosition.GetComponent<EnemyPosition>();

            if (enemyPosDatas != null && !enemyPosDatas.unit.isDestroyed)
            {
                int randomAlliedPos = Random.Range(0, alliedPositions.Count);

                EnemyTarget(enemyPosDatas, alliedPositions[randomAlliedPos]);
            }
        }

        fightPhase = FightPhase.PlayerTurn;
        CheckPhase();

        foreach (GameObject unitPos in alliedPositions)
        {
            unitPos.GetComponent<AlliedPosition>().unit.havePlayed = false;

            if(!unitPos.GetComponent<AlliedPosition>().unit.isDestroyed)
            {
                unitPos.GetComponent<Renderer>().material.color = Color.white;
            }
        }
    }

    public void EnemyTarget(EnemyPosition shooter, GameObject target)
    {
        AlliedPosition targetDatas = target.GetComponent<AlliedPosition>();

        targetDatas.unit.currentLife -= shooter.unit.currentAttack;
        targetDatas.associatedDatas.UpdateDatasInFight();

        CheckUnitStats(targetDatas);
    }

    #endregion



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

                EnemyChooseTarget();

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

    private void CheckPlayerEndTurn()
    {
        if (fightPhase == FightPhase.PlayerTurn)
        {
            int unitPlayedNumber = 0;

            foreach (GameObject unitPosition in alliedPositions)
            {
                AlliedPosition alliedPosition = unitPosition.GetComponent<AlliedPosition>();

                if (alliedPosition.unit.havePlayed || alliedPosition.unit.isDestroyed)
                {
                    unitPlayedNumber++;

                    if(unitPlayedNumber >= alliedPositions.Count())
                    {
                        fightPhase = FightPhase.EnemyTurn;
                        CheckPhase();

                        /*foreach (GameObject unitPos in alliedPositions)
                        unitPos.GetComponent<Renderer>().material.color = Color.gray;*/
                    }
                }
            }
        }
    }

    private void CheckVictory()
    {
        alliedUnitDestroyed = 0;
        enemyUnitDestroyed = 0;

        foreach (GameObject alliedPos in alliedPositions)
        {
            if(alliedPos.GetComponent<AlliedPosition>().unit.isDestroyed)
            {
                alliedUnitDestroyed++;

                if(alliedUnitDestroyed >= alliedPositions.Count())
                {
                    fightPhase = FightPhase.EndGame;

                    EnemyWin();
                }
            }
        }

        foreach(Transform enemyPosition in enemyFormation.transform)
        {
            EnemyPosition enemyPosDatas = enemyPosition.GetComponent<EnemyPosition>();

            if(enemyPosDatas != null && enemyPosDatas.unit.isDestroyed)
            {
                enemyUnitDestroyed++;

                if (enemyUnitDestroyed >= alliedPositions.Count())
                {
                    fightPhase = FightPhase.EndGame;

                    PlayerWin();
                }
            }
        }
    }

    private void PlayerWin()
    {
        string winText = "VICTORY";

        blackBackground.SetActive(true);
        blackBackground.GetComponentInChildren<TextMeshProUGUI>().text = winText;

        foreach(MissionRewards missionReward in mission.missionRewards)
        {
            GameObject missionRewardCard = Instantiate(missionRewardCardPrefab, missionRewardParent.transform);
            RewardCard rewardCard = missionRewardCard.GetComponent<RewardCard>();

            rewardCard.rewardName = missionReward.rewardType.ToString();
            rewardCard.rewardNumber = missionReward.rewardNumber;

            if(missionReward.rewardType == MissionRewards.RewardType.money)
            {
                RessourceManager.Instance.EarnMoney(missionReward.rewardNumber);
                print("Money Obtained");
            }

            if (missionReward.rewardType == MissionRewards.RewardType.detachedPieces)
            {
                RessourceManager.Instance.EarnDetachedPieces(missionReward.rewardNumber);
                print("Detached pieces obtained");
            }

            if (missionReward.rewardType == MissionRewards.RewardType.plans)
            {
                RessourceManager.Instance.GetNewPlans(missionReward.unitPlans, missionReward.rewardNumber);
                print("Plans Obtained");
            }
        }
    }

    private void EnemyWin()
    {
        string winText = "DEFEAT";

        blackBackground.SetActive(true);
        blackBackground.GetComponentInChildren<TextMeshProUGUI>().text = winText;
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
