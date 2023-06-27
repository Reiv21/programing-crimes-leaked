using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using System;

public class MenuScript : MonoBehaviour
{
    public SaveSettingAndMenuConfig saveSettingAndMenuConfig;
    public GameObject MainMainMenu;
    public GameObject LoadingScreen;
    public GameObject OptionsMenu;
    public GameObject PlayMenu;
    public GameObject AuthorMenu;
    public GameObject ExitMenu;
    public GameObject HeroMenu;
    public GameObject MapMenu;
    public GameObject UpgradeMenu;
    public GameObject ShopMenu;
    public GameObject DiaryMenu;
    public GameObject NewGameMenu;
    public GameObject ContinueMenu;
    public GameObject ChooseGameMenu;

    public SaveToFile saveToFile;
    public GameLoader gameLoader;

    public Image LoadingBarFill;

    public List<char> bannedCharsForName;

    //Hero Menu
    public GameObject activeHero;
    public HeroConfig heroConfig;
    public TMP_Text heroName;
    public TMP_Text heroDesc;
    public TMP_Text spellOneName;
    public TMP_Text spellTwoName;
    public TMP_Text spellOneDesc;
    public TMP_Text spellTwoDesc;
    public RawImage spellOneImg;
    public RawImage spellTwoImg;
    int choosenHeroId;
    public List<GameObject> heroButtonsList;
    public Texture lockedHeroImg;

    //Map Menu
    public GameObject selectButton;
    public GameObject summary;
    public TMP_Text mapHeroName;
    public RawImage mapHeroImg;
    public RawImage mapHeroSpell1Img;
    public RawImage mapHeroSpell2Img;

    public TMP_Text mapName;
    public TMP_Text mapDesc;
    public GameObject activeDifficultyMode;

    int difficulty;
    int mapSelected;

    //New Game Menu
    public TMP_InputField inputField;
    public GameObject yourNameIsValid;
    public GameObject nameSumbit;
    public GameObject overwriteMenu;
    public GameObject buttonPrefab;
    public GameObject contener;

    //Continue Menu
    public GameObject saveSumbitButton;
    public GameObject noSavesDetected;
    string chooseSaveDirectory;
    string chooseSaveDeleteDirectory;
    public TMP_Text nameOfSave;
    public GameObject deleteSaveMenu;
    public TMP_Text deleteSaveTxt;
    public Button deleteSaveConfirmButton;

    // Start is called before the first frame update
    void Start()
    {
        heroConfig = GetComponent<HeroConfig>();
        saveToFile = GetComponent<SaveToFile>();
        gameLoader = GetComponent<GameLoader>();
    }

