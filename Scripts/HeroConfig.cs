using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class HeroConfig : MonoBehaviour
{
    public Hero activeHero;
    public GameObject heroGameobject;
    public List<Hero> heroList = new List<Hero>();
    public Transform spawnHeroTransform;
    public RawImage heroImage;
    public GameObject spell1Button;
    public GameObject spell2Button;

    [Serializable]
    public class Hero
    {
        public string name;
        public string description;
        public GameObject heroObj;
        public Texture heroImage;
        public Texture spellOneImg;
        public Texture spellTwoImg;
        public string spellOneName;
        public string spellTwoName;
        public string spellOneDesc;
        public string spellTwoDesc;
        public float spellOneCooldown;
        public float spellTwoCooldown;
    }
    public void SpawnHero()
    {
        
        activeHero = heroList[GetComponent<GameLoader>().saveSettingAndMenuConfig.heroId];
        heroGameobject = Instantiate(activeHero.heroObj,spawnHeroTransform);
        heroImage.texture = activeHero.heroImage;
        switch (GetComponent<GameLoader>().saveSettingAndMenuConfig.heroId)
        {
            case 0:
                heroGameobject.GetComponent<PawelekScript>().navMeshAgent = spawnHeroTransform.gameObject.GetComponent<NavMeshAgent>();
                break;
            case 1:
                //heroGameobject.GetComponent<PawelekScript>().navMeshAgent = spawnHeroTransform.gameObject.GetComponent<NavMeshAgent>();
                break;
            case 2:
                //heroGameobject.GetComponent<PawelekScript>().navMeshAgent = spawnHeroTransform.gameObject.GetComponent<NavMeshAgent>();
                break;
            case 3:
                //heroGameobject.GetComponent<PawelekScript>().navMeshAgent = spawnHeroTransform.gameObject.GetComponent<NavMeshAgent>();
                break;
            case 4:
                //heroGameobject.GetComponent<PawelekScript>().navMeshAgent = spawnHeroTransform.gameObject.GetComponent<NavMeshAgent>();
                break;
            case 5:
                //heroGameobject.GetComponent<PawelekScript>().navMeshAgent = spawnHeroTransform.gameObject.GetComponent<NavMeshAgent>();
                break;
            default:
                break;
        }
        spell1Button.GetComponent<RawImage>().texture = activeHero.spellOneImg;
        spell2Button.GetComponent<RawImage>().texture = activeHero.spellTwoImg;
    }

}
