using System;
using UnityEngine;

[Serializable]
public struct Stat
{
	[SerializeField] private float calculatedValue;     // It is base value after implications = (baseValue + flatIncome) * (1f + percentIncome / 100f)
														// Changing value in below variables runs CalculateValue() function
	[SerializeField] private float baseValue;           // Starting stat value 
	[SerializeField] private float flatIncome;          // Flat value added to base one
	[SerializeField] private float percentIncome;       // Percentage modifier

	public float CalculatedValue { get => calculatedValue; set => calculatedValue = value; }
	public float BaseValue
	{
		get => baseValue;
		set
		{
			if (value > 0)
			{
				baseValue = value;
				CalculateValue();
			}
		}
	}
	public float FlatIncome
	{
		get => flatIncome;
		set
		{
			if (value >= 0)
			{
				flatIncome = value;
				CalculateValue();
			}
		}
	}
	public float PercentIncome
	{
		get => percentIncome;
		set
		{
			if (value >= 0)
			{
				percentIncome = value;
				CalculateValue();
			}
		}
	}


	public Stat(float baseValue, float flatIncome, float percentIncome) : this()
	{
		BaseValue = baseValue;
		FlatIncome = flatIncome;
		PercentIncome = percentIncome;
		CalculateValue();
	}

	public Stat(float baseValue) : this()
	{
		BaseValue = baseValue;
		CalculateValue();
	}

	public static Stat operator +(Stat baseStat, Stat add)
	{
		baseStat.baseValue += add.baseValue;
		baseStat.flatIncome += add.flatIncome;
		baseStat.percentIncome += add.percentIncome;

		baseStat.CalculateValue();

		return baseStat;
	}

	public static Stat operator -(Stat baseStat, Stat add)
	{
		baseStat.baseValue -= add.baseValue;
		baseStat.flatIncome -= add.flatIncome;
		baseStat.percentIncome -= add.percentIncome;

		baseStat.CalculateValue();

		return baseStat;
	}

	public void CalculateValue()
	{
		CalculatedValue = (BaseValue + FlatIncome) * (1 + PercentIncome / 100f);
	}
}