    public void Back()
    {
        MainMainMenu.SetActive(true);
        AuthorMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        PlayMenu.SetActive(false);
        HeroMenu.SetActive(false);
        ChooseGameMenu.SetActive(false);
    }
    public void BackToPlayMenu()
    {
        DiaryMenu.SetActive(false);
        ShopMenu.SetActive(false);
        UpgradeMenu.SetActive(false);
        MapMenu.SetActive(false);
        HeroMenu.SetActive(false);
        PlayMenu.SetActive(true);
    }
    public void BackToChooseMenu()
    {
        ContinueMenu.SetActive(false);
        NewGameMenu.SetActive(false);
        ChooseGameMenu.SetActive(true);
    }
    public void ExitButton()
    {
        ExitMenu.SetActive(true);
    }
    public void YesExitButton()
    {
        Application.Quit();
    }
    public void NoExitButton()
    {
        ExitMenu.SetActive(false);
    }
    public void AuthorButton()
    {
        MainMainMenu.SetActive(false);
        AuthorMenu.SetActive(true);
    }
    public void OptionsButton()
    {
        MainMainMenu.SetActive(false);
        OptionsMenu.SetActive(true);
    }
    public void PlayButton()
    {
        MainMainMenu.SetActive(false);
        ChooseGameMenu.SetActive(true);
    }
    public void MapButton()
    {
        selectButton.GetComponent<Button>().enabled = false;
        selectButton.GetComponent<Image>().color = new Color32(137, 137, 137,100);
        activeDifficultyMode.SetActive(false);

        summary.SetActive(false);
        MapMenu.SetActive(true);
        PlayMenu.SetActive(false);
    }
    public void UpgradeButton()
    {
        UpgradeMenu.SetActive(true);
        PlayMenu.SetActive(false);
    }
    public void ShopButton()
    {
        ShopMenu.SetActive(true);
        PlayMenu.SetActive(false);
    }
    public void DiaryButton()
    {
        DiaryMenu.SetActive(true);
        PlayMenu.SetActive(false);
    }
    public void ContinueButton()
    {
        foreach (Transform child in contener.transform)
        {
            Destroy(child.gameObject);
        }
        nameOfSave.text = null;
        deleteSaveMenu.SetActive(false);
        saveSumbitButton.GetComponent<Button>().interactable = false;
        gameLoader.ReadDirectories();
        ChooseGameMenu.SetActive(false);
        ContinueMenu.SetActive(true);
    }
    public void NewGameButton()
    {
        overwriteMenu.SetActive(false);
        nameSumbit.GetComponent<Button>().interactable = false;
        inputField.text = null;
        ChooseGameMenu.SetActive(false);
        NewGameMenu.SetActive(true);
    }
    public void LoadSaveButton(string saveName)
    {
        chooseSaveDirectory = saveName;
        saveSumbitButton.GetComponent<Button>().interactable = true;
        nameOfSave.text = Path.GetFileName(chooseSaveDirectory);
    }
    public void SumbitLoadSave()
    {
        PlayMenu.SetActive(true);
        ContinueMenu.SetActive(false);
        gameLoader.saveDirectory = chooseSaveDirectory;
        gameLoader.LoadFromFile(true);
    }
    public void DeleteSave(string saveName)
    {
        chooseSaveDeleteDirectory = saveName;
        deleteSaveConfirmButton.interactable = false;
        StartCoroutine(DeleteSaveButtonCooldown());
        deleteSaveTxt.text = Path.GetFileName(chooseSaveDeleteDirectory);
        deleteSaveMenu.SetActive(true);

    }
    public void DeleteSaveConfirm()
    {
        deleteSaveMenu.SetActive(false);
        Directory.Delete(chooseSaveDeleteDirectory, true);
        chooseSaveDirectory = null; chooseSaveDeleteDirectory = null;
        foreach (Transform child in contener.transform)
        {
            Destroy(child.gameObject);
        }
        gameLoader.ReadDirectories();
    }
    public void DeleteSaveNo() 
    {
        deleteSaveMenu.SetActive(false);
    }
    IEnumerator DeleteSaveButtonCooldown()
    {
        yield return new WaitForSeconds(3);
        deleteSaveConfirmButton.interactable= true;
        yield return null;
    }
    public void CheckName(string saveName)
    {
        // jeœli napisze np aaa>>> to nie wywala b³êdu :(   
        if(saveName.Length == 0)
        {
            nameSumbit.GetComponent<Button>().interactable = false;
            return; 
        }
        char c;
        char c2;
        bool nameIsGood = true;
        for (int i = 0; i < bannedCharsForName.Count; i++)
        {
            print("1 " + nameIsGood);
            c = bannedCharsForName[i];
            c2 = saveName[0];

            if (saveName.Contains(c) || saveName == null || char.IsWhiteSpace(c2))
            {
                print("2 " + nameIsGood);
                nameIsGood = false;
                yourNameIsValid.SetActive(true);
                i = bannedCharsForName.Count - 1;
            }
        }
        print("3 " + nameIsGood);
        if (nameIsGood)
        {
            nameSumbit.GetComponent<Button>().interactable = true;
            saveSettingAndMenuConfig.activeSaveName = saveName;
        }
        else
        {
            nameSumbit.GetComponent<Button>().interactable = false;
        }
    }

    public void NameSubmitButton()
    {
        nameSumbit.GetComponent<Button>().interactable = false;
        saveToFile.CheckIfDirectoryExistIfNotSave();
    }
    public void DirectoryAlreadyExist()
    {
        overwriteMenu.SetActive(true);
    }
    public void OverwriteYes()
    {
        saveToFile.SaveFile(true);
    }
    public void OverwriteNo()
    {
        overwriteMenu.SetActive(false);
        inputField.text = null;
        saveSettingAndMenuConfig.activeSaveName = null;
    }
    public void StartNewGame()
    {
        NewGameMenu.SetActive(false);
        PlayMenu.SetActive(true);
    }

