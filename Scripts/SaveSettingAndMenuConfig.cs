using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SaveSettingAndMenuConfig
{
    public string activeSaveName;
    public int heroId;
    public List<string> unlockedTowerUpgradesList;
    public int difficultyLevel;
    public List<int> unlockedHeroesList;
    public List<Map> MapsList;

    [Serializable]
    public class Map
    {
        public string name;
        public string desc;
        public int levelOfDifficult;
        public int sceneID;
        public int mapIDforButtonsInMapMenu;
    }
}
