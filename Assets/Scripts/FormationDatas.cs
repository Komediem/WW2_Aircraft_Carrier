using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FormationDatas : MonoBehaviour
{
    public Formations formation;

    [Header("Text Feedbacks")]
    [SerializeField] private TextMeshProUGUI formationName;
    [SerializeField] private TextMeshProUGUI formationDescription;

    [Header("Image Feedbacks")]
    [SerializeField] private Image formationImage;

    public void Start()
    {
        DisplayFormationDatas();
    }

    public void DisplayFormationDatas()
    {
        if(formationName != null)
        formationName.text = formation.formationName;

        if(formationImage != null)
        formationImage.sprite = formation.formationImage;

        if(formationDescription != null)
        formationDescription.text = formation.formationDescription;
    }
}
