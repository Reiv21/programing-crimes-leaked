using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public class GameLoader : MonoBehaviour
{
    public string saveName = "savedGame";
    public string saveDirectory;
    public SaveSettingAndMenuConfig saveSettingAndMenuConfig;
    public MenuScript menuScript;

    private void Start()
    {
        menuScript = GetComponent<MenuScript>();
    }
    public void ReadDirectories()
    {
       string[] subdirs = Directory.GetDirectories("Saves/");
        if (subdirs.Length == 0)
        {
            menuScript.NoSavesDetected();
        }
        else
        {
            for(int i = 0; i < subdirs.Length; i++)
            {
                menuScript.SpawnSaveButton(Path.GetFileName(subdirs[i]));
            }   
        }
    }
    public void LoadFromFile(bool isForMenu)
    {

        BinaryFormatter formatter = new BinaryFormatter();

        FileStream saveFile = File.Open(saveDirectory + "/" + saveName + ".bin", FileMode.Open);

        saveSettingAndMenuConfig = (SaveSettingAndMenuConfig) formatter.Deserialize(saveFile);

        if (isForMenu)
        {
            menuScript.saveSettingAndMenuConfig = saveSettingAndMenuConfig;
        }
    }
}
