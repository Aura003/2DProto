using UnityEngine;

public class Stats : MonoBehaviour
{
    [Header("Base-Stats")]
    [SerializeField] private int strength;
    [SerializeField] private int vitality;
    [SerializeField] private int agility;
    [SerializeField] private int luck;
    [Header("Stat-Scaling")]
    private int baseDamage = 2;
    private int damagePerStrength = 2;
    private int baseHealth = 100;
    private int healthPerVitality = 5;
    private float baseEvasion = 5;
    private float evasionPerAgility = 1;
    private float baseCrit = 5;
    private float critPerLuck = 1f;

    public int Strength => strength;
    public int Vitality => vitality;
    public int Agility => agility;
    public int Luck => luck;

    public int Damage => baseDamage + (Strength * damagePerStrength);
    public int MaxHp=> baseHealth + (Vitality * healthPerVitality);

    public float EvasionRating => baseEvasion + (Agility * evasionPerAgility);

    public float CritRate => baseCrit +(Luck * critPerLuck);

    public bool RollEvasion()
    {
        return Random.value * 100f < EvasionRating;
    }
    public bool RollCrit()
    {
        return Random.value *100f < CritRate;
    }

}
