using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public void ShowElement(GameObject elementToShow)
    {
        elementToShow.SetActive(true);
    }

    public void HideElement(GameObject elementToHide)
    {
        elementToHide.SetActive(false);
    }
}
