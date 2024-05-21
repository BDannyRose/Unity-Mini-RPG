using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load(string profileID)
    {
        if (profileID == null)
        {
            return null;
        }

        string fullPath = Path.Combine(dataDirPath, profileID, dataFileName);
        GameData loadedData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                string dataJSON = "";

                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataJSON = reader.ReadToEnd();
                    }
                }

                loadedData = JsonUtility.FromJson<GameData>(dataJSON);
                Debug.Log("Successfully loaded data from " +  fullPath);
            }
            catch (Exception e)
            {
                Debug.LogError("Error while trying to save data to: " + fullPath + "\n" + e);
            }
        }

        return loadedData;
    }

    public void Save(GameData data, string profileID)
    {
        if (profileID == null)
        {
            return;
        }

        string fullPath = Path.Combine(dataDirPath, profileID, dataFileName);
        try
        {
            // create directory if it doesn't exist yet
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // serialize GameData class to a JSON object
            string dataJSON = JsonUtility.ToJson(data, true);

            // write file to file system
            // this syntax means that we close the file as soon as the operation finishes
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataJSON);
                }
            }
            Debug.Log("Successfully saved data to " + fullPath);
        }
        catch (Exception e)
        {
            Debug.LogError("Error while trying to save data to: " + fullPath + "\n" + e);
        }
    }

    public Dictionary<string, GameData> LoadAllProfiles()
    {
        Dictionary<string, GameData> profileDictionary = new Dictionary<string, GameData>();

        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataDirPath).EnumerateDirectories();
        foreach(DirectoryInfo dirInfo in dirInfos)
        {
            string profileID = dirInfo.Name;

            string fullPath = Path.Combine(dataDirPath, profileID, dataFileName);
            // check to see if the save file is present inside the directory
            if (!File.Exists(fullPath))
            {
                Debug.LogWarning($"Skippind directory {profileID} when loading all profiles because it doesn't contain data.");
                continue;
            }

            GameData profileData = Load(profileID);
            if (profileData != null)
            {
                profileDictionary.Add(profileID, profileData);
            }    
            else
            {
                Debug.LogError($"Tried to load profile \"{profileID}\" but something went wrong!");
            }
        }

        return profileDictionary;
    }

    public string GetMostRecentlyUpdatedProfileID()
    {
        string mostRecentProfileID = null;

        Dictionary<string, GameData> profilesGameData = LoadAllProfiles();
        foreach (KeyValuePair<string, GameData> pair in profilesGameData)
        {
            string profileID = pair.Key;
            GameData gameData = pair.Value;

            if (gameData == null)
            {
                continue;
            }

            if (mostRecentProfileID == null)
            {
                mostRecentProfileID = profileID;
            }
            else
            {
                DateTime mostRecent = DateTime.FromBinary(profilesGameData[mostRecentProfileID].lastUpdated);
                DateTime thisTime = DateTime.FromBinary(gameData.lastUpdated);
                if (thisTime > mostRecent)
                {
                    mostRecentProfileID = profileID;
                }
            }
        }

        return mostRecentProfileID;
    }

    public void Delete(string profileID)
    {
        string fullPath = Path.Combine(dataDirPath, profileID, dataFileName);
        try
        {
            Directory.Delete(Path.GetDirectoryName(fullPath));
        }
        catch (Exception e)
        {
            Debug.LogError("Error while trying to save data to: " + fullPath + "\n" + e);
        }
    }
}
