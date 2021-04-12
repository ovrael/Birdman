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
	[SerializeField] GameObject cantPickInfo;

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
	[SerializeField] Material materialWithBetterShader;
	[Space]
	[SerializeField] Color activeColor;
	[SerializeField] Color activeTint;
	[Space]
	[SerializeField] Color inactiveColor;
	[SerializeField] Color inactiveTint;


	public void CheckIfCanPickPassive(int index)
	{
		bool hasRequiredNode = false;
		PassiveButton passiveButton = passiveButtons[index];

		if (playerStats.PassivePoints <= 0 || passiveButtons[index].node.isPicked)
		{
			StartCoroutine(ShowCantPickInfo());
			return;
		}

		foreach (PassiveNodeScript node in passiveButton.requiredNodes)
		{
			if (node.isPicked)
			{
				hasRequiredNode = true;
				break;
			}
		}

		if (passiveButton.requiredNodes.Length == 0)
			hasRequiredNode = true;

		if (hasRequiredNode)
		{
			ApplyPassiveNode(passiveButton.node);
			ActivateNode(passiveButton.button.image.material);
		}
		else
		{
			StartCoroutine(ShowCantPickInfo());
		}

	}
	IEnumerator ShowCantPickInfo()
	{
		cantPickInfo.SetActive(true);
		yield return new WaitForSeconds(1.5f);
		cantPickInfo.SetActive(false);
	}

	public void ApplyPassiveNode(PassiveNodeScript passiveNode)
	{
		bool error = false;
		foreach (Node node in passiveNode.nodes)
		{
			switch (node.type)
			{
				case NodeType.FlatIncrease:
					{
						switch (node.targetStat)
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
						switch (node.targetStat)
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
						switch (node.targetStat)
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
						switch (node.targetStat)
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
			StartCoroutine(ShowCantPickInfo());
			Debug.LogError("Error node in: " + passiveNode.nodeName);
		}
		else
		{
			playerStats.PassiveIds.Add(passiveNode.id);
			playerStats.PassivePoints--;

			passiveNode.isPicked = true;
		}
	}

	public void ActivateNode(Material material)
	{
		material.SetColor("_Color", activeColor);
		material.SetColor("_Tint", activeTint);
	}
	public void DeactivateNode(Material material)
	{
		material.SetColor("_Color", inactiveColor);
		material.SetColor("_Tint", inactiveTint);
	}

	private void ResetPassives()
	{
		foreach (PassiveButton passiveButton in passiveButtons)
		{
			passiveButton.node.isPicked = false;
			DeactivateNode(passiveButton.button.image.material);
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

			Material newMaterial = new Material(materialWithBetterShader);
			newMaterial.SetTexture("_MainTex", image.mainTexture);

			image.material = newMaterial;

			if (!passiveButtons[x].node.isPicked)
				DeactivateNode(image.material);

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
