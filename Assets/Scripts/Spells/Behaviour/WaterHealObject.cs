using UnityEngine;

public class WaterHealObject : MonoBehaviour
{
	[SerializeField] WaterHeal spellData;

	private float deleteTime;

	private void Awake()
	{
		GameObject player = GameObject.FindWithTag("Player");
		if (player != null)
		{
			PlayerStats playerStats = player.GetComponent<PlayerStats>();

			playerStats.CurrentHP += spellData.CalculateDamagePerInstance();
			deleteTime = Time.time + spellData.duration;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	// Start is called before the first frame update
	void Start()
	{

	}

	private void Update()
	{
		if (Time.time > deleteTime)
		{
			Destroy(gameObject);
		}
	}
}
