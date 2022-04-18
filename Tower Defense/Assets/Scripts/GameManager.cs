using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject Enemy;
    [SerializeField] private Transform SpawnPoint;
    [HideInInspector] public int EnemyValue; //The value of money to assign as a prize to new enemy spawns
    [HideInInspector] public int EnemyLives; // The number of lives to assign an enemy on spawn
    [HideInInspector] public float EnemySpeed; // The movement speed of spawned enemies
    private float DelayBetweenEnemies; // The delay between enemy spawns in seconds, ramps up as game progresses
    private int EnemiesSpawned; // Tracks the number of enemies spawned, used to ramp difficulty
    private int NextDifficultyRamp; // When this many total enemies have been spawned, increase difficulty
    private GameObject EnemyParent; // Keeps the enemies organized under a parent object
    
    public static int S_PlayerLives; // The number of lives the player has (-1 per escaped enemy)
    private static TMP_Text s_LivesText;
    public static int S_PlayerCash; // The amount of cash the player has (spent on towers/upgrades)
    private static TMP_Text s_CashText;
    public static int S_PlayerScore; // The amount of enemies the player has defeated
    private static TMP_Text s_ScoreText;
    void Start()
    {
        s_LivesText = GameObject.Find("LivesText").GetComponent<TMP_Text>();
        s_CashText = GameObject.Find("CashText").GetComponent<TMP_Text>();
        s_ScoreText = GameObject.Find("ScoreText").GetComponent<TMP_Text>();
        EnemyParent = GameObject.Find("Enemies");
        EnemyLives = 1;
        EnemyValue = 3;
        EnemySpeed = 0.0025f;
        S_PlayerLives = 100;
        S_PlayerCash = 100;
        DelayBetweenEnemies = 2;
        NextDifficultyRamp = 20;

        StartCoroutine(EnemySpawning()); // Starts spawning enemies, will continue until the player dies or the game crashes.
    }

    void Update()
    {
        
    }

    private IEnumerator EnemySpawning()
    {
        while (true) // This coroutine runs forever if uninterrupted
        {
            Instantiate(Enemy, SpawnPoint.position, Quaternion.identity, EnemyParent.transform);
            EnemiesSpawned++; // Counts the spawned enemy
            if (EnemiesSpawned == NextDifficultyRamp) //When enough enemies have been spawned, increase the strength of future enemies and re-raise the bar.
            {
                DifficultyRamp();
                Debug.Log(NextDifficultyRamp + " enemies spawned, ramping difficulty");
                NextDifficultyRamp *= 2;
            }
            yield return new WaitForSeconds(DelayBetweenEnemies);
        }
    }
    private void DifficultyRamp()
    {
        LivesRamp();
        SpeedRamp();
        DelayRamp();
    }
    private void LivesRamp() // Increases enemy lives by 10% or 1, whichever is higher.
    {
        if (EnemyLives * 0.1 < 1)
        {
            EnemyLives += 1;
        }
        else
        {
            EnemyLives += Mathf.RoundToInt(EnemyLives * 0.1f);
        }
    }
    private void SpeedRamp()
    {
        EnemySpeed *= 1.05f;
    }
    private void DelayRamp()
    {
        if (DelayBetweenEnemies > 0.2)
        {
            DelayBetweenEnemies *= 0.9f;
        }
    }
    public void EnemyLeaked()
    {
        S_PlayerLives--;
        s_LivesText.text = ("Lives: " + S_PlayerLives.ToString());
    }
    public static void SetMoney(int MoneyValue)
    {
        S_PlayerCash = MoneyValue;
        s_CashText.text = ("Cash: " + S_PlayerCash.ToString() + "$");
    }
    public static void ScoreUp(int PointsGained)
    {
        S_PlayerScore += PointsGained;
        s_ScoreText.text = ("Score: " + S_PlayerScore.ToString());
    }
}
