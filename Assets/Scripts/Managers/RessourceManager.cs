using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RessourceManager : MonoBehaviour
{
    public static RessourceManager Instance;

    public Ressources ressources;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI detachedPiecesText;

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        if (moneyText != null)
            moneyText.text = ressources.money.ToString();

        if (detachedPiecesText != null)
            detachedPiecesText.text = ressources.detachedPieces.ToString();
    }

    public void EarnMoney(float moneyEarn)
    {
        ressources.money += moneyEarn;
        if (moneyText != null)
            moneyText.text = ressources.money.ToString();
    }

    public void LoseMoney(float moneyLose)
    {
        ressources.money -= moneyLose;
        if (moneyText != null)
            moneyText.text = ressources.money.ToString();
    }

    public void EarnDetachedPieces(float detachedPiecesEarned)
    {
        ressources.detachedPieces += detachedPiecesEarned;
        if (detachedPiecesText != null)
            detachedPiecesText.text = ressources.detachedPieces.ToString();
    }

    public void LoseDetachedPieces(float detachedPiecesLose)
    {
        ressources.detachedPieces -= detachedPiecesLose;
        if (detachedPiecesText != null)
            detachedPiecesText.text = ressources.detachedPieces.ToString();
    }

    public void GetNewPlans(Unit unit, int number)
    {
        unit.plansCurrent += number;

        if (unit.unitFeedbacks != null)
            unit.unitFeedbacks.CheckLocking();
        unit.unitFeedbacks.ShowUnitData();
    }
}
