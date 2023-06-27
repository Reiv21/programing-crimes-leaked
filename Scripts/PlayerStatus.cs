using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerStatus : MonoBehaviour
{
    public TMP_Text MoneyText;
    public TMP_Text FPSText;
    public TMP_Text healthText;
    public float Money = 0f;
    public float StartMoney;
    public float health;
    int avgFrameRate;
    bool GameIsPaused = false;
    public GameObject PauseMenu;
    public GameObject PauseButton;
    public GameObject LoadingScreen;
    public GameObject ExitMenu;
    public GameObject BackMenu;
    public GameObject loseMenu;
    public GameObject winMenu;
    public Image LoadingBarFill;
    public TMP_Text waveText;
    public HeroConfig heroConfig;
    public SaveSettingAndMenuConfig saveSettingAndMenuConfig;
    GameLoader gameLoader;
    WaveSpawner waveSpawner;
    void Start()
    {
        gameLoader = GetComponent<GameLoader>();
        heroConfig.SpawnHero();
        // wy³¹czone do testów
        //gameLoader.LoadFromFile(false);

        saveSettingAndMenuConfig = gameLoader.saveSettingAndMenuConfig;
        waveSpawner = gameObject.GetComponent<WaveSpawner>();
        waveText.text = waveSpawner.waveNumber + "/" + waveSpawner.maxWaveNumber;
        InvokeRepeating("UpdatePlayerStatus", 0f, 0.5f);
        Money = StartMoney;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space))
        {
            Pause();
        }
        int current;
        current = (int)(1f / Time.unscaledDeltaTime);
        avgFrameRate = (int)current;
        
        FPSText.text = "FPS = " + avgFrameRate;

        
    }
    void UpdatePlayerStatus()
    {
        MoneyText.text = "Bud¿et $" + Money;
        waveText.text = waveSpawner.waveNumber + "/" + waveSpawner.maxWaveNumber;
        healthText.text = "¯ycie " + health;
    }
    public void BackToMenu()
    {
        BackMenu.SetActive(true);
    }
    public void BackMenuYesButton()
    {
        StartCoroutine(LoadSceneAsync(0));
    }
    public void BackMenuNoButton()
    {
        BackMenu.SetActive(false);
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
    public void ExitButton()
    {
        ExitMenu.SetActive(true);
    }
    public void ExitYesButton()
    {
        Application.Quit();
    }
    public void ExitNoButton()
    {
        ExitMenu.SetActive(false);
    }
    public void Pause()
    {
        if(GameIsPaused)
        {
            //W³¹cz gre
            Time.timeScale = 1;
            PauseMenu.SetActive(false);
            PauseButton.GetComponentInChildren<TMP_Text>().text="Pauza";
            GameIsPaused = false;
        }
        else
        {
            //Wy³¹cz gre
            Time.timeScale = 0;
            PauseMenu.SetActive(true);
            PauseButton.GetComponentInChildren<TMP_Text>().text = "Wznów";
            GameIsPaused = true;
        }
    }

    public void lowerHealth(float amouth)
    {
        health -= amouth;
        if (health <= 0)
        {
            loseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void ReloadScene()
    {
        StartCoroutine(LoadSceneAsync(SceneManager.GetActiveScene().buildIndex));
    }

    public void WonLevel()
    {
        winMenu.SetActive(true);
        Time.timeScale = 0f;
    }


}
