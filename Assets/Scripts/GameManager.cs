using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { private set; get; }

    [Header("DamageNumbers")]
    public DamagerNumber DamageNumberPrefab;
    public int InitialPoolSize;
    [Header("RPG Stats")]
    public Stats PlayerStats;

    [Header("StatsUI")]
    public StatRow StrengthRow;
    public StatRow VitRow;
    public StatRow AgiRow;
    public StatRow LuckRow;

    private readonly Queue<DamagerNumber> availableNumbers = new Queue<DamagerNumber>();
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        CreatePool();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RefreshUI();
        StrengthRow.IncreaseBTN.onClick.AddListener(IncreaseSTR);
        AgiRow.IncreaseBTN.onClick.AddListener(IncreaseAGI);
        VitRow.IncreaseBTN.onClick.AddListener(IncreaseVIT);
        LuckRow.IncreaseBTN.onClick.AddListener(IncreaseLUK);

        PlayerStats.OnStatsChanged += RefreshUI;
        PlayerStats.OnLevelUP += RefreshUI;
    }

    void CreatePool()
    {
        for (int i = 0; i < InitialPoolSize; i++)
        {
            CreateNumber();
        }
    }
    DamagerNumber CreateNumber()
    {
        DamagerNumber number = Instantiate(DamageNumberPrefab, transform);
        number.Initialize(this);
        number.gameObject.SetActive(false);
        availableNumbers.Enqueue(number);
        return number;
    }
    public void ShowNumbers(int damage, Vector3 position, bool isCritical = false)
    {
        DamagerNumber number = GetNumber();

        Color c = isCritical ? Color.red : Color.yellow;
        number.DisplayDamage(damage, position, isCritical,c);

    }
    DamagerNumber GetNumber()
    {
        if (availableNumbers.Count > 0)
            return availableNumbers.Dequeue();

        DamagerNumber number = Instantiate(DamageNumberPrefab, transform);
        number.Initialize(this);
        return number;
    }
    DamagerNumber CreateNumberFromPool()
    {
        DamagerNumber number = Instantiate(DamageNumberPrefab, transform);
        number.Initialize(this);
        return number;
    }
    public void Release(DamagerNumber number)
    {
        number.gameObject.SetActive(false);
        availableNumbers.Enqueue(number);
    }

    public void RefreshUI()
    {
        StrengthRow.BaseValue.text = PlayerStats.Strength.ToString();
        StrengthRow.DerivedValue.text = PlayerStats.Damage.ToString();

        VitRow.BaseValue.text = PlayerStats.Vitality.ToString();
        VitRow.DerivedValue.text = PlayerStats.MaxHp.ToString();

        AgiRow.BaseValue.text = PlayerStats.Agility.ToString();
        AgiRow.DerivedValue.text = $"{PlayerStats.EvasionRating:0.#}%";


        LuckRow.BaseValue.text=PlayerStats.Luck.ToString();
        LuckRow.DerivedValue.text = $"{PlayerStats.CritRate:0.#}%";

        bool canIncrease = PlayerStats.AvailableStatPoints > 0;
        StrengthRow.IncreaseBTN.interactable = canIncrease;
        VitRow.IncreaseBTN.interactable = canIncrease;
        AgiRow.IncreaseBTN.interactable = canIncrease;
        LuckRow.IncreaseBTN.interactable = canIncrease;
    }
    void IncreaseSTR()
    {
        PlayerStats.TryIncreaseStat(StatType.STR);
    }
    void IncreaseVIT()
    {
        PlayerStats.TryIncreaseStat(StatType.VIT);
    }
    void IncreaseAGI()
    {
        PlayerStats.TryIncreaseStat(StatType.AGI);
    }
    void IncreaseLUK()
    {
        PlayerStats.TryIncreaseStat(StatType.LUK);
    }
}
[System.Serializable]
public class StatRow
{
    public Button IncreaseBTN;
    public TextMeshProUGUI BaseValue;
    public TextMeshProUGUI DerivedValue;
}