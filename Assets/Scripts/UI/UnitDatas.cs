using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitDatas : MonoBehaviour
{
    public Unit unit;

    #region Text Feedback
    [Header("Text Feedback")]
    [SerializeField] private TextMeshProUGUI unitName;
    [SerializeField] private TextMeshProUGUI unitLife;
    [SerializeField] private Slider unitLifeSlider;
    [SerializeField] private TextMeshProUGUI unitAttack;
    [SerializeField] private TextMeshProUGUI unitSpeed;
    [SerializeField] private TextMeshProUGUI unitCost;
    [SerializeField] private TextMeshProUGUI unitLevel;
    [SerializeField] private TextMeshProUGUI unitUpgrade;
    [SerializeField] private TextMeshProUGUI unitPlans;
    #endregion

    #region Unit Image
    [Header("Unit Type Image")]
    [SerializeField] private Sprite fighterIcon;
    [SerializeField] private Sprite assaultPlaneIcon;
    [SerializeField] private Sprite bomberIcon;
    [SerializeField] private Sprite antiAerialIcon;
    #endregion

    #region Unit Materials
    [Header("Unit Type Image")]
    [SerializeField] private Material commonMaterial;
    [SerializeField] private Material rareMaterial;
    [SerializeField] private Material epiqueMaterial;
    [SerializeField] private Material legendaryMaterial;
    #endregion

    [SerializeField] private Button upgrade;

    #region Image Feedback
    [Header("Image Feedback")]
    [SerializeField] private Image unitImage;
    [SerializeField] private Image unitType;
    [SerializeField] private Image cardBorder;
    [SerializeField] private Image cardBackground;
    #endregion

    #region Locked
    public GameObject lockedScreen;
    public GameObject hiddenInFightScreen;

    #endregion

    public void Start()
    {
        ShowUnitData();
        DisplayUnitType();
        DisplayBackgroundColor();
        CheckLocking();

        unit.unitFeedbacks = this;

        if (upgrade != null)
        {
            upgrade.onClick.AddListener(delegate { CheckLevel(); });
            upgrade.onClick.AddListener(delegate { ShowUnitData(); });
        }
    }

    public void ShowUnitData()
    {
        if(unitName != null)
        unitName.text = unit.unitName;

        if(unitLife != null) 
        unitLife.text = unit.currentLife.ToString();

        if(unitAttack != null) 
        unitAttack.text = unit.currentAttack.ToString();

        if(unitSpeed != null)
        unitSpeed.text = unit.currentSpeed.ToString();

        if(unitImage != null) 
        unitImage.sprite = unit.unitIcon;

        if(unitCost != null)
        unitCost.text = unit.moneyCost.ToString();

        if(unitLevel != null)
        unitLevel.text = unit.level.ToString();

        if(unitUpgrade != null)
        unitUpgrade.text = unit.currentUpgradeCostLevel.ToString();

        if(unitPlans != null)
        unitPlans.text = unit.plansCurrent.ToString() + " / " + unit.plansMax.ToString();

        if (unitLifeSlider != null)
        {
            unitLifeSlider.maxValue = unit.currentLife;
            unitLifeSlider.value = unitLifeSlider.maxValue;
        }
    }

    public void DisplayUnitType()
    {
        if (unitType != null)
        {
            if(unit.UnitType == Unit.unitType.Fighter)
            {
                unitType.sprite = fighterIcon;
            }

            else if(unit.UnitType == Unit.unitType.AssaultPlane)
            {
                unitType.sprite = assaultPlaneIcon;
            }

            else if(unit.UnitType == Unit.unitType.Bomber)
            {
                unitType.sprite = bomberIcon;
            }

            else if(unit.UnitType == Unit.unitType.AntiAerial)
            {
                unitType.sprite = antiAerialIcon;
            }
        }
    }

    public void DisplayBackgroundColor()
    {
        if(cardBorder != null)
        {
            if (unit.Rarity == Unit.rarity.Common)
            {
                //Color if common

                if(commonMaterial != null)
                {
                    cardBorder.material = commonMaterial;
                    cardBackground.material = commonMaterial;
                }
            }

            else if (unit.Rarity == Unit.rarity.Rare)
            {
                //Color if rare

                if (rareMaterial != null)
                {
                    cardBorder.material = rareMaterial;
                    cardBackground.material = rareMaterial;
                }
            }

            else if(unit.Rarity == Unit.rarity.Epique)
            {
                //Color if epique

                if (epiqueMaterial != null)
                {
                    cardBorder.material = epiqueMaterial;
                    cardBackground.material = epiqueMaterial;
                }
            }

            else if (unit.Rarity == Unit.rarity.Legendary)
            {
                //Color if legendary

                if (legendaryMaterial != null)
                {
                    cardBorder.material = legendaryMaterial;
                    cardBackground.material = legendaryMaterial;
                }
            }
        }
    }

    public void CheckLevel()
    {
        if(unit.level < unit.levelMax)
        {
            Garage.Instance.UpgradeUnit(unit);
        }

        else if(unit.level >= unit.levelMax)
        {
            if (unitUpgrade != null)
                unitUpgrade.text = "Merge";
        }
    }

    public void CheckLocking()
    {
        if (unit.plansCurrent >= unit.plansMax)
        {
            unit.isUnlocked = true;
        }

        if (unit.isUnlocked && lockedScreen != null)
        {
            lockedScreen.SetActive(false);
        }

        if(!unit.isUnlocked && lockedScreen != null)
        {
            lockedScreen.SetActive(true);
        }
    }

    public void ProposeVeteran()
    {

    }

    public void CheckUnitInFight()
    {
        if(unit != null && unit.isInFight && hiddenInFightScreen != null)
        {
            hiddenInFightScreen.SetActive(true) ;
        }

        else if(unit != null && !unit.isInFight && hiddenInFightScreen != null)
        {
            hiddenInFightScreen.SetActive(false);
        }
    }

    public void UpdateDatasInFight()
    {
        if(unitLifeSlider != null)
        unitLifeSlider.value = unit.currentLife;

        if (unitLife != null)
        unitLife.text = unit.currentLife.ToString();
    }
}
