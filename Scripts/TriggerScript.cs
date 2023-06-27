using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScript : MonoBehaviour
{
    public bool cooldownIsDown = true;

    public TowerShootTarget TowerScript;
    EnemyScript EnemyScript;
    public Animator Animator;
    public string AnimationName;
    public float AnimationLenght;
    float cooldown;
    // Shell variables
    public GameObject ejector;
    public Rigidbody bulletShell;
    public float ejectForce;
    public bool canEject = false;

    public ParticleSystem ShootParticle;
    void Start()
    {
        cooldown = TowerScript.cooldown;
    }
     void Update()
    {
        if (canEject)
        {
            canEject = false;
            Eject();
        }
        cooldown -= Time.deltaTime;
    }
    void OnTriggerStay (Collider collider)
    {
        if (TowerScript.GameObjectTarget == null)
        {
            Debug.Log("Nie ma targetu");
            return;
        }
        if (collider.gameObject == TowerScript.GameObjectTarget && cooldown <= 0)
        {
            ShootParticle.Play();
            cooldown = TowerScript.cooldown;
            cooldown -= AnimationLenght;
            EnemyScript = TowerScript.GameObjectTarget.GetComponent<EnemyScript>();
            EnemyScript.health -= TowerScript.damage;
            Eject();
            Animator.Play(AnimationName);
            gameObject.SetActive(false);

        }
    }
    // Shell code
    public void Eject()
    {
        Rigidbody clone = Instantiate(bulletShell, ejector.transform.position, ejector.transform.rotation) as Rigidbody;
        clone.velocity = transform.right * ejectForce;
    }
}
