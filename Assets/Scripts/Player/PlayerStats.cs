using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using TMPro;

public class PlayerStats : MonoBehaviour, ITakeDamage
{
	private const int fixedUpdateRate = 50;         // Value needed to correctly apply regeneration
	[SerializeField] SceneChanger sceneChanger;     // Calls "LoadGameOver" scene when player die

	[Header("God mode")]
	[SerializeField] bool unlimitedHP;              // Give player max health every update
	[SerializeField] bool unlimitedMP;              // Give player max mana every update
	[SerializeField] bool noCooldowns;              // Removes cooldowns
	[Tooltip("Player will stay alive at 1HP")]
	[SerializeField] bool cantDie;                  // Player never dies (stays at 1hp)

	public bool NoCooldowns { get => noCooldowns; }

	[Header("Bools")]
	[SerializeField] bool resetStats = false;
	[SerializeField] bool printPassvieNodesIds = false;

	#region Health
	[Header("Health")]

	// Available in Unity
	[SerializeField] Stat health;
	[SerializeField] float currentHP = 500;
	[SerializeField] float hpBoostPerLevel = 30;
	[Space]
	[Tooltip("Health regeneration per second (applied 1/50 value per fixedUpdate()")]
	[SerializeField] Stat regenHP;
	[Space]
	[SerializeField] Slider hpSlider;               // UI Slider that shows current HP

	// Properties
	public Stat Health
	{
		get => health;
		set
		{
			health = value;
			hpSlider.maxValue = Health.CalculatedValue;
		}
	}

	public float CurrentHP
	{
		get => currentHP;
		set
		{
			if (value < health.CalculatedValue)
			{
				currentHP = value;
			}
			else
			{
				currentHP = health.CalculatedValue;
			}
		}
	}

	public Stat RegenHP
	{
		get => regenHP;
		set => regenHP = value;
	}


	#endregion

	#region Mana
	[Header("Mana")]

	// Available in Unity
	[SerializeField] Stat mana;
	[SerializeField] float currentMP = 200;
	[SerializeField] float mpBoostPerLevel = 20;
	[Tooltip("Health regeneration per second (applied 1/50 value per fixedUpdate()")]
	[SerializeField] Stat regenMP;
	[Space]
	[SerializeField] Slider mpSlider;                // UI Slider that shows current MP

	// Properties
	public Stat Mana
	{
		get => mana;
		set
		{
			mana = value;
			mpSlider.maxValue = Mana.CalculatedValue;
		}
	}

	public float CurrentMP
	{
		get => currentMP;
		set
		{
			if (value >= 0 && value <= Mana.CalculatedValue)
			{
				currentMP = value;
			}
			if (value > Mana.CalculatedValue)
			{
				currentMP = Mana.CalculatedValue;
			}
		}
	}
	public Stat RegenMP
	{
		get => regenMP;
		set => regenMP = value;
	}

	#endregion

	#region Experience
	[Header("Experience")]

	// Available in Unity
	[SerializeField] int expNeededToLevelUp = 600;
	[SerializeField] int currentExp = 0;
	[Space]
	[SerializeField] int level = 1;
	[SerializeField] TMP_Text levelText;
	[Space]
	[SerializeField] int spellPoints = 1;
	[SerializeField] TMP_Text spellPointsText;
	[Space]
	[SerializeField] int passivePoints = 1;
	[SerializeField] TMP_Text passivePointsText;
	[Space]
	[SerializeField] Slider expSlider;

	public int CurrentExp
	{
		get => currentExp;
		set
		{
			if (value > 0)
			{
				currentExp = value;
			}
		}
	}
	public int ExpNeededToLevelUp
	{
		get => expNeededToLevelUp;
		set
		{
			if (value > 0)
			{
				expNeededToLevelUp = value;
			}
		}
	}

	public int Level
	{
		get => level;
		set
		{
			if (value >= 1)
			{
				level = value;
				levelText.text = spellPoints.ToString();
			}
		}
	}

	public int SpellPoints
	{
		get => spellPoints;
		set
		{
			if (value >= 0)
			{
				spellPoints = value;
				if (spellPointsText != null)
					spellPointsText.text = spellPoints.ToString();
			}
		}
	}

	public int PassivePoints
	{
		get => passivePoints;
		set
		{
			if (value >= 0)
			{
				passivePoints = value;
				if (passivePointsText != null)
					passivePointsText.text = passivePoints.ToString();
			}
		}
	}

	void LevelUpPlayer()
	{
		UpdateStats();
		UpdateHUD();
	}

	void CalcNeededExperienceToLevelUp()
	{
		float a = 5;        // a > 0
		float b = 10;
		float c = 485;      // a+b+c = exp needed to level up at 1 level

		// The basic quadratic function to calc
		expNeededToLevelUp = (int)Mathf.Floor(a * Level * Level + b * Level + c);
	}

	#endregion

	#region Taking Damage
	[Header("Taking Damage")]

	[Tooltip("If value is below zero, object takes increased damage")]
	[SerializeField] Stat armor;
	bool tookDamage = false;

	public Stat Armor
	{
		get => armor;
		set => armor = value;
	}

	public void TakeDamage(float damageTaken)
	{
		tookDamage = true;
		float damageAfterReduction = ((100f - Armor.CalculatedValue) / 100) * damageTaken;
		CurrentHP -= damageAfterReduction;
	}

	#endregion

	#region Movement
	[Header("Movement")]

	// Available in Unity
	[SerializeField] float speed = 20;
	[SerializeField] float jumpPower = 15;

