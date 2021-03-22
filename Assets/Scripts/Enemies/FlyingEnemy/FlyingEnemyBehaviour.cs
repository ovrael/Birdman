using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FlyingEnemyBehaviour : MonoBehaviour
{

    #region Public Variables
    public float attackDistance; //Minimum distance for attack
    public float timer; //Timer for cooldown between attacks
    public Transform target;
    [HideInInspector] public bool inRange; //Check if  is in range
    public Collider2D targetCollider;
    public BoxCollider2D hitBox;
    public AIPath aiPath;
    #endregion

    #region Private Variables
    private float distance; //Store the distance b/w enemy and 
    private bool attackMode;
    private bool cooling; //Check if Enemy is cooling after attack
    private float intTimer;
    #endregion

    void Awake()
    {
        intTimer = timer; //Store the inital value of timer
    }

    void Update()
    {

        if (aiPath.desiredVelocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (aiPath.desiredVelocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        EnemyLogic();
    }

    void EnemyLogic()
    {
        distance = Vector2.Distance(transform.position, target.position);

        if (distance > attackDistance)
        {
            StopAttack();
        }
        else if (attackDistance >= distance && cooling == false)
        {
            if (hitBox.IsTouching(targetCollider))
            {
                //float damage = gameObject.GetComponent<EnemyStats>().Damage;
                target.gameObject.GetComponent<PlayerStats>().TakeDamage(100);
                Debug.LogWarning("damage jebany furrasie!");
            }
        }

        if (cooling)
        {
            Cooldown();
        }
    }


    void Cooldown()
    {
        timer -= Time.deltaTime;

        if (timer <= 0 && cooling && attackMode)
        {
            cooling = false;
            timer = intTimer;
        }
    }

    void StopAttack()
    {
        cooling = false;
        attackMode = false;
    }

    public void TriggerCooling()
    {
        cooling = true;
    }

}
