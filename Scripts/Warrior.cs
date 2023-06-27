using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class Warrior : MonoBehaviour
{
    [Header("WarriorStats")]
    public float speed = 5f;
    public float MaxHealth = 50f;
    public float health = 50f;
    public float regen = 5f;
    public float def = 5f;
    public float magicDef = 5f;
    public float damage = 1f;
    public float attackSpeed = 0.5f;

    // Zrobiæ, ¿eby od pos number zalezalo miejsce w szyku. Dostaja je od warrior base przy spawnie
    public int posNumber;
    NavMeshAgent NavMeshAgent;
    EnemyScript enemyScript = null;
    public WarriorsBase WarriorsBase;
    bool fighting = false;
    bool CanFight = false;
    public Transform target = null;
    public float range  = 15f;
    float LerpPercentOfExit = 0f;
    public Transform Exit;
    public Vector3 targetPos;
    Transform startPos;
    Transform startPosOfExit;
    private Slider HpSlider;
    bool Wyszed³ = false;
    void Start()
    {
        HpSlider = GetComponent<Slider>();
        HpSlider.maxValue = MaxHealth;
        NavMeshAgent = GetComponent<NavMeshAgent>();
        NavMeshAgent.enabled = false;
        startPosOfExit = transform;
        InvokeRepeating("UpdateTarget", 0f, Random.Range(0.51f, 0.61f));
        InvokeRepeating("UpdateFlag", 0f, 0.51f);
        
    }


    void Update()
    {
      
        HpSlider.value = health;
        if (health <= 0)
        {
            Dead();
        }
        if (!Wyszed³)
        {
            
            Vector3 ExitVector = Exit.transform.position - new Vector3(0, 1f, 0);
            transform.position = Vector3.Lerp(startPosOfExit.position, ExitVector, LerpPercentOfExit);
            LerpPercentOfExit += 1 * Time.deltaTime;
            if (LerpPercentOfExit > 1)
            {
                Wyszed³ = true;
                NavMeshAgent.enabled = true;
            }
            return;
        }
        else
        {
            CanFight = true;
        }
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float enemiesInRange = 0;
        float enemiesInRangeAndFighting = 0;

        GameObject EnemyTarget;
        float shortestDistance = Mathf.Infinity;
        float smalestPriority = Mathf.Infinity;
        float smalestPriorityOfEnemyFighting = Mathf.Infinity;
        float distanceToEnemy = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            //Im mniejsze priority tym jest wa¿niejszy
            EnemyScript searchingEnemyScript;
            distanceToEnemy = Vector3.Distance(WarriorsBase.Flag.transform.position, enemy.transform.position);
            bool isEnemyFighting = enemy.GetComponent<EnemyScript>().fighting;
            searchingEnemyScript = enemy.GetComponent<EnemyScript>();
            //Wrogowie niewalcz¹cy
            float priorityEnemy = 0;
            //Wrogowie walcz¹cy
            float priorityOffFightingEnemy = 0;

            if (distanceToEnemy <= range)
            {
                //Debug.Log("Oponent w zasiêgu");
                if (searchingEnemyScript.fighting == false)
                {
                    enemiesInRange += 1;
                    priorityEnemy = searchingEnemyScript.priority;

                }
                else
                {
                    enemiesInRangeAndFighting++;
                    priorityOffFightingEnemy = searchingEnemyScript.priority;

                }



                if (enemiesInRange + enemiesInRangeAndFighting == 1)
                {
                    // ustawia pierwsz¹ wartoœæ
                    if (target != null)
                    {
                        nearestEnemy = target.gameObject;
                    }
                    else
                    {
                        nearestEnemy = enemy;
                    }

                }

                if (enemiesInRange < 1)
                {
                    // chyu
                    // && EnemyScript != null
                    if (enemiesInRangeAndFighting != 1 && searchingEnemyScript.howManyWarriors < (nearestEnemy.GetComponent<EnemyScript>().howManyWarriors - 1))
                    {
                        //Debug.Log("ustawia tych walczacych. Mój numer to " + posNumber);
                        //je¿eli ma mniej walcz¹cych oponentów to walczy (chyba o to w tym chodzi)
                        smalestPriorityOfEnemyFighting = priorityEnemy;
                        nearestEnemy = enemy;

                    }
                }
                else
                {
                    if (priorityEnemy < smalestPriority && enemiesInRange != 1 || nearestEnemy.GetComponent<EnemyScript>().fighting == true)
                    {
                        smalestPriority = priorityEnemy;
                        nearestEnemy = enemy;
                        //Debug.Log("ustawia sie niewalczacych");


                    }
                }
            }
        }
        if (nearestEnemy != null && distanceToEnemy <= range && target != nearestEnemy.transform)
        {
            if (target != null)
            {
                enemyScript.howManyWarriors--;
            }
            //target.GetComponent<EnemyScript>().howManyWarriors--;
            target = nearestEnemy.transform;
            enemyScript = target.GetComponent<EnemyScript>();
            enemyScript.howManyWarriors+=1;
        }
    }
    void UpdateFlag()
    {
        //UPDATE FLAG POSITION |
        //                     V
        if (Wyszed³)
        {
            if (target == null)
            {
                if ((WarriorsBase.Flag.transform.position - transform.position).magnitude <= 0.5f)
                {
                    NavMeshAgent.ResetPath();
                    return;
                }

                //targetPos = WarriorsBase.Flag.transform.position - new Vector3(0, 1f, 0);
                //Debug.Log(posNumber);
                NavMeshAgent.destination = WarriorsBase.FlagPositions[posNumber];
            }
            else
            {
                NavMeshAgent.destination = target.transform.position;
                if ((target.transform.position - transform.position).magnitude <= 3f)
                {
                    Fight();
                    NavMeshAgent.ResetPath();
                    return;
                }
            }

        }
        
    }
    void Fight()
    {
        if(enemyScript.WarriorScript==false)
        {
            enemyScript.fighting = true;
            fighting = true;
            enemyScript.WarriorScript = this;
        }
        else
        {
            fighting = true;
        }

        if (target == null)
        {
            fighting = false;
            CancelInvoke("DealDamage");
            return;
        }
        InvokeRepeating("DealDamage", 0f, attackSpeed);

    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
    void DealDamage()
    {
        if (enemyScript.WarriorScript == false)
        {
            enemyScript.fighting = true;
            fighting = true;
            enemyScript.WarriorScript = this;
        }
        else
        {
            fighting = true;
        }
        if (target == null)
        {
            CancelInvoke("DealDamage");
            fighting = false;
            return;
        }
        enemyScript.health -= damage;
        Debug.Log("Atakuje");

    }
    public void Dead()
    {
        enemyScript.howManyWarriors--;
        WarriorsBase.HowManyWarriorsAreDead.Add(posNumber);
        StopAllCoroutines();
        Destroy(gameObject);
    }
}
