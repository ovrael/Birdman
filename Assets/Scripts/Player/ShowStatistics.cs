using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Text;


public class ShowStatistics : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	[SerializeField] Button showButton;
	[SerializeField] GameObject stats;
	[SerializeField] TMP_Text statsText;

	PlayerStats player;
	StringBuilder builderStats;

	void Start()
	{
		player = FindObjectOfType<PlayerStats>();
		builderStats = new StringBuilder();

		BuildStat(player.Health, "Health");
		BuildStat(player.RegenMP, "RegenHP");
		BuildStat(player.Mana, "Mana");
		BuildStat(player.RegenHP, "RegenMP");
		BuildStat(player.Armor, "Armor");

		statsText.text = builderStats.ToString();
	}

	void BuildDescription()
	{
		builderStats = new StringBuilder();

		BuildStat(player.Health, "Health");
		BuildStat(player.RegenHP, "RegenHP");
		BuildStat(player.Mana, "Mana");
		BuildStat(player.RegenMP, "RegenMP");
		BuildStat(player.Armor, "Armor");

		statsText.text = builderStats.ToString();
	}

	void BuildStat(Stat stat, string name)
	{
		builderStats.Append(string.Format("{0, -20}", name));
		builderStats.Append("- Calculated value: ");
		builderStats.Append(string.Format("{0, 6}", stat.CalculatedValue));
		builderStats.Append("\tBase value: ");
		builderStats.Append(string.Format("{0, 6}", stat.BaseValue));
		builderStats.Append("\tFlat income: ");
		builderStats.Append(string.Format("{0, 6}", stat.FlatIncome));
		builderStats.Append("\tPercent income: ");
		builderStats.AppendLine(string.Format("{0, 6}", stat.PercentIncome));
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		BuildDescription();
		stats.SetActive(true);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		stats.SetActive(false);
	}
}
