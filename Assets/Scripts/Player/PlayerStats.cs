using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
<<<<<<< Updated upstream:Assets/Scripts/Player/PlayerStats.cs
=======
	[Header("God mode")]
	[SerializeField] bool unlimitedHP;
	[SerializeField] bool unlimitedMP;
	[SerializeField] bool noCooldowns;

	public bool NoCooldowns { get => noCooldowns; }
>>>>>>> Stashed changes:Assets/Scripts/PlayerStats.cs

	#region Health
	[Header("Health")]

	// Available in Unity
	[SerializeField] Slider hpSlider;
	[SerializeField] float maxHP = 500;
	[SerializeField] float currentHP = 500;

	// Properties
	public float MaxHp
	{
		get => maxHP;
		set
		{
			if (value > 0)
			{
				maxHP = value;
			}
		}
	}

	public float CurrentHP
	{
		get => currentHP;
		set
		{
			if (value <= maxHP)
			{
				currentHP = value;
			}
		}
	}


	#endregion

	#region Mana
	[Header("Mana")]

	// Available in Unity
	[SerializeField] Slider mpSlider;
	[SerializeField] float maxMP = 200;
	[SerializeField] float currentMP = 200;

	// Properties
	public float MaxMP
	{
		get => maxMP;
		set
		{
			if (value > 0)
			{
				maxMP = value;
			}
		}
	}

	public float CurrentMP
	{
		get => currentMP;
		set
		{
			if (value >= 0 && value <= maxMP)
			{
				currentMP = value;
			}
		}
	}

	#endregion

	#region Spells
	[Header("Spels")]

	// Available in Unity
	[SerializeField] int maxSpells = 3;
	[SerializeField] SpellData[] spells;

	public SpellData[] Spells
	{
		get => spells;
		set
		{
			if (value.Length <= maxSpells)
			{
				Spells = value;
			}
		}
	}



	#endregion

	#region Experience
	[Header("Experience")]

	// Available in Unity
	[SerializeField] Slider expSlider;
	[SerializeField] float expNeededToLevelUp = 600;
	[SerializeField] float currentExp = 0;
	[SerializeField] Text playerLevelText;
	[SerializeField] int playerLevel = 1;

	void LevelUpPlayer()
	{
		playerLevel++;

		UpdateStatsAndHUD();
		// The rest - WIP
	}

	void UpdateStatsAndHUD()
	{
		bool isFullHP = (currentHP >= maxHP) ? true : false;
		bool isFullMP = (currentMP >= maxMP) ? true : false;

		playerLevelText.text = string.Format($"{playerLevel}");

		currentExp -= expNeededToLevelUp;
		expNeededToLevelUp += 50;
		expSlider.maxValue = expNeededToLevelUp;

		maxHP += 30;
		hpSlider.maxValue = maxHP;
		if (isFullHP)
			currentHP = maxHP;

		maxMP += 10;
		mpSlider.maxValue = maxMP;
		if (isFullMP)
			currentMP = maxMP;
	}

	#endregion

	#region Movement
	[Header("Movement")]

	// Available in Unity
	[SerializeField] float speed = 20;                               // Start at about 20
	[SerializeField] float jumpPower = 250;                          // Start at about 250  
	/*[SerializeField] float fallMultiplier;                         // Start at about 3
	 */

	// Properties
	public float Speed { get => speed; }
	public float JumpPower { get => jumpPower; }
	//public float FallMultiplier { get => fallMultiplier; set => fallMultiplier = value; }

	#endregion

	private void Awake()
	{
		// Set up
		hpSlider.minValue = 0;
		hpSlider.maxValue = maxHP;
		hpSlider.value = currentHP;

		mpSlider.minValue = 0;
		mpSlider.maxValue = maxMP;
		mpSlider.value = currentMP;

		expSlider.minValue = 0;
		expSlider.maxValue = expNeededToLevelUp;
		expSlider.value = currentExp;

		playerLevelText.text = string.Format($"{playerLevel}");
	}

	// Start is called before the first frame update
	void Start()
	{

	}

	private void Update()
	{
		hpSlider.value = currentHP;
		mpSlider.value = currentMP;
		expSlider.value = currentExp;

		if (unlimitedHP)
		{
			currentHP = maxHP;
		}

		if (unlimitedMP)
		{
			currentMP = maxMP;
		}

		if (currentExp >= expNeededToLevelUp)
		{
			LevelUpPlayer();
		}
	}
}
