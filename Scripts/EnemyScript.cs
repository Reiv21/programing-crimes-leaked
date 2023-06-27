using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    [Header("EnemyStats")]
    public float speed = 1f;
    public float MaxHealth = 5f;
    public float health;
    public float regen = 5f;
    public float def = 5f;
    public float magicDef = 5f;
    public float damage = 5f;
    public float hearth = 5f;
    public float attackSpeed = 0.5f;
    public float moneyFromDeath = 5f;
    public float healthDealedWhenEnd = 1f;
    public bool dead = false;
    public int howManyWarriors = 0;
    //Kiedyœ mo¿e zrobie liste z tym kogo po koleji ma biæ
    //public GameObject[] WarriorsAttacking;
    public Warrior WarriorScript;
    PlayerStatus playerStatus;
    
    [Header("Debug")]
    public bool fighting = false;
    public float priority = 0f;

    private Transform Target;
    private int wavepointIndex = 0;
    private Slider HpSlider;

    void Start()
    {
        playerStatus = GameObject.FindWithTag("GameMaster").GetComponent<PlayerStatus>();
        health = MaxHealth;
        HpSlider = GetComponent<Slider>();
        HpSlider.maxValue = health;
        Target = Waypoints.points[0];
    }

    void Update()
    {
        HpSlider.value = health;
        if (dead || health <= 0)
        {
            Dead(); ;
        }
        if (fighting)
        {
            Fight();
            return;
        }

        priority -= speed * Time.deltaTime;
        Vector3 dir = Target.position - transform.position;
        transform.Translate(speed * Time.deltaTime * dir.normalized, Space.World);
        
        
        if (Vector3.Distance(transform.position, Target.position) <= 0.4f)
        {
            GetNextWayPoint();
        }
    }
    void GetNextWayPoint()
    {
        if (wavepointIndex >= Waypoints.points.Length - 1)
        {
            playerStatus.lowerHealth(healthDealedWhenEnd);
            Destroy(gameObject);
            return;
        }

        wavepointIndex++;
        Target = Waypoints.points[wavepointIndex];
        var rotationAngle = Quaternion.LookRotation(Target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationAngle, 1);
        //transform.LookAt(Target);
    }
    void Fight()
    {
        if (WarriorScript == null)
        {
            fighting = false;
            CancelInvoke("DealDamage");
            return;
        }
        InvokeRepeating("DealDamage", 0f, attackSpeed);

    }
    void DealDamage()
    {
        if (WarriorScript == null)
        {
            CancelInvoke("DealDamage");
            fighting = false;
            return;
        }
        WarriorScript.health -= damage;
        Debug.Log("Atakuje Obroñce");

    }
    void Dead()
    {
        playerStatus.Money += moneyFromDeath;
        Destroy(gameObject);
    }
}
