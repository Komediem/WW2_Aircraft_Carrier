using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyGain : MonoBehaviour
{
    
    [SerializeField] private TextMeshPro MoneyAvailable;

    [Header("Money Count + Add")]
    [SerializeField] private float MoneyAvailableCount;
    [SerializeField] private float MoneyAvailableByCycle;
    [SerializeField] private float MoneyAvailableMax;

    [Header ("Timer")]
    [SerializeField] private float TimeRemaining;
    [SerializeField] private float BaseTimeRemaining;

    [Header ("Upgrades")]
    [SerializeField] private float UpgradeMoneyPlus;
    [SerializeField] private TextMeshProUGUI CostReduceTimer;
    [SerializeField] private TextMeshPro CostUpgradeGain;
    [SerializeField] private float CostTimer;
    [SerializeField] private float CostGain;

    void Update()
    {
        if (TimeRemaining > 0)
        {
            TimeRemaining -= Time.deltaTime;
            if (TimeRemaining <= 0 )
            {
                TimeRemaining = BaseTimeRemaining;
                MoneyAvailablePlus();
            }
        }
    }

    //MoneyAvailable(TMP)
    public void MoneyAvailablePlus()
    {
        MoneyAvailableCount += MoneyAvailableByCycle;
        MoneyAvailable.text = MoneyAvailableCount.ToString();
    }

    //MoneyAvailable(BUTTON)
    //MoneyOwned(TMP)
    public void ClaimMoney()
    {
        RessourceManager.Instance.EarnMoney(MoneyAvailableCount);
        MoneyAvailableCount = 0;
        MoneyAvailable.text = MoneyAvailableCount.ToString();
    }

    //UpgradeReduceTimer(BUTTON)
    /*public void UpgradeReduceTimer()
    {
        if (RessourceManager.Instance.money >= CostTimer && BaseTimeRemaining > 2)
        {
            BaseTimeRemaining -= 1;
            RessourceManager.Instance.money -= CostTimer;
            CostTimer += CostTimer;
            //CostReduceTimer.text = CostTimer.ToString();
            RessourceManager.Instance.LoseMoney(CostTimer);
        }
    }*/

    //UpgradeMoneyGain(BUTTON)
    public void UpgradeMoreMoney()
    {
        if (RessourceManager.Instance.money >= CostGain)
        {
            MoneyAvailableByCycle += UpgradeMoneyPlus;
            RessourceManager.Instance.LoseMoney(CostGain);
            CostGain += CostGain;
            CostUpgradeGain.text = CostGain.ToString();
        }
    }
}
