using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spells/Water Heal")]
public class WaterHeal : SpellData
{
	public override void CreateDescription()
	{
		// Debug.Log("Create new description for Water Heal");

		base.CreateDescription();
		StringBuilder info = new StringBuilder();

		info.Append("Heals ");
		info.Append(minDamagePerInstance.CalculatedValue.ToString("0.00"));
		info.Append(" health points.");

		createdDescription = info.ToString();
	}
}
