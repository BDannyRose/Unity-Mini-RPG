using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField]
    private string profileID = "";

    [Header("Content")]
    [SerializeField]
    private GameObject noDataContent;
    [SerializeField]
    private GameObject hasDataContent;
    [SerializeField]
    private TextMeshProUGUI levelText;
    [SerializeField]
    private TextMeshProUGUI skillPointsText;

    private Button saveSlotButton;

    private void Awake()
    {
        saveSlotButton = this.GetComponent<Button>();
    }

    public void SetData(GameData gameData)
    {
        if (gameData == null)
        {
            noDataContent.SetActive(true);
            hasDataContent.SetActive(false);
        }
        else
        {
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);

            levelText.text = "Level: " + gameData.level.ToString();
            skillPointsText.text = "Skill points: " + gameData.skillPoints.ToString();
        }
    }

    public string GetProfileID()
    {
        return profileID;
    }
    public void SetInteractable(bool interactable)
    {
        saveSlotButton.interactable = interactable;
    }
}
