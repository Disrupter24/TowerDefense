using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyEngine : MonoBehaviour
{
    private GameManager GameManager;
    private GameObject[] _pathingNodes; // An array storing the positions where the enemy will change course, pulled from nodes placed in the scene
    [SerializeField] private Transform _healthBarTransform;
    [SerializeField] private TMP_Text LivesText; // The reference to change the text on the UI Lives Counter, set in inspector
    private int _nodeProgress; // Keeps track of where the enemy is heading next (pathing)
    public int NumberOfLives; // Tracks the number of lives each enemy has, default value will increase with game length, if reduced to 0 enemy is killed
    private int CashBounty; // Tracks the amount of money each enemy will drop, default value will increase with game length, used to buy/upgrade towers
    private float MoveSpeed; // The speed at which the enemy moves
    public int StepsTaken; // Will be used by towers to establish priority
    private int StartingHealth; // Will be used to track how much damage has been taken for visuals

    private void Start() // Note: Enemies will be always generated at _pathingNodes[0].position
    {
        _pathingNodes = GameObject.FindGameObjectsWithTag("PathingNode");
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        SetStartingLives();
        SetStartingValue();
        SetStartingSpeed();
    }

    private void Update()
    {
        EnemyMovementHandler();
    }

    private void SetStartingLives()
    {
        StartingHealth = GameManager.EnemyLives;
        NumberOfLives = StartingHealth;
        LivesText.text = NumberOfLives.ToString();
    }
    private void SetStartingValue()
    {
        CashBounty = GameManager.EnemyValue;
    }
    private void SetStartingSpeed()
    {
        MoveSpeed = GameManager.EnemySpeed;
    }
    private void EnemyMovementHandler()
    {
        if(_nodeProgress == _pathingNodes.Length - 1) // Triggers when the enemy has moved through all of the pathing nodes and "cleared" the map.
        {
            EnemyEscape();
            return;
        }

        transform.position += MoveSpeed * GetNormalizedDirection(); // Moves the player MoveSpeed distance towards the next node.
        StepsTaken++;

        if (Vector3.Distance(transform.position, _pathingNodes[_nodeProgress + 1].transform.position) < 0.01f) // If the enemy reaches a node, begin pathing for the next one.
        {
            transform.position = _pathingNodes[_nodeProgress + 1].transform.position;
            _nodeProgress++;
        }
    }
    private Vector3 GetNormalizedDirection()
    {
        Vector3 OutputVector;
        OutputVector = _pathingNodes[_nodeProgress + 1].transform.position - transform.position;
        return Vector3.Normalize(OutputVector);
    }
    private void EnemyEscape()
    {
        GameManager.EnemyLeaked(); // Informs the GameManager that an enemy has leaked.
        Destroy(gameObject);
    }
    public void TakeDamage(int DamageTaken)
    {
        NumberOfLives -= DamageTaken;
        LivesText.text = NumberOfLives.ToString();
        float _HealthBarScale = (((float)NumberOfLives / (float)StartingHealth) * 0.9f);
        _healthBarTransform.localScale = new Vector3(_HealthBarScale, _HealthBarScale, 1);
        if (NumberOfLives <= 0)
        {
            GameManager.SetMoney(GameManager.S_PlayerCash + CashBounty);
            Destroy(gameObject);
        }
    }
}
