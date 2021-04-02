using UnityEngine;

public class WaterHealObject : MonoBehaviour
{
	[SerializeField] WaterHeal spellStats;

	private void Start()
	{
		GameObject player = GameObject.FindWithTag("Player");
		if (player != null)
		{
			PlayerStats playerStats = player.GetComponentInChildren<PlayerStats>();

			playerStats.CurrentHP += spellStats.CalculateDamagePerInstance();
			Destroy(gameObject, spellStats.duration.CalculatedValue);
		}
	}
}
