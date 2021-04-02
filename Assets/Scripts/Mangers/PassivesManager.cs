using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct PassiveButton
{
	public Button button;
	public PassiveNodeScript node;
	public PassiveNodeScript[] requiredNodes;
}

public class PassivesManager : MonoBehaviour
{
	[Header("Player Data")]
	[SerializeField] PlayerStats playerStats;

	[Header("All spells")]
	[Tooltip("Contains the spells ScriptableObjects to which nodes will be applied")]
	public SpellData[] allSpells;

	[Header("Management")]
	[SerializeField] bool resetPassives;

	[Tooltip("Use it to reset all spells to 1. level")]
	[SerializeField] bool resetSpells;

	[Header("Buttons")]
	[SerializeField] PassiveButton[] passiveButtons;

	[Header("Node Look")]
	[SerializeField] Shader activeShader;
	[SerializeField] Shader inactiveShader;
	[SerializeField] Material defaultMaterial;

	public void CheckIfCanPickPassive(int index)
	{
		bool hasRequiredNode = false;

		foreach (PassiveNodeScript node in passiveButtons[index].requiredNodes)
		{
			if (node.isPicked)
			{
				hasRequiredNode = true;
				break;
			}
		}

		if (passiveButtons[index].requiredNodes.Length == 0)
			hasRequiredNode = true;

		if (playerStats.PassivePoints > 0 && hasRequiredNode)
		{
			ApplyPassiveNode(passiveButtons[index].node);
			ActivateNode(passiveButtons[index].button.image);
		}
		else
		{
			Debug.Log("Cant pick new passive node!");
		}
	}

