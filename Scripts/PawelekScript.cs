using System.Collections;
using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class PawelekScript : MonoBehaviour
{
    Vector3 pos;
    Camera camera;
    public LineRenderer circleRenderer;
    public GameObject whereHeroIsGoing;
    public GameObject spell1Button;
    public GameObject spell2Button;
    public GameObject heroImageAndButton;
    public GameObject heroPanel;
    public HeroConfig.Hero hero;
    public HeroConfig heroConfig;
    public NavMeshAgent navMeshAgent;
    public Animator animator;
    //
    public Vector3 targetPos;
    public Transform target = null;
    EnemyScript enemyScript = null;

    //Hero stats
    float healthMax = 1000f;
    float health;
    float healthRegenTimeInOneSec = 1f;
    float movmentSpeed = 3f;
    float attackDamage = 3f;
    float attackSpeed = 1.2f;
    bool isHeroMoving = false;
    // czas prze³adowania (animacji)
    float reloadTime = 2.15f;
    int maxAmmoInMag = 16;
    public int ammoInMag = 0;
    float range = 10f;
    public int TargetChooseOption = 1;
    public bool canShoot = true;
    bool isShooting = false;

    //Spell1
    public GameObject grenadePrefab;
    bool isSpell1ChoosingActive = false;
    GameObject spell1Frame;
    GameObject spell2Frame;
    Vector3 granadePos;
    public Transform granadeSpawn;
    float granadeRange = 5f;

    //Spell2
    float boostFromSpell1;

    // Start is called before the first frame update
    void Start()
    {
        DrawCircle(100);
        circleRenderer.enabled= false;
        spell1Frame = GameObject.Find("spell1Frame");
        spell2Frame = GameObject.Find("spell2Frame");
        ammoInMag = maxAmmoInMag;
        navMeshAgent.speed = movmentSpeed;
        gameObject.transform.localPosition = new Vector3(0f, 1f, 0f);
        whereHeroIsGoing = GameObject.Find("WhereHeroIsGoing");
        whereHeroIsGoing.SetActive(false);
        //Jak wersja na tel to phone camera
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        //navMeshAgent.
        heroPanel = GameObject.Find("HeroPanel");
        heroConfig = GameObject.Find("UI AND GAME MANAGER").GetComponent<HeroConfig>();
        hero = heroConfig.activeHero;
        spell1Button = GameObject.Find("Skill1Button");
        spell2Button = GameObject.Find("Skill2Button");
        heroImageAndButton = GameObject.Find("HeroImage");
        heroImageAndButton.GetComponent<Button>().onClick.AddListener(delegate { MoveHero(); });
        spell1Button.GetComponent<Button>().onClick.AddListener(delegate { Spell1(); });
        spell2Button.GetComponent<Button>().onClick.AddListener(delegate { Spell2(); });

        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        // https://www.reddit.com/r/Unity3D/comments/m0wkw9/how_to_join_the_navmesh_baked_into_prefab/ 
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 100f;
        mousePos = camera.ScreenToWorldPoint(mousePos);
        Debug.DrawRay(camera.transform.position, mousePos - camera.transform.position, Color.blue);

        if (Input.GetMouseButtonDown(0) && isHeroMoving)
        {
            MoveHero();
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 500))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    MoveHero();
                    return;
                }
                if (hit.collider != null)
                {
                    pos = hit.point;
                    InvokeRepeating("UpdateHeroPos",0,0.5f);
                }
            }
            else
            {
                MoveHero();
            }
        }
        if (Input.GetMouseButtonDown(0) && isSpell1ChoosingActive)
        {
            //MoveHero();
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 500))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    isSpell1ChoosingActive = false;
                    spell1Frame.GetComponent<Image>().color = new Color32(68, 221, 191, 255);
                    return;
                }
                if (hit.collider != null)
                {
                    granadePos = hit.point;
                    isSpell1ChoosingActive = false;
                    spell1Frame.GetComponent<Image>().color = new Color32(68, 221, 191, 255);
                    //InvokeRepeating("UpdateHeroPos", 0, 0.5f);

                    //Rzucanie granatem
                    GameObject granade = Instantiate(grenadePrefab);
                    granade.transform.position = hit.point - new Vector3(0,0.5f,0);
                }
            }
            else
            {
                isSpell1ChoosingActive = false;
                spell1Frame.GetComponent<Image>().color = new Color32(68, 221, 191, 255);
            }
        }
    }
    public void Spell1()
    {
        if (isSpell1ChoosingActive)
        {
            circleRenderer.enabled = false;
            isSpell1ChoosingActive = false;
            spell1Frame.GetComponent<Image>().color = new Color32(68, 221, 191, 255);
            return;
        }
        circleRenderer.enabled = true;
        isSpell1ChoosingActive = true;
        spell1Frame.GetComponent<Image>().color = Color.yellow;
    }
    public void Spell2()
    {

    }
    public IEnumerator DealDamage()
    {
        Debug.Log("Strzelam");
        isShooting = true;
        for (int nonelol; ammoInMag >= 1 && canShoot; ammoInMag--)
        {
            if (target == null)
            {
                Debug.Log("target null");
                isShooting = false;
                break;
            }
            Debug.Log("Odejmuje hp");
            enemyScript.health -= attackDamage;
            yield return new WaitForSeconds(attackSpeed);
        }
        if (ammoInMag < 1)
        {
            Reload();
            yield return new WaitForSeconds(reloadTime + attackSpeed);
            StartCoroutine(DealDamage());
        }
        yield return null;
    }
    public void Reload()
    {
        canShoot = false;
        animator.Play("ReloadPawe³ek");
        ammoInMag = maxAmmoInMag;
    }
    public void MoveHero()
    {
        if (isHeroMoving)
        {
            isHeroMoving = false;
            heroPanel.GetComponent<Image>().color = new Color32(68, 221, 191, 255);
            whereHeroIsGoing.SetActive(false);
        }
        else
        {
            isHeroMoving = true;
            heroPanel.GetComponent<Image>().color = Color.yellow; //new Color32(172, 255, 143, 255);
            whereHeroIsGoing.SetActive(true);
        }
    }
    public void UpdateHeroPos()
    {
        whereHeroIsGoing.transform.position = new Vector3(pos.x, 0.65f, pos.z);
        if ((pos - transform.position).magnitude <= 0.5f)
        {
            navMeshAgent.ResetPath();
            return;
        }
        navMeshAgent.destination = pos; 
    }
    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float distanceToEnemy;
        float enemiesInRange = 0;
        float biggestPriority = Mathf.Infinity;
        float healthEnemy = Mathf.Infinity;
        float biggestHealth = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {

            distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            float priorityEnemy = enemy.GetComponent<EnemyScript>().priority;
            if (distanceToEnemy <= range)
            {
                enemiesInRange += 1;
            }
            if (priorityEnemy < biggestPriority && distanceToEnemy <= range && TargetChooseOption == 1)
            {
                biggestPriority = priorityEnemy;
                nearestEnemy = enemy;
                enemyScript = enemy.GetComponent<EnemyScript>();

            }
            if (healthEnemy < biggestHealth && distanceToEnemy <= range && TargetChooseOption == 2)
            {
                biggestPriority = priorityEnemy;
                nearestEnemy = enemy;
                enemyScript = enemy.GetComponent<EnemyScript>();
            }

        }
        if (nearestEnemy != null)
        {
            target = nearestEnemy.transform;
            enemyScript = target.GetComponent<EnemyScript>();
            if (target != null && !isShooting)
            {
                StartCoroutine(DealDamage());
            }
        }
    }
    void DrawCircle(int steps)
    {
        steps += 1;
        float radius = granadeRange / 3;
        circleRenderer.positionCount = steps + 1;
        for (int currentStep = 0; currentStep <= steps; currentStep++)
        {
            float circumferenceProgress = (float)currentStep / steps;
            float currentRadian = circumferenceProgress * 2 * Mathf.PI;
            float xScaled = Mathf.Cos(currentRadian);
            float yScaled = Mathf.Sin(currentRadian);
            float x = xScaled * radius;
            float y = yScaled * radius;

            Vector3 currentPosition = new Vector3(x, y, 0.65f);

            circleRenderer.SetPosition(currentStep, currentPosition);
            circleRenderer.enabled = false;
        }
    }
}
