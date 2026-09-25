using System;
using UnityEngine;
public enum StatType { STR,VIT,AGI,LUK};
public class Stats : MonoBehaviour
{
    [Header("Base-Stats")]
    [SerializeField] private int strength;
    [SerializeField] private int vitality;
    [SerializeField] private int agility;
    [SerializeField] private int luck;

    [Header("StatPointsAndExp")]
    private int availableStatPoints = 0;
    [SerializeField]private int statPointsPerLevel = 3;
    private int level =1;
    private int currentExperience = 0;
    private int requiredExperience=100;

    [Header("Stat-Scaling")]
    private int baseDamage = 2;
    private int damagePerStrength = 2;
    private int baseHealth = 100;
    private int healthPerVitality = 5;
    private float baseEvasion = 1;
    private float evasionPerAgility = 1;
    private float baseCrit = 1;
    private float critPerLuck = 1f;

    public int Strength => strength;
    public int Vitality => vitality;
    public int Agility => agility;
    public int Luck => luck;

    public int Level => level;
    public int CurrentExp => currentExperience;
    public int AvailableStatPoints => availableStatPoints;
    public int RequiredExperience => requiredExperience * (1<<(Level-1));
    public int Damage => baseDamage + (Strength * damagePerStrength);
    public int MaxHp=> baseHealth + (Vitality * healthPerVitality);

    public float EvasionRating => baseEvasion + (Agility * evasionPerAgility);

    public float CritRate => baseCrit +(Luck * critPerLuck);

    public event Action OnLevelUP;
    public event Action OnStatsChanged;
    public bool RollEvasion()
    {
        return UnityEngine.Random.value * 100f < EvasionRating;
    }
    public bool RollCrit()
    {
        return UnityEngine.Random.value *100f < CritRate;
    }

    public void AddSTR(int value)
    {
        strength += value;
    }
    public void AddVIT(int value)
    {
        vitality += value;
    }
    public void AddAGI(int value)
    {
        agility += value;
    }
    public void AddLUK(int value)
    {
        luck += value;
    }
    public bool TryIncreaseStat(StatType type)
    {
        if (availableStatPoints <= 0)
            return false;

        switch (type)
        {
            case StatType.STR:
                strength++;
                break;
            case StatType.VIT:
                vitality++;
                break;
            case StatType.AGI:
                agility++;
                break;
            case StatType.LUK:
                luck++;
                break;

            default:
                return false;
        }
        availableStatPoints--;
        OnStatsChanged?.Invoke();
        return true;
    }
    public bool CanIncreaseStat(StatType type)
    {
        return availableStatPoints > 0;
    }
    public void AddExp(int amount)
    {
        if (amount < 0)
            return;
        currentExperience += amount;
        while (currentExperience >= requiredExperience)
        {
            currentExperience -= requiredExperience;
            LevelUP();
        }
        OnStatsChanged?.Invoke();
    }
    void LevelUP()
    {
        level++;
        availableStatPoints += statPointsPerLevel;
        OnLevelUP?.Invoke();
    }
}
