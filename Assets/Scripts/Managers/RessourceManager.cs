using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RessourceManager : MonoBehaviour
{
    public static RessourceManager Instance;

    [Header("Datas")]
    public float money;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI moneyText;

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        if (moneyText != null)
            moneyText.text = money.ToString();
    }

    public void EarnMoney(float moneyEarn)
    {
        money += moneyEarn;
        if (moneyText != null)
            moneyText.text = money.ToString();
    }

    public void LoseMoney(float moneyLose)
    {
        money -= moneyLose;
        if (moneyText != null)
            moneyText.text = money.ToString();
    }
}
