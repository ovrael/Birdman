using System;
using UnityEngine;

public enum NodeType
{
	FlatIncrease,
	FlatDecrease,
	PercentIncrease,
	PercentDecrease
}

public enum TargetStat
{
	Health = 0,
	HealthRegen = 1,
	Mana = 2,
	ManaRegen = 3,
	Armor = 4,
	SpellCooldown = 100,
	SpellManaCost = 101,
	SpellDamage = 102,
	FireDamage = 103,
	WaterDamage = 104,
	LightningDamage = 105,
}

[Serializable]
public struct Node
{
	public NodeType type;
	public TargetStat stat;
	[Tooltip("If NodeType is percent you should use range between 0 and 100, value = value% and then is calculated to fraction")]
	public float value;

	public Node(NodeType type, TargetStat stat, float value)
	{
		this.type = type;
		this.stat = stat;
		this.value = value;
	}
}

[CreateAssetMenu(fileName = "New passive node", menuName = "Passives/Node")]
public class PassiveNodeScript : ScriptableObject
{
	public int id;
	public string nodeName;
	public string description;
	public bool isPicked;

	public Sprite icon;

	public Node[] nodes;
}
