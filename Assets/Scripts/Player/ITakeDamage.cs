public interface ITakeDamage
{
	float PercentageDamageReduction { get; set; }
	void TakeDamage(float damageTaken);
}
