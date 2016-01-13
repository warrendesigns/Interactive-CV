using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour
{
    //objects/classes
    private Text moneyText;
    private Text experienceText;
    private Text daysText;
    private Text skillText;
    private Text bugText;
    private RectTransform experienceBar;
    private PlayerController playerController;

    //called before start: get references
    private void Awake()
    {
        moneyText = GameObject.Find("Money Text").GetComponent<Text>();
        experienceText = GameObject.Find("Experience Text").GetComponent<Text>();
        daysText = GameObject.Find("Days Left Text").GetComponent<Text>();
        skillText = GameObject.Find("Skill Level Text").GetComponent<Text>();
        bugText = GameObject.Find("Bug Text").GetComponent<Text>();
        experienceBar = GameObject.Find("Experience Bar - Fill").GetComponent<RectTransform>();
        playerController =  GetComponent<PlayerController>();
    }

    //------------------------------------------------------------------------------------------------------------------------------
    public void UpdateDaysUI (int days)
    {
        daysText.text = "Days   Left: " + days;
    }

    //------------------------------------------------------------------------------------------------------------------------------
    public void UpdateMoneyUI (int money)
    {
        moneyText.text = "Balance: £" + money;
    }

    //------------------------------------------------------------------------------------------------------------------------------
    public void UpdateExperienceUI (float exp, float total)
    {
        experienceText.text = "Experience: " + Mathf.Round(exp) + " / " + total;

        float scale = exp / total;

        if (scale < 0)
        {
            scale = 0;
        }
        if (scale == 1)
        {
            playerController.PlayerLevelUp();
        }

        experienceBar.localScale = new Vector3(scale, experienceBar.localScale.y, experienceBar.localScale.z);
    }

    //------------------------------------------------------------------------------------------------------------------------------
    public void UpdateSkillUI (string skill)
    {
        skillText.text = "Skill   Level: " + skill;
    }

    //------------------------------------------------------------------------------------------------------------------------------
    public void UpdateBugUI (int bugs, int total, bool isEnabled)
    {
        bugText.text = "Bugs   Left: " + bugs + " / " + total;
        bugText.enabled = isEnabled;
    }

    public void UpdateBugUI(bool isEnabled)
    {
        bugText.enabled = isEnabled;
    }
}
