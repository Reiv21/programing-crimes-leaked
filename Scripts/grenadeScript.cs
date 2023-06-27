using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grenadeScript : MonoBehaviour
{
    float range = 10f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(boom());
    }

    IEnumerator boom()
    {
        yield return new WaitForSeconds(0.15f);
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float distanceToEnemy;
        foreach (GameObject enemy in enemies)
        {

            distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            float priorityEnemy = enemy.GetComponent<EnemyScript>().priority;
            if (distanceToEnemy <= range)
            {
                dealDamage(enemy, distanceToEnemy);
            }
        }
        yield return new WaitForSeconds(0.75f);
        Destroy(gameObject);
        yield return null;
    }
    void dealDamage(GameObject enemy, float range)
    {
        enemy.GetComponent<EnemyScript>().health -= 20f / (range * 0.5f);
    }
}
