using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RewardCard : MonoBehaviour
{
    [Header("Datas")]
    public int rewardNumber;
    public string rewardName;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI rewardNumberText;
    [SerializeField] private TextMeshProUGUI rewardNameText;

    public void Start()
    {
        rewardNameText.text = rewardName;
        rewardNumberText.text = rewardNumber.ToString();
    }
}
