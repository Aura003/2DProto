using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { private set; get; }

    [Header("DamageNumbers")]
    public DamagerNumber DamageNumberPrefab;
    public int InitialPoolSize;


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
}
