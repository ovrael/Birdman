using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class PlayerStats : MonoBehaviour
{
	private const int fixedUpdateRate = 50;

	SceneChanger sceneChanger;

	[Header("God mode")]
	[SerializeField] bool unlimitedHP;
	[SerializeField] bool unlimitedMP;
	[SerializeField] bool noCooldowns;
	[Tooltip("Player will stay alive at 1HP")]
	[SerializeField] bool cantDie;

	public bool NoCooldowns { get => noCooldowns; }

	#region Health
	[Header("Health")]

	// Available in Unity
	[SerializeField] float maxHP = 500;
	[SerializeField] float currentHP = 500;
	[Tooltip("The regen is applied once per second")]
	[SerializeField] float regenHP = 2.5f;
	[Space]
	[SerializeField] Slider hpSlider;

	// Properties
	public float MaxHP
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
			else
			{
				currentHP = maxHP;
			}
		}
	}

	public float RegenHP
	{
		get => regenHP;
		set => regenHP = value;
	}


	#endregion

	#region Mana
	[Header("Mana")]

	// Available in Unity
	[SerializeField] float maxMP = 200;
	[SerializeField] float currentMP = 200;
	[Tooltip("The regen is applied once per second")]
	[SerializeField] float regenMP = 1.5f;
	[Space]
	[SerializeField] Slider mpSlider;

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
			if (value > maxMP)
			{
				currentMP = maxMP;
			}
		}
	}
	public float RegenMP
	{
		get => regenMP;
		set => regenMP = value;
	}

	#endregion

	#region Experience
	[Header("Experience")]

	// Available in Unity
	[SerializeField] float expNeededToLevelUp = 600;
	[SerializeField] float currentExp = 0;
	[Space]
	[SerializeField] int level = 1;
	[SerializeField] TMP_Text levelText;
	[Space]
	[SerializeField] int spellPoints = 1;
	[SerializeField] TMP_Text spellPointsText;
	[Space]
	[SerializeField] Slider expSlider;

	public float CurrentExp
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

	public float ExpNeededToLevelUp
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
				if (spellPointsText == null)
					FindLeftPointsText();
				spellPointsText.text = spellPoints.ToString();
			}
		}
	}

	void LevelUpPlayer()
	{
		UpdateStats();
		UpdateHUD();
		// The rest - WIP
	}

	void UpdateHUD()
	{
		expSlider.maxValue = expNeededToLevelUp;
		hpSlider.maxValue = MaxHP;
		mpSlider.maxValue = MaxMP;
		levelText.text = level.ToString();

		if (spellPointsText == null)
			FindLeftPointsText();
		spellPointsText.text = spellPoints.ToString();
	}

	void UpdateStats()
	{
		level++;
		spellPoints++;

		currentExp -= expNeededToLevelUp;
		expNeededToLevelUp += 50;

		bool isFullHP = (CurrentHP >= MaxHP) ? true : false;
		bool isFullMP = (CurrentMP >= MaxMP) ? true : false;

		maxHP += 30;
		if (isFullHP)
			currentHP = maxHP;

		maxMP += 10;
		if (isFullMP)
			currentMP = maxMP;
	}

	#endregion

	#region Movement
	[Header("Movement")]

	// Available in Unity
	[SerializeField] float speed = 20;                               // Start at about 20
	[SerializeField] float jumpPower = 250;                          // Start at about 250  

	// Properties
	public float Speed { get => speed; }
	public float JumpPower { get => jumpPower; }

	#endregion

	public void TakeDamage(float damageTaken)
	{
		CurrentHP -= damageTaken;
	}

	public void SetUp()
	{
		hpSlider.minValue = 0;
		hpSlider.maxValue = maxHP;
		hpSlider.value = currentHP;

		mpSlider.minValue = 0;
		mpSlider.maxValue = maxMP;
		mpSlider.value = currentMP;

		expSlider.minValue = 0;
		expSlider.maxValue = expNeededToLevelUp;
		expSlider.value = currentExp;

		levelText.text = level.ToString();
		spellPointsText.text = spellPoints.ToString();
	}


	private void Awake()
	{
		SetUp();
	}

	// Start is called before the first frame update
	void Start()
	{
		GameObject sceneManager = GameObject.FindGameObjectsWithTag("Manager").Where(g => g.name == "GameManager").FirstOrDefault();

		if (sceneManager != null)
		{
			sceneChanger = sceneManager.GetComponent<SceneChanger>();
		}
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

		if (currentHP <= 0)
		{
			if (!cantDie)
			{
				SpellSystem spellSystem = transform.parent.GetComponentInChildren<SpellSystem>();

				DataManager.GetPlayerData(this, spellSystem);



				//Time.timeScale = 0f;
				sceneChanger.LoadGameOver();
			}
			else
			{
				currentHP = 1f;
			}
		}

		if (spellPointsText == null)
			FindLeftPointsText();
	}

	private void FindLeftPointsText()
	{
		var tmpTexts = Resources.FindObjectsOfTypeAll<TMP_Text>();
		foreach (var text in tmpTexts)
		{
			if (text.name == "LeftPointsText")
			{
				spellPointsText = text;
				break;
			}
		}
	}

	private void FixedUpdate()
	{
		CurrentHP += regenHP / fixedUpdateRate;
		CurrentMP += regenMP / fixedUpdateRate;
	}
}