	public void ApplyPassiveNode(PassiveNodeScript passiveNode)
	{
		bool error = false;
		foreach (Node node in passiveNode.nodes)
		{
			// Flat stat =  new Stat(0, node.value, 0);
			// Percent stat =  new Stat(0, 0, node.value);

			switch (node.type)
			{
				case NodeType.FlatIncrease:
					{
						switch (node.stat)
						{
							case TargetStat.Health:
								{
									playerStats.Health += new Stat(0, node.value, 0);
									break;
								}
							case TargetStat.HealthRegen:
								{
									playerStats.RegenHP += new Stat(0, node.value, 0);
									break;
								}
							case TargetStat.Mana:
								{
									playerStats.Mana += new Stat(0, node.value, 0);
									break;
								}
							case TargetStat.ManaRegen:
								{
									playerStats.RegenMP += new Stat(0, node.value, 0);
									break;
								}
							case TargetStat.Armor:
								{
									playerStats.Armor += new Stat(0, node.value, 0);
									break;
								}
							case TargetStat.SpellManaCost:
								{
									foreach (SpellData spell in allSpells)
									{
										spell.manaCost += new Stat(0, node.value, 0);
									}
									break;
								}
							case TargetStat.SpellDamage:
								{
									foreach (SpellData spell in allSpells)
									{
										spell.minDamagePerInstance += new Stat(0, node.value, 0);
										spell.maxDamagePerInstance += new Stat(0, node.value, 0);
									}
									break;
								}
							case TargetStat.FireDamage:
								{
									foreach (SpellData spell in allSpells)
									{
										if (spell.damageType == SpellDamageType.Fire)
										{
											spell.minDamagePerInstance += new Stat(0, node.value, 0);
											spell.maxDamagePerInstance += new Stat(0, node.value, 0);
										}
									}
									break;
								}
							case TargetStat.WaterDamage:
								{
									foreach (SpellData spell in allSpells)
									{
										if (spell.damageType == SpellDamageType.Water)
										{
											spell.minDamagePerInstance += new Stat(0, node.value, 0);
											spell.maxDamagePerInstance += new Stat(0, node.value, 0);
										}
									}
									break;
								}
							case TargetStat.LightningDamage:
								{
									foreach (SpellData spell in allSpells)
									{
										if (spell.damageType == SpellDamageType.Lightning)
										{
											spell.minDamagePerInstance += new Stat(0, node.value, 0);
											spell.maxDamagePerInstance += new Stat(0, node.value, 0);
										}
									}
									break;
								}
							default:
								{
									error = true;
									break;
								}
						}
						break;
					}
				case NodeType.FlatDecrease:
					{
						switch (node.stat)
						{
							case TargetStat.SpellManaCost:
								{
									foreach (SpellData spell in allSpells)
									{
										spell.manaCost -= new Stat(0, node.value, 0);
									}
									break;
								}
							default:
								{
									error = true;
									break;
								}
						}
						break;
					}
				case NodeType.PercentIncrease:
					{
						switch (node.stat)
						{
							case TargetStat.Health:
								{
									playerStats.Health += new Stat(0, 0, node.value);
									break;
								}
							case TargetStat.HealthRegen:
								{
									playerStats.RegenHP += new Stat(0, 0, node.value);
									break;
								}
							case TargetStat.Mana:
								{
									playerStats.Mana += new Stat(0, 0, node.value);
									break;
								}
							case TargetStat.ManaRegen:
								{
									playerStats.RegenMP += new Stat(0, 0, node.value);
									break;
								}
							case TargetStat.Armor:
								{
									playerStats.Armor += new Stat(0, 0, node.value);
									break;
								}
							case TargetStat.SpellCooldown:
								{
									foreach (SpellData spell in allSpells)
									{
										spell.cooldown -= new Stat(0, 0, node.value);   // We want to reduce cooldown so we subtract value
									}
									break;
								}
							case TargetStat.SpellManaCost:
								{
									foreach (SpellData spell in allSpells)
									{
										spell.manaCost -= new Stat(0, 0, node.value);   // We want to reduce manacost so we subtract value
									}
									break;
								}
							case TargetStat.SpellDamage:
								{
									foreach (SpellData spell in allSpells)
									{
										spell.minDamagePerInstance += new Stat(0, 0, node.value);
										spell.maxDamagePerInstance += new Stat(0, 0, node.value);
									}
									break;
								}
							case TargetStat.FireDamage:
								{
									foreach (SpellData spell in allSpells)
									{
										if (spell.damageType == SpellDamageType.Fire)
										{
											spell.minDamagePerInstance += new Stat(0, 0, node.value);
											spell.maxDamagePerInstance += new Stat(0, 0, node.value);
										}
									}
									break;
								}
							case TargetStat.WaterDamage:
								{
									foreach (SpellData spell in allSpells)
									{
										if (spell.damageType == SpellDamageType.Water)
										{
											spell.minDamagePerInstance += new Stat(0, 0, node.value);
											spell.maxDamagePerInstance += new Stat(0, 0, node.value);
										}
									}
									break;
								}
							case TargetStat.LightningDamage:
								{
									foreach (SpellData spell in allSpells)
									{
										if (spell.damageType == SpellDamageType.Lightning)
										{
											spell.minDamagePerInstance += new Stat(0, 0, node.value);
											spell.maxDamagePerInstance += new Stat(0, 0, node.value);
										}
									}
									break;
								}
							default:
								{
									error = true;
									break;
								}
						}
						break;
					}
				case NodeType.PercentDecrease:
					{
						switch (node.stat)
						{
							case TargetStat.Health:
								{
									playerStats.Health -= new Stat(0, 0, node.value);
									break;
								}
							case TargetStat.HealthRegen:
								{
									playerStats.RegenHP -= new Stat(0, 0, node.value);
									break;
								}
							case TargetStat.Mana:
								{
									playerStats.Mana -= new Stat(0, 0, node.value);
									break;
								}
							case TargetStat.ManaRegen:
								{
									playerStats.RegenMP -= new Stat(0, 0, node.value);
									break;
								}
							case TargetStat.Armor:
								{
									playerStats.Armor -= new Stat(0, 0, node.value);
									break;
								}
							case TargetStat.SpellCooldown:
								{
									foreach (SpellData spell in allSpells)
									{
										spell.cooldown -= new Stat(0, 0, node.value);   // We want to reduce cooldown so we subtract value
									}
									break;
								}
							case TargetStat.SpellManaCost:
								{
									foreach (SpellData spell in allSpells)
									{
										spell.manaCost -= new Stat(0, 0, node.value);   // We want to reduce manacost so we subtract value
									}
									break;
								}
							case TargetStat.SpellDamage:
								{
									foreach (SpellData spell in allSpells)
									{
										spell.minDamagePerInstance -= new Stat(0, 0, node.value);
										spell.maxDamagePerInstance -= new Stat(0, 0, node.value);
									}
									break;
								}
							case TargetStat.FireDamage:
								{
									foreach (SpellData spell in allSpells)
									{
										if (spell.damageType == SpellDamageType.Fire)
										{
											spell.minDamagePerInstance -= new Stat(0, 0, node.value);
											spell.maxDamagePerInstance -= new Stat(0, 0, node.value);
										}
									}
									break;
								}
							case TargetStat.WaterDamage:
								{
									foreach (SpellData spell in allSpells)
									{
										if (spell.damageType == SpellDamageType.Water)
										{
											spell.minDamagePerInstance -= new Stat(0, 0, node.value);
											spell.maxDamagePerInstance -= new Stat(0, 0, node.value);
										}
									}
									break;
								}
							case TargetStat.LightningDamage:
								{
									foreach (SpellData spell in allSpells)
									{
										if (spell.damageType == SpellDamageType.Lightning)
										{
											spell.minDamagePerInstance -= new Stat(0, 0, node.value);
											spell.maxDamagePerInstance -= new Stat(0, 0, node.value);
										}
									}
									break;
								}
							default:
								{
									error = true;
									break;
								}
						}
						break;
					}
				default:
					{
						error = true;
						break;
					}
			}
		}

		if (error)
		{
			Debug.LogError("Error node in: " + passiveNode.nodeName);
		}
		else
		{
			playerStats.PassiveIds.Add(passiveNode.id);
			playerStats.PassivePoints--;

			passiveNode.isPicked = true;
		}
	}

