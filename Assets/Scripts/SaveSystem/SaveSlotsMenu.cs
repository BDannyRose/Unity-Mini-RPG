using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSlotsMenu : Menu
{
    [Header("Menu navigation")]
    [SerializeField] private MainMenu mainMenu;

    [Header("Menu buttons")]
    [SerializeField] private Button backButton;

    private SaveSlot[] slots;
    private bool isLoadingGame = false;

    private void Awake()
    {
        slots = this.GetComponentsInChildren<SaveSlot>();
    }

    public void OnSaveSlotClicked(SaveSlot saveSlot)
    {
        DisableMenuButtons();
        DataPersistenceManager.Instance.ChangeSelectedProfileID(saveSlot.GetProfileID());
        
        if (!isLoadingGame)
        {
            DataPersistenceManager.Instance.NewGame();
        }
        
        // have to save BEFORE scene is unloaded
        DataPersistenceManager.Instance.SaveGame();

        SceneManager.LoadSceneAsync("SampleScene");
    }

    public void OnBackClicked()
    {
        mainMenu.ActivateMenu();
        this.DeactivateMenu();
    }

    public void ActivateMenu(bool isLoadingGame)
    {
        this.gameObject.SetActive(true);

        this.isLoadingGame = isLoadingGame;
        // load all existing profiles from memory
        Dictionary<string, GameData> profilesGameData = DataPersistenceManager.Instance.GetAllProfilesGameData();

        // loop through each save slot in the menu and set its content
        GameObject firstSelectedObject= backButton.gameObject;
        foreach (SaveSlot slot in slots)
        {
            GameData profileData = null;
            profilesGameData.TryGetValue(slot.GetProfileID(), out profileData);
            slot.SetData(profileData);
            if (profileData == null && this.isLoadingGame)
            {
                slot.SetInteractable(false);
            }
            else
            {
                slot.SetInteractable(true);
                if (firstSelectedObject.Equals(backButton.gameObject))
                {
                    firstSelectedObject = slot.gameObject;
                }
            }    
        }
        Button firstSelectedButton = firstSelectedObject.GetComponent<Button>();
        SetFirstSelected(firstSelectedButton);
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }

    private void DisableMenuButtons()
    {
        foreach (SaveSlot slot in slots)
        {
            slot.SetInteractable(false);
        }
        backButton.interactable = false;
    }
}
