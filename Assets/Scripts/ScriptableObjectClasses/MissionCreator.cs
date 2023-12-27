using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mission", menuName = "Mission")]
public class MissionCreator : ScriptableObject
{
    public MissionFormation missionFormationSelection;
    public Formations imposedFormation;
    public Formations enemyFormation;

    public enum MissionFormation
    {
        imposed,
        free
    }

    public List<AlliedMissionPositions> alliedMissionPositions = new();
    public List<EnemyMissionPositions> enemyMissionPositions;
    public List<MissionRewards> missionRewards;
}

[Serializable]
public struct AlliedMissionPositions
{
    public AlliedPosition alliedPositionParameters;
    public PositionEffect alliedPositionEffect;
}

[Serializable]
public struct EnemyMissionPositions
{
    public EnemyPosition enemyPositionParameters;
    public PositionEffect enemyPositionEffect;
    public MissionEnemies enemy;
}

[Serializable]
public struct MissionEnemies
{
    public Unit enemyUnit;
    public int enemyUnitLevel;
}

[Serializable]
public struct MissionRewards
{
    public enum RewardType
    {
        money,
        detachedPieces,
        plans
    }

    public RewardType rewardType;

    [Header("Ressources")]
    public int rewardNumber;

    [Header("Plans")]
    public Unit unitPlans;
}
