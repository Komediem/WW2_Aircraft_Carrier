using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackClass : MonoBehaviour
{
    public List<PackCard> cards;

    [Header("moneyStats")]
    [SerializeField] private int minMoney, maxMoney;

    [Header("detachedPieces")]
    [SerializeField] private int min, max;

    private void Start()
    {
        foreach(PackCard card in cards)
        {
            switch(card.cardType)
            {
                case PackCard.CardType.money:



                    break;

                case PackCard.CardType.detachedPieces: 
                    


                    break;
            }
        }
    }
}
