using DG.Tweening;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[DefaultExecutionOrder(-100)]  //越小越优先执行
public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    private List<ISaveable> saveableList = new List<ISaveable>();

    public VoidEvent_SO saveDataEvent;

    private Data saveData;

    public VoidEvent_SO loadDataEvent;

    private string jsonFolder;

    private void Awake()
    {
        if(instance !=null)
        {
            Destroy(instance.gameObject);
        }
        instance = this;

        saveData = new Data();

        jsonFolder = Application.persistentDataPath + "/SaveData";

        ReadSaveData();
    }

    private void OnEnable()
    {
        saveDataEvent.OnEventRaised += Save;
        loadDataEvent.OnEventRaised += Load;
    }
    private void OnDisable()
    {
        saveDataEvent.OnEventRaised -= Save;
        loadDataEvent.OnEventRaised -= Load;
    }

    private void Update()
    {
        if(Keyboard.current.lKey.wasPressedThisFrame)
        {
            Load();
        }
    }

    public void RegisterSaveData(ISaveable saveable)
    {
        if(!saveableList.Contains(saveable))
        {
            saveableList.Add(saveable);
        }
    }

    public void UnRegisterSaveData(ISaveable saveable)
    {
        saveableList.Remove(saveable);
    }

    public void Save()
    {
        foreach(ISaveable saveable in saveableList)
        {
            saveable.GetSaveData(saveData);
        }

        var resultPath = jsonFolder + "/data.sav";

        var jsonData = JsonConvert.SerializeObject(saveData, Formatting.Indented);

        if(!File.Exists(resultPath))
        {
            Directory.CreateDirectory(jsonFolder);
        }

        File.WriteAllText(resultPath, jsonData);
    }

    public void Load()
    {
        foreach (ISaveable saveable in saveableList)
        {
            saveable.LoadData(saveData);
        }
    }

    private void ReadSaveData()
    {
        var resultPath = jsonFolder + "/data.sav";

        if (File.Exists(resultPath))
        {
            var stringData = File.ReadAllText(resultPath);

            var jsonData = JsonConvert.DeserializeObject<Data>(stringData);

            saveData = jsonData;
        }
    }
}
