using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerShootTarget : MonoBehaviour
{
    [Header("TowerStats")]
    public float rotateSpeed = 0.1f;
    public float damage = 1f;
    public float attackSpeed = 0.5f;
    public float cooldown = 2f;
    public int TargetChooseOption = 1;
    private Coroutine LookCoroutine;
    EnemyScript EnemyScript;
    public GameObject GameObjectTarget = null;
    public Transform targetTransform = null;
    public float range = 15f;
    public GameObject Trigger;
    public bool EnemyTriggered;
    float distanceToEnemy;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
    }
    void StartRotating()
    {
        if (LookCoroutine != null || targetTransform == null)
        {
            StopCoroutine(LookCoroutine);
        }
        LookCoroutine = StartCoroutine(LookAt());
    }
    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
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
                EnemyScript = enemy.GetComponent<EnemyScript>();

            }
            if (healthEnemy < biggestHealth && distanceToEnemy <= range && TargetChooseOption == 2)
            {
                biggestPriority = priorityEnemy;
                nearestEnemy = enemy;
                EnemyScript = enemy.GetComponent<EnemyScript>();

            }

        }
        if (nearestEnemy != null)
        {
            GameObjectTarget = nearestEnemy;
            targetTransform = nearestEnemy.transform;
            StartRotating();
        }
    }

    private IEnumerator LookAt()
    {
        float distanceToEnemy = Vector3.Distance(transform.position, targetTransform.position);
        if (distanceToEnemy > range)
        {
            yield return null;
        }
        Quaternion lookRotation = Quaternion.LookRotation(targetTransform.position - transform.position);

        float time = 0;
        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time);

            time += Time.deltaTime * rotateSpeed;
            yield return null;
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
