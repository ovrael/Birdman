using System;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
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
	LightningDamage = 105
}

[Serializable]
public struct Node
{
	public NodeType type;
	public TargetStat targetStat;
	[Tooltip("If NodeType is percent you should use range between 0 and 100, value = value% and then is calculated to fraction")]
	public float value;

	public Node(NodeType type, TargetStat targetStat, float value)
	{
		this.type = type;
		this.targetStat = targetStat;
		this.value = value;
	}
}

[CreateAssetMenu(fileName = "New passive node", menuName = "Passives/Node")]
public class PassiveNodeScript : ScriptableObject
{
	public int id;
	public string nodeName;
	[Tooltip("Fill with \"GENERATE\" to auto-ganarate description.")]
	[TextArea] public string description;
	public bool isPicked;

	public Sprite icon;

	public Node[] nodes;

	void Awake()
	{
		if (nodes.Length > 0 && description.Trim() == "GENERATE")
			CreateDescription();
	}

	void CreateDescription()
	{
		StringBuilder descriptionBuilder = new StringBuilder();
		foreach (Node node in nodes)
		{
			descriptionBuilder.AppendLine(CreateNodeDescription(node));
		}

		description = descriptionBuilder.ToString().Trim();
	}

	string CreateNodeDescription(Node node)
	{
		StringBuilder nodeDescriptionBuilder = new StringBuilder();

		if (node.type == NodeType.FlatIncrease || node.type == NodeType.PercentIncrease)
		{
			nodeDescriptionBuilder.Append("Increase ");
		}
		else
		{
			nodeDescriptionBuilder.Append("Decrease ");
		}

		string targetEnum = TargetEnumToString(node.targetStat);
		nodeDescriptionBuilder.Append(targetEnum + ' ');
		nodeDescriptionBuilder.Append("by ");
		nodeDescriptionBuilder.Append(node.value);

		if (node.type == NodeType.PercentIncrease || node.type == NodeType.PercentDecrease)
		{
			nodeDescriptionBuilder.Append("%");
		}
		else
		{
			nodeDescriptionBuilder.Append(" points");
		}


		return nodeDescriptionBuilder.ToString();
	}

	string TargetEnumToString(TargetStat targetStat)
	{
		string targetEnum = targetStat.ToString();

		StringBuilder builder = new StringBuilder();
		foreach (char c in targetEnum)
		{
			if (char.IsUpper(c) && builder.Length > 0)
				builder.Append(' ');

			builder.Append(char.ToLower(c));
		}

		return builder.ToString();
	}
}
