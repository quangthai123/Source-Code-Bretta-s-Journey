using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public FileDataHandler(string _dataDirPath, string _dataFileName)
    {
        this.dataDirPath = _dataDirPath;
        this.dataFileName = _dataFileName;
    }
    public void Save(GameDatas _data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);  
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string dataToStore = JsonUtility.ToJson(_data, true);
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        } catch(Exception e)
        {
            Debug.Log("Error when trying to save data to file " + fullPath + "\n" + e);
        }
    }
    public GameDatas Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameDatas loadData = null;
        if(File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                //loadData = JsonUtility.FromJson<GameDatas>(dataToLoad);
                //Debug.Log("Data to load: " + dataToLoad);

                loadData = ScriptableObject.CreateInstance<GameDatas>();
                JsonUtility.FromJsonOverwrite(dataToLoad, loadData);

                //Debug.Log("Loaded Data: " + JsonUtility.ToJson(loadData, true));
            } catch (Exception e)
            {
                Debug.Log("Error when trying to load data from file " + fullPath + "\n" + e);
            }
        }
        return loadData;
    }
    public void DeleteSavedFile()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        if(File.Exists(fullPath)) 
            File.Delete(fullPath);
    }
}
