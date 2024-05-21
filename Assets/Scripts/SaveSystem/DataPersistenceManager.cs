using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.SceneManagement;
using UnityEditor.Search;

public class DataPersistenceManager : MonoBehaviour
{
    public static DataPersistenceManager Instance { get; private set; }

    [Header("Debugging")]
    [SerializeField] private bool disableDataPersistence = false;
    [SerializeField] private bool initializeDataIfNull = false;
    [SerializeField] private bool overrideSelectedProfileID = false;
    [SerializeField] private bool overrideSaveName = false;
    [SerializeField] private string debugSelectedProfileID;
    [SerializeField] private string debugSaveName;


    [SerializeField]
    private string fileName;

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;
    private string selectedProfileID = "test";

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("DataPersistenceManager is already present in this scene! Destroying the newest object.");
            Destroy(this.gameObject);
            return;
        }

        if (disableDataPersistence)
        {
            Debug.LogWarning("Data Persistence is disabled!");
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);

        this.selectedProfileID = dataHandler.GetMostRecentlyUpdatedProfileID();

        if (overrideSelectedProfileID)
        {
            this.selectedProfileID = debugSelectedProfileID;
            Debug.LogWarning("Selected profile ID is overriden to " + debugSelectedProfileID);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        if (disableDataPersistence) return;

        // load saved data from file using data handler
        // returns null if save file not present
        this.gameData = dataHandler.Load(selectedProfileID);

        if (this.gameData == null && initializeDataIfNull)
        {
            if (overrideSaveName)
            {
                this.fileName = debugSaveName;
                this.gameData = dataHandler.Load(selectedProfileID);
            }
            else NewGame();
        }

        // if there is no data, initialize to a new game
        if (gameData ==  null)
        {
            Debug.Log("No game data was found. A new game needs to be started.");
            return;
        }
        // push the game data to all scripts that need it
        foreach (IDataPersistence dataPersistenceObject in dataPersistenceObjects)
        {
            dataPersistenceObject.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        if (disableDataPersistence) return;

        if (this.gameData  == null)
        {
            Debug.LogWarning("No Data waas found. A new game needs to be started before data can be saved.");
            return;
        }

        // pass game data to all scripts that need to modify it
        foreach (IDataPersistence dataPersistenceObject in dataPersistenceObjects)
        {
            dataPersistenceObject.SaveData(gameData);
        }

        gameData.lastUpdated = System.DateTime.Now.ToBinary();

        // save game data to a file using data handler
        dataHandler.Save(gameData, selectedProfileID);
    }

    public void DeleteGame(SaveSlot slot)
    {
        dataHandler.Delete(slot.GetProfileID());
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public bool HasGameData()
    {
        return this.gameData != null;
    }

    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return dataHandler.LoadAllProfiles();
    }

    public void ChangeSelectedProfileID(string newProfileID)
    {
        this.selectedProfileID = newProfileID;
        LoadGame();
    }
}