	// Properties
	public float Speed { get => speed; }
	public float JumpPower { get => jumpPower; }

	#endregion

	#region Passives
	[Header("Passives")]
	public List<int> PassiveIds;

	#endregion

	#region Material and colors
	[Header("Material and colors")]
	[SerializeField] Material spriteMaterial;
	[SerializeField] Color takeDamageTint;
	Color currentTint;
	[SerializeField] float tintFadeSpeed;

	#endregion

	#region Methods
	void UpdateStats()
	{
		Level++;
		SpellPoints++;
		PassivePoints++;

		currentExp -= expNeededToLevelUp;
		CalcNeededExperienceToLevelUp();

		bool isFullHP = (CurrentHP >= Health.CalculatedValue) ? true : false;
		bool isFullMP = (CurrentMP >= Mana.CalculatedValue) ? true : false;

		health.BaseValue += hpBoostPerLevel;
		if (isFullHP)
			currentHP = health.CalculatedValue;

		mana.BaseValue += mpBoostPerLevel;
		if (isFullMP)
			currentMP = Mana.CalculatedValue;
	}

	void UpdateHUD()
	{
		expSlider.maxValue = expNeededToLevelUp;
		hpSlider.maxValue = Health.CalculatedValue;
		mpSlider.maxValue = Mana.CalculatedValue;
		levelText.text = level.ToString();

		if (spellPointsText != null)
			spellPointsText.text = spellPoints.ToString();

		if (passivePointsText != null)
			passivePointsText.text = passivePoints.ToString();
	}

	public void SetUp()
	{
		hpSlider.minValue = 0;
		hpSlider.maxValue = health.CalculatedValue;
		hpSlider.value = currentHP;

		mpSlider.minValue = 0;
		mpSlider.maxValue = Mana.CalculatedValue;
		mpSlider.value = currentMP;

		CalcNeededExperienceToLevelUp();
		expSlider.minValue = 0;
		expSlider.maxValue = expNeededToLevelUp;
		expSlider.value = currentExp;

		levelText.text = level.ToString();
		if (spellPointsText != null)
			spellPointsText.text = spellPoints.ToString();
		if (passivePointsText != null)
			passivePointsText.text = passivePoints.ToString();

		spriteMaterial.SetColor("_Tint", currentTint);
	}

	public void ResetStats()
	{
		Health = new Stat(Health.BaseValue);
		Mana = new Stat(Mana.BaseValue);
		RegenHP = new Stat(RegenHP.BaseValue);
		RegenMP = new Stat(RegenMP.BaseValue);
		Armor = new Stat(Armor.BaseValue);

		PassiveIds = new List<int>();
		PassivePoints = Level;
	}

	public void PrintPickedNodes()
	{
		string nodes = string.Empty;
		foreach (var node in PassiveIds)
		{
			nodes += node + ", ";
		}

		Debug.Log("Picked nodes: " + nodes);
	}

	private TMP_Text FindTextObjectByName(string objectName)
	{
		var tmpTexts = Resources.FindObjectsOfTypeAll<TMP_Text>();
		foreach (var text in tmpTexts)
		{
			if (text.name == objectName)
			{
				return text;
			}
		}
		return null;
	}
	#endregion

	#region Unity Methods

	void Start()
	{
		// Finds sceneChanger if loses reference
		if (sceneChanger != null)
		{
			GameObject sceneManager = GameObject.FindGameObjectsWithTag("Manager").Where(g => g.name == "GameManager").FirstOrDefault();
			sceneChanger = sceneManager.GetComponent<SceneChanger>();
		}

		SetUp();
	}

	private void Update()
	{
		hpSlider.value = currentHP;
		mpSlider.value = currentMP;
		expSlider.value = currentExp;

		if (tookDamage)
		{
			currentTint = takeDamageTint; // Makes hero material red
			tookDamage = false;
		}

		// Changes hero material back to normal color through time
		if (currentTint.a > 0)
		{
			currentTint.a = Mathf.Clamp01(currentTint.a - tintFadeSpeed * Time.deltaTime);
			spriteMaterial.SetColor("_Tint", currentTint);
		}

		// Check if can level up
		if (currentExp >= expNeededToLevelUp)
		{
			LevelUpPlayer();
		}

		// Check if dying
		if (currentHP <= 0)
		{
			if (!cantDie)
			{
				SpellSystem spellSystem = transform.parent.GetComponentInChildren<SpellSystem>();
				DataManager.GetPlayerData(this, spellSystem);

				sceneChanger.LoadGameOver();
			}
			else
			{
				currentHP = 1f;
			}
		}

		if (spellPointsText == null)
			spellPointsText = FindTextObjectByName("LeftSpellPointsText");

		if (passivePointsText == null)
			passivePointsText = FindTextObjectByName("LeftPassivePointsText");


		if (resetStats)
		{
			ResetStats();
			resetStats = false;
		}

		if (printPassvieNodesIds)
		{
			PrintPickedNodes();
			printPassvieNodesIds = false;
		}


		// Cheats
		if (unlimitedHP)
		{
			currentHP = health.CalculatedValue;
		}

		if (unlimitedMP)
		{
			currentMP = Mana.CalculatedValue;
		}
	}

	private void FixedUpdate()
	{
		CurrentHP += RegenHP.CalculatedValue / fixedUpdateRate;
		CurrentMP += RegenMP.CalculatedValue / fixedUpdateRate;
	}

	#endregion
}
