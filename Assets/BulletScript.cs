using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    GameObject target;
    public float speed;
    public float damage;
    public bool facingRight = false;
    Rigidbody2D bulletRB;

    public BulletScript(float damage)
    {
        this.damage = damage;
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(0, 0, 90);
        bulletRB = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
        if (target.transform.position.x > gameObject.transform.position.x && facingRight) Flip();
        if (target.transform.position.x < gameObject.transform.position.x && !facingRight) Flip();
        Vector2 moveDir = (target.transform.position - transform.position).normalized * speed;
        bulletRB.velocity = new Vector2(moveDir.x, moveDir.y);
        Destroy(this.gameObject, 5);
    }

    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            collider.transform.GetComponentInChildren<PlayerStats>().TakeDamage(damage);
            Destroy(gameObject);
        }

        if (collider.gameObject.CompareTag("Terrain"))
        {
            Destroy(gameObject);
        }
    }
    void Flip()
    {
        transform.Rotate(0, 0, 180);
    }
}
