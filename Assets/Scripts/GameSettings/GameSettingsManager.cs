using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class GameSettingsManager : MonoBehaviour
{
    public static GameSettingsManager instance { get; private set; }

    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    private string selectedProfileId = "gamesettings";

    private List<ISettingsDataPersistence> dataPersistenceObjects;
    public GameSettingsData settingsData;
    private FileDataHandler dataHandler;

    [SerializeField] private TextMeshProUGUI textMeshProUGUI;

    private void Awake()
    {

        if (instance != null)
        {
            Debug.Log("Found more than one Data Persistence Manager in the scene. Destroying the newest one.");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);


        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);

        //this.selectedProfileId = dataHandler.GetMostRecentlyUpdatedProfileId();
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadSettings();
    }

    private void LoadSettings()
    {
        // load any saved data from a file using the data handler
        dataHandler.Load(selectedProfileId, out this.settingsData);

        // start a new game if the data is null and we're configured to initialize data for debugging purposes
        if (this.settingsData == null)
        {
            NewSettings();
        }

        // if no data can be loaded, don't continue
        if (this.settingsData == null)
        {
            Debug.Log("No data was found. A New Game needs to be started before data can be loaded.");
            return;
        }

        // push the loaded data to all other scripts that need it
        foreach (ISettingsDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(settingsData);
        }

    }

    public void UpdateWithSettings()
    {

        if (Screen.fullScreen != settingsData.fullscreen || settingsData.resolutionWidth != Screen.currentResolution.width || settingsData.resolutionHeight != Screen.currentResolution.height)
        {
            Screen.SetResolution(settingsData.resolutionWidth, settingsData.resolutionHeight, settingsData.fullscreen);
        }
    }

    private void NewSettings()
    {
        this.settingsData = new GameSettingsData();
    }

    public void OnSceneUnloaded(Scene scene)
    {
        SaveSettings();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void SaveSettings()
    {
        // if we don't have any data to save, log a warning here
        if (this.settingsData == null)
        {
            Debug.LogWarning("No data was found. A New Game needs to be started before data can be saved.");
            return;
        }

        // pass the data to other scripts so they can update it
        foreach (ISettingsDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(settingsData);
        }

        // timestamp the data so we know when it was last saved
        settingsData.lastUpdated = System.DateTime.Now.ToBinary();

        Debug.Log("It's time to Save Game Setting: " + selectedProfileId);

        // save that data to a file using the data handler
        dataHandler.Save(settingsData, selectedProfileId);
    }

    private List<ISettingsDataPersistence> FindAllDataPersistenceObjects()
    {
        //IEnumerable<ISettingsDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<ISettingsDataPersistence>();
        IEnumerable<ISettingsDataPersistence> dataPersistenceObjects = Resources.FindObjectsOfTypeAll(typeof(MonoBehaviour)).OfType<ISettingsDataPersistence>();
        return new List<ISettingsDataPersistence>(dataPersistenceObjects);
    }

    public void Update()
    {
        UpdateWithSettings();
    }
    public void Start()
    {
        UpdateWithSettings();
    }
    private void OnApplicationQuit()
    {
        SaveSettings();
    }
}