	public void ActivateNode(Image image)
	{
		Material material = image.material;
		material.shader = activeShader;
	}
	public void DeactivateNode(Image image)
	{
		Material material = image.material;
		material.shader = inactiveShader;
	}

	private void ResetPassives()
	{
		foreach (PassiveButton passiveButton in passiveButtons)
		{
			passiveButton.node.isPicked = false;
			DeactivateNode(passiveButton.button.image);
		}

		playerStats.ResetStats();
		playerStats.PassiveIds = new List<int>();

		foreach (SpellData spell in allSpells)
		{
			spell.ResetSpellPassives();
		}
	}

	private void ResetSpells()
	{
		foreach (SpellData spell in allSpells)
		{
			spell.ResetSpellLevel();
		}
		playerStats.SpellPoints = playerStats.Level;
	}


	private void Start()
	{
		for (int i = 0; i < passiveButtons.Length; i++)
		{
			int x = i;

			Image image = passiveButtons[x].button.GetComponent<Image>();
			image.sprite = passiveButtons[x].node.icon;

			Material newMaterial = new Material(defaultMaterial);
			newMaterial.SetTexture("_MainTex", image.mainTexture);

			image.material = newMaterial;

			if (!passiveButtons[x].node.isPicked)
				DeactivateNode(image);

			passiveButtons[x].button.onClick.AddListener(() => { CheckIfCanPickPassive(x); });
		}
	}

	private void Update()
	{
		if (playerStats == null)
		{
			playerStats = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerStats>();
		}

		if (resetPassives)
		{
			resetPassives = false;
			ResetPassives();
		}

		if (resetSpells)
		{
			resetSpells = false;
			ResetSpells();
		}

	}
}
