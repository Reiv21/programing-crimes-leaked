using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public class SaveToFile : MonoBehaviour
{
    public SaveSettingAndMenuConfig startSaveSettingAndMenuConfig = new SaveSettingAndMenuConfig();
    public SaveSettingAndMenuConfig saveSettingAndMenuConfig = new SaveSettingAndMenuConfig();
    public SaveGame saveGame;
    public string saveName = "savedGame";
    public string directoryName;

    // Start is called before the first frame update
    
    public void CheckIfDirectoryExistIfNotSave()
    {
        if (Directory.Exists("Saves/"+GetComponent<MenuScript>().saveSettingAndMenuConfig.activeSaveName))
        {
            GetComponent<MenuScript>().DirectoryAlreadyExist();
        }
        else
        {
            StartNewProfile();
        }
    }
    public void SaveFile(bool isForMenu)
    {
        if (isForMenu)
        {
             saveSettingAndMenuConfig = GetComponent<MenuScript>().saveSettingAndMenuConfig;
        }
        directoryName = saveSettingAndMenuConfig.activeSaveName;
        if(!Directory.Exists("Saves"))
           Directory.CreateDirectory("Saves");
        if (!Directory.Exists("Saves/" + directoryName))
            Directory.CreateDirectory("Saves/" + directoryName);

        BinaryFormatter formatter = new BinaryFormatter();

        FileStream saveFile = File.Create("Saves/" + directoryName + "/" + saveName + ".bin");

        formatter.Serialize(saveFile, saveSettingAndMenuConfig);

        saveFile.Close();

        print("game saved to " + Directory.GetCurrentDirectory().ToString() + "/saves/" + saveName + ".bin");
    }

    public void StartNewProfile()
    {
        
        startSaveSettingAndMenuConfig.activeSaveName = GetComponent<MenuScript>().saveSettingAndMenuConfig.activeSaveName;
        directoryName = startSaveSettingAndMenuConfig.activeSaveName;
        if (!Directory.Exists("Saves"))
            Directory.CreateDirectory("Saves");
        if (!Directory.Exists("Saves/" + directoryName))
            Directory.CreateDirectory("Saves/" + directoryName);

        BinaryFormatter formatter = new BinaryFormatter();

        FileStream saveFile = File.Create("Saves/" + directoryName + "/" + saveName + ".bin");

        formatter.Serialize(saveFile, startSaveSettingAndMenuConfig);

        saveFile.Close();

        print("game saved to " + Directory.GetCurrentDirectory().ToString() + "/saves/" + saveName + ".bin");

        GetComponent<MenuScript>().saveSettingAndMenuConfig= startSaveSettingAndMenuConfig;

        GetComponent<MenuScript>().StartNewGame();
    }
}
