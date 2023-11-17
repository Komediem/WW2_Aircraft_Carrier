using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newCard", menuName = "pack Card")]
public class PackCard : ScriptableObject
{
    public CardType cardType;

    public enum CardType
    {
        money,
        detachedPieces,
        unit,
        other
    }
}