    public void NoSavesDetected()
    {
        noSavesDetected.SetActive(true);
    }
    public void SpawnSaveButton(string driName)
    {
        noSavesDetected.SetActive(false);
        GameObject button = Instantiate(buttonPrefab);
        button.transform.SetParent(contener.transform);
        button.GetComponentInChildren<TMP_Text>().text = driName;
        button.GetComponent<Button>().onClick.AddListener(delegate { LoadSaveButton("Saves/"+driName); });
        
        button.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { DeleteSave("Saves /" + driName); });
    }
    public void HeroButton(int heroID)
    {
        for (int i = 0; i < heroButtonsList.Count; i++)
        {
            heroButtonsList[i].GetComponent<RawImage>().texture = lockedHeroImg;
            heroButtonsList[i].GetComponent<Button>().interactable = false;
            for (int j = 0; j < saveSettingAndMenuConfig.unlockedHeroesList.Count; j++)
            {
                if (Int32.Parse(heroButtonsList[i].name) == saveSettingAndMenuConfig.unlockedHeroesList[j])
                {
                    heroButtonsList[i].GetComponent<RawImage>().texture = heroConfig.heroList[Int32.Parse(heroButtonsList[i].name)].heroImage;
                    heroButtonsList[i].GetComponent<Button>().interactable = true;
                }

            }
        }
        if(heroID == -1)
        {
            Debug.Log(saveSettingAndMenuConfig.heroId);
            heroID = saveSettingAndMenuConfig.heroId;
        }

        choosenHeroId = heroID;

        if (heroID == 0)
        {
            activeHero.GetComponent<RectTransform>().localPosition = new Vector3(-683f, 133f, 0f);
        }
        else if (heroID == 1)
        {
            activeHero.GetComponent<RectTransform>().localPosition = new Vector3(-298f, 133f, 0f);
        }
        else if (heroID == 2)
        {
            activeHero.GetComponent<RectTransform>().localPosition = new Vector3(87f,133f,0f);
        }
        else if (heroID == 3)
        {
            activeHero.GetComponent<RectTransform>().localPosition = new Vector3(-683f, -242f, 0f);
        }
        else if (heroID == 4)
        {
            activeHero.GetComponent<RectTransform>().localPosition = new Vector3(-298f, -242f, 0f);
        }
        else
        {
            activeHero.GetComponent<RectTransform>().localPosition = new Vector3(87f, -242f, 0f);
        }

        heroName.text = heroConfig.heroList[heroID].name;
        heroDesc.text = heroConfig.heroList[heroID].description;
        spellOneName.text = heroConfig.heroList[heroID].spellOneName;
        spellTwoName.text = heroConfig.heroList[heroID].spellTwoName;
        spellOneDesc.text = heroConfig.heroList[heroID].spellOneDesc;
        spellTwoDesc.text = heroConfig.heroList[heroID].spellTwoDesc;
        spellOneImg.texture = heroConfig.heroList[heroID].spellOneImg;
        spellTwoImg.texture = heroConfig.heroList[heroID].spellTwoImg;
        
        PlayMenu.SetActive(false);
        HeroMenu.SetActive(true);
        
    }
    public void Save()
    {
        saveSettingAndMenuConfig.heroId = choosenHeroId;
        saveToFile.saveSettingAndMenuConfig = saveSettingAndMenuConfig;
        saveToFile.SaveFile(true);
        Back();
    }
    public void LoadMap()
    {
        SaveSettingAndMenuConfig.Map map = new SaveSettingAndMenuConfig.Map();
        for (int i = 0; i < saveSettingAndMenuConfig.MapsList.Count; i++)
        {
            map = saveSettingAndMenuConfig.MapsList[i];
            if(map.levelOfDifficult == difficulty && map.mapIDforButtonsInMapMenu == mapSelected)
            {
                break;
            }
        }
        saveToFile.saveSettingAndMenuConfig = saveSettingAndMenuConfig;
        saveToFile.SaveFile(true);
        StartCoroutine(LoadSceneAsync(map.sceneID));
    }
    IEnumerator LoadSceneAsync(int sceneId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

        LoadingScreen.SetActive(true);
            
        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            LoadingBarFill.fillAmount = progressValue;
            yield return null;
        }
    }

    //Map Menu
    public void SelectMap(int mapID)
    {
        if (summary.activeSelf && mapSelected == mapID)
        {
            summary.SetActive(false);
        }
        else
        {
            mapSelected = mapID;
            SaveSettingAndMenuConfig.Map mapClassSelected = new SaveSettingAndMenuConfig.Map();
            for (int i = 0; i < saveSettingAndMenuConfig.MapsList.Count; i++)
            {
                mapClassSelected = saveSettingAndMenuConfig.MapsList[i];
                print(i);
                if(mapClassSelected.mapIDforButtonsInMapMenu == mapSelected)
                {
                    i = saveSettingAndMenuConfig.MapsList.Count - 1;
                    //konczy petle
                }
            }
            mapDesc.text = mapClassSelected.desc;
            mapName.text = mapClassSelected.name;
            mapHeroName.text = heroConfig.heroList[saveSettingAndMenuConfig.heroId].name;
            mapHeroImg.texture = heroConfig.heroList[saveSettingAndMenuConfig.heroId].heroImage;
            mapHeroSpell1Img.texture = heroConfig.heroList[saveSettingAndMenuConfig.heroId].spellOneImg;
            mapHeroSpell2Img.texture = heroConfig.heroList[saveSettingAndMenuConfig.heroId].spellTwoImg;

            summary.SetActive(true);
        }
    }
    public void EasyButton()
    {
        difficulty = 1;
        activeDifficultyMode.SetActive(true);
        activeDifficultyMode.transform.localPosition = new Vector3(-159.8f, -122f);
        StartButtonActiveting();
    }
    public void MediumButton()
    {
        difficulty = 2;
        activeDifficultyMode.SetActive(true);
        activeDifficultyMode.transform.localPosition = new Vector3(-0.1804f, -122f);
        StartButtonActiveting();
    }
    public void HardButton()
    {
        difficulty = 3;
        activeDifficultyMode.SetActive(true);
        activeDifficultyMode.transform.localPosition = new Vector3(160.3f, -122f);
        StartButtonActiveting();
    }

    void StartButtonActiveting()
    {
        selectButton.GetComponent<Button>().enabled = true;
        selectButton.GetComponent<Image>().color = new Color32(90, 217, 255, 100);
    }
}
