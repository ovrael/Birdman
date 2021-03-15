using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardSpell : MonoBehaviour
{
	[SerializeField] SpellData spellStats;

	private Rigidbody2D rb;
	public float speed = 150f;

	// Start is called before the first frame update
	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		//transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
	}

	private void Start()
	{
		rb.velocity = transform.up * speed;
	}

	void OnCollisionEnter2D(Collision2D collider)
	{
		if (collider.gameObject.CompareTag("Enemy"))
		{
			collider.gameObject.GetComponent<EnemyStats>().TakeDamage(spellStats.CalculateDamagePerInstance());
			Destroy(gameObject);
		}

		if (collider.gameObject.CompareTag("Wall"))
		{
			Destroy(gameObject);
		}
	}
}
