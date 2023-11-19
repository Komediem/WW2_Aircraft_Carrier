using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Diagnostics;

public class MoneyGain : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI MoneyAvailable;

    [Header("Money Owned")]
    [SerializeField] private TextMeshProUGUI MoneyOwned;
    [SerializeField] private float MoneyCountOwned;

    [Header("Money Count + Add")]
    [SerializeField] private float MoneyDisplayCount;
    [SerializeField] private float MoneyPlus;

    [Header ("Timer")]
    [SerializeField] private float TimeRemaining;
    [SerializeField] private float BaseTimeRemaining;

    [Header ("Upgrades")]
    [SerializeField] private float UpgradeMoneyPlus;

    //Upgrade's costs ++ New price for next upgrade.

//Now it needs upgrades available to reduce the timer / get more money by claim !

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
        MoneyDisplayCount += MoneyPlus;
        MoneyAvailable.text = MoneyDisplayCount.ToString();
    }

    //MoneyAvailable(BUTTON)
    //MoneyOwned(TMP)
    public void ClaimMoney()
    {
        MoneyCountOwned += MoneyDisplayCount;
        MoneyDisplayCount = 0;
        MoneyAvailable.text = MoneyDisplayCount.ToString();
        MoneyOwned.text = MoneyCountOwned.ToString();
    }

    //UpgradeReduceTimer(BUTTON)
    public void UpgradeReduceTimer(float Cost)
    {
        if (MoneyCountOwned >= Cost && BaseTimeRemaining > 2)
        {
            BaseTimeRemaining -= 1;
            MoneyCountOwned -= Cost;
            Cost += Cost;
            MoneyOwned.text = MoneyCountOwned.ToString();
        }
    }

    //UpgradeMoneyGain(BUTTON)
    public void UpgradeMoreMoney(float Cost)
    {
        if (MoneyCountOwned >= Cost)
        {
            MoneyPlus += UpgradeMoneyPlus;
            MoneyCountOwned -= Cost;
            Cost += Cost;
            MoneyOwned.text = MoneyCountOwned.ToString();
        }
    }
}
