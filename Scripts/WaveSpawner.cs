using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    [Header("Settings")]
    public GameObject enemyPrefab;
    public Transform spawnPoint;
    public Image SpawnerImageFill;
    public float timeBetweenWaves = 30f;
    private float countdown = 2f;
    public float timeToWave;
    private bool GameStarted = false;
    public int waveNumber = 0;
    public int maxWaveNumber;
    [Header("Wave Settings")]
    [Header("Countdown jest liczony ile DO KOÑCA rundy zosta³o.")]
    [Header("Czyli 55 pojawi siê kiedy zosta³o 55 do nowej fali.")]
    public List<Wave> spawnOfMobsList = new List<Wave>();

    //[Header("Wave Manager")]
    //public string[] ListOfWaves = new string[];
    private void Start()
    {
        timeToWave = countdown;
        SpawnerImageFill.fillAmount = 0;
    }
    private void Update()
    {
        if (waveNumber == maxWaveNumber)
        {
            return;
        }

        if (timeToWave <= 0)
        {
            if (waveNumber > maxWaveNumber)
            {
                GetComponent<PlayerStatus>().WonLevel();
            }
            //Mo¿e zmieniæ na listê czas pomiêdzy falami?
            timeToWave = timeBetweenWaves;
            GameStarted = true;
            StartCoroutine(SpawnWave());
        }

        if (GameStarted)
        {
            SpawnerImageFill.fillAmount = timeToWave / timeBetweenWaves;
        }
        else
        {
            SpawnerImageFill.fillAmount = timeToWave / countdown;
        }
        timeToWave -= Time.deltaTime;

    }
    IEnumerator SpawnWave()
    {
        waveNumber++;
        for (int i = 0; i < spawnOfMobsList.Count; i++)
        {
            Wave checkingWaveMob = spawnOfMobsList[i];
            if(checkingWaveMob.waveNumber == waveNumber)
            {
                StartCoroutine(SpawnEnemy(checkingWaveMob));
                spawnOfMobsList.RemoveAt(i);
                i--;
            }
        }

        yield return null;

    }
    IEnumerator SpawnEnemy(Wave checkingWaveMob)
    {
        while (true) 
        {
            if (checkingWaveMob.countdown >= timeToWave)
            {
                for (int j = 0; j < checkingWaveMob.howMany; j++)
                {
                    Instantiate(checkingWaveMob.mobObj, spawnPoint.position, new Quaternion(0f, 0f, 0f, 0f));
                    yield return new WaitForSeconds(checkingWaveMob.cooldown);
                }
                break;
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
        yield return null;
    }


    [Serializable]
    public class Wave
    {
        public int waveNumber;
        public GameObject mobObj;
        public float countdown;
        public float cooldown;
        public int howMany;
    }

}
