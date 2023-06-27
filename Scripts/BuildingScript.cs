using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class BuildingScript : MonoBehaviour
{
    public GameLoader gameLoader;
    public GameObject Buttons;
    bool MenuIsActive = false;
    public Animator Animator;
    string BuiltAlready = null;
    string WhatToBuild;
    float TowerPrice;
    float ActualTowerPrice;
    bool FlagPosChoose = false;
    Vector3 SpawnTrasform;
    public GameObject AcceptButtonGameObject;
    public GameObject SellButtonGameObject;
    GameObject Building;
    LineRenderer circleRenderer;
    [Header("TextButtons")]
    public TMP_Text TextName;
    public TMP_Text TextDesc;
    public TMP_Text TextPrice;
    public TMP_Text TextButton1;
    public TMP_Text TextButton2;
    public TMP_Text TextButton3;
    public TMP_Text TextButton4;

    BuildingSetup buildingSetup = new BuildingSetup();
    BuildingSetup buttonBuildingSetup = new BuildingSetup();

    [Header("Building Setup")]
    public List<BuildingSetup> buildingsList = new();
    PlayerStatus PlayerStatus;
    void Awake()
    {
        gameLoader = GameObject.FindGameObjectsWithTag("GameMaster")[0].GetComponent<GameLoader>();
        for (int i = 0; i < buildingsList.Count; i++)
        {
            BuildingSetup building = buildingsList[i];
            for (int j = 0; j < gameLoader.saveSettingAndMenuConfig.unlockedTowerUpgradesList.Count; j++)
            {
                if(building.Name == gameLoader.saveSettingAndMenuConfig.unlockedTowerUpgradesList[j])
                {
                    building.isUnlocked = true;
                }
            }
        }
        PlayerStatus = FindObjectOfType<PlayerStatus>();
        AcceptButtonGameObject.SetActive(false);
        Buttons.SetActive(false);
        SpawnTrasform = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        
    }
    [Serializable]
    public class BuildingSetup
    {
        public string Name = null;
        public string Desc = null;
        public float Price = 0f;
        public string Button1 = null;
        public string Button2 = null;
        public string Button3 = null;
        public string Button4 = null;
        public GameObject buildingModel = null;
        public bool isUnlocked = false;
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject()& !FlagPosChoose)
            {
                if (MenuIsActive)
                {
                    Animator.Play("ButtonsOff");
                    MenuIsActive = false;
                    WhatToBuild = null;
                    if (circleRenderer != null)
                    {
                        circleRenderer.enabled = false;
                    }
                }
            }
        }
    }
    public void ButtonScript()
    {
        TextButton1.text = null;
        TextButton2.text = null;
        TextButton3.text = null;
        TextButton4.text = null;
        if (MenuIsActive)
        {
            Animator.Play("ButtonsOff");
            MenuIsActive = false;
            WhatToBuild = null;
            if (circleRenderer != null)
            {
                circleRenderer.enabled = false;
            }
        }
        else
        {
            TextName.text = null;
            TextDesc.text = null;
            TextPrice.text = null;
            AcceptButtonGameObject.SetActive(false);
            Animator.Play("ButtonsOn");
            MenuIsActive = true;

            FindBuildingSetup(BuiltAlready, true);

            if (BuiltAlready!= null)
            {
                SellButtonGameObject.SetActive(true);
            }
        }
    }
    public void Button1()
    {
        FindBuildingSetup(buildingSetup.Button1, false);
    }
    public void Button2()
    {
        FindBuildingSetup(buildingSetup.Button2, false);
    }
    public void ShowAcceptButton()
    {
        if(buttonBuildingSetup.Name == "Zablokowana"||buttonBuildingSetup.Name == "-1")
        {
            AcceptButtonGameObject.SetActive(false);
            return;
        }
        AcceptButtonGameObject.SetActive(true);
    }
    public void Button3()
    {
        if (BuiltAlready == "Koszary")
        {
            TextButton3.text = "Flaga";
            if (FlagPosChoose)
            {
                TextButton3.text = "Flaga";
                FlagPosChoose = false;
                Building.GetComponent<WarriorsBase>().FlagPosChoose = FlagPosChoose;
                Building.GetComponent<WarriorsBase>().FlagButTransparent.SetActive(false);
                return;
            }
            else
            {
                if (circleRenderer != null)
                {
                    circleRenderer.enabled = false;
                }
                AcceptButtonGameObject.SetActive(false);
                FlagPosChoose = true;
                Building.GetComponent<WarriorsBase>().FlagButTransparent.SetActive(true);
                Building.GetComponent<WarriorsBase>().FlagPosChoose = FlagPosChoose;
                Building.GetComponent<WarriorsBase>().InvokeRepeating("ShowItWithMouse", 0f, 0.01f);
                TextName.text = "Flaga";
                TextDesc.text = "Flaga. W tym miejscu bedą sie zbierać żołnierze";
                TowerPrice = 0;
                //TextPrice.text = ("Darmowe");
                TextButton3.text = "Anuluj";
                return;
            }
        }
        else
        {
            FindBuildingSetup(buildingSetup.Button3, false);
        }
    }
    public void Button4()
    {
        FindBuildingSetup(buildingSetup.Button4, false);
    }
    public void SellButton()
    {
        WhatToBuild = "Sprzedaj";
        FindBuildingSetup(WhatToBuild,false);
    }
    public void AcceptButton()
    {
        Accept();
    }
    
    public void Accept()
    {
        TMP_Text AcceptText;
        AcceptText = GameObject.Find("accept").GetComponent<TextMeshProUGUI>();
        if (PlayerStatus.Money < TowerPrice)
        {
            AcceptText.color = new Color32(255, 0, 0, 255);
            Debug.Log("Nie masz wystarczająco pengi");
            return;
        }
        AcceptText.color = new Color32(63, 192, 0, 255);

        if (WhatToBuild == "Sprzedaj")
        {
            Destroy(Building);
            BuiltAlready = null;
            PlayerStatus.Money += TowerPrice;
            ActualTowerPrice = 0f;
        }
        else
        {
            Building = Instantiate(buttonBuildingSetup.buildingModel, SpawnTrasform, transform.rotation);
            if(WhatToBuild == "Koszary")
            {
                Building.GetComponent<WarriorsBase>().BuildingScript = this;
            }
            PlayerStatus.Money -= TowerPrice;
            BuiltAlready = WhatToBuild;
            ActualTowerPrice = TowerPrice;
        }

        WhatToBuild = null;
        MenuIsActive = false;
        Animator.Play("Empty");
        AcceptButtonGameObject.SetActive(false);    
        //Building.transform.parent = gameObject.transform;

    }
    public void FlagPlaced()
    {
        Button3();
        FlagPosChoose = false;
    }

    void FindBuildingSetup(string buildingName, bool isForButtonBuildingSetup)
    {
        BuildingSetup selectedBuildingSetup = new BuildingSetup();
        if(buildingName == "-1")
        {
            TextName.text = null;
            TextDesc.text = null;
            TowerPrice = 0f;
            TextPrice.text = ("$" + TowerPrice.ToString());
            AcceptButtonGameObject.SetActive(false);
            return;
        }
        if (buildingName == null)
        {
            selectedBuildingSetup.Button1 = "Ciężka";
            selectedBuildingSetup.Button2 = "Lekka";
            selectedBuildingSetup.Button3 = "Koszary";
            selectedBuildingSetup.Button4 = "Magiczna";
            SellButtonGameObject.SetActive(false);
        }
        else
        {
            SellButtonGameObject.SetActive(true);
            for (int i = 0; i < buildingsList.Count; i++)
            {
                BuildingSetup buildingFinding = buildingsList[i];
                if (buildingFinding.Name == buildingName)
                {
                    selectedBuildingSetup = buildingFinding;
                    i = buildingsList.Count - 1;
                }
            }
            if (!selectedBuildingSetup.isUnlocked)
            {
                selectedBuildingSetup = buildingsList[6];
            }
        }
        if (buildingName == "Sprzedaj")
        {

            TextName.text = "Sprzedaj";
            TextDesc.text = "Sprzedaj aktualny budynek i otrzymaj 75% jej wartości.";
            TowerPrice = ActualTowerPrice * 0.75f;
            TextPrice.text = ("$" + TowerPrice.ToString());
            ShowAcceptButton();
            return;
        }
        else
        {
            if(BuiltAlready == null)
            {
                SellButtonGameObject.SetActive(false);
            }
            else
            {
                SellButtonGameObject.SetActive(true);
            }
        }

        if (isForButtonBuildingSetup)
        {
            buildingSetup = selectedBuildingSetup;
            TextButton1.text = buildingSetup.Button1;
            TextButton2.text = buildingSetup.Button2;
            TextButton3.text = buildingSetup.Button3;
            TextButton4.text = buildingSetup.Button4;
            
            if (buildingName != null)
            {
                for (int i = 0; i < buildingsList.Count; i++)
                {
                    BuildingSetup buildingFinding = buildingsList[i];
                    Debug.Log(buildingFinding.Name);

                    if (buildingFinding.Name == selectedBuildingSetup.Button1)
                    {
                        if (!buildingFinding.isUnlocked)
                        {
                            TextButton1.text = "Zablokowany";
                        }
                    }
                    else if (buildingFinding.Name == selectedBuildingSetup.Button2)
                    {
                        if (!buildingFinding.isUnlocked)
                        {
                            TextButton2.text = "Zablokowany";
                        }
                    }
                    else if (buildingFinding.Name == selectedBuildingSetup.Button3)
                    {
                        if (!buildingFinding.isUnlocked)
                        {
                            TextButton3.text = "Zablokowany";
                        }
                    }
                    else if (buildingFinding.Name == selectedBuildingSetup.Button4)
                    {
                        if (!buildingFinding.isUnlocked)
                        {
                            TextButton4.text = "Zablokowany";
                        }
                    }
                    if(selectedBuildingSetup.Button1 == "-1")
                    {
                        TextButton1.text = null;
                    }
                    if (selectedBuildingSetup.Button2 == "-1")
                    {
                        TextButton2.text = null;
                    }
                    if (selectedBuildingSetup.Button3 == "-1")
                    {
                        TextButton3.text = null;
                    }
                    if (selectedBuildingSetup.Button4 == "-1")
                    {
                        TextButton4.text = null;
                    }
                }
            }
        }
        else
        {
            buttonBuildingSetup = selectedBuildingSetup;
            ShowAcceptButton();

            WhatToBuild = buttonBuildingSetup.Name;
            TextName.text = buttonBuildingSetup.Name;
            TextDesc.text = buttonBuildingSetup.Desc;
            TowerPrice = buttonBuildingSetup.Price;
            TextPrice.text = ("$" + TowerPrice.ToString());

        }
    }
    
}
