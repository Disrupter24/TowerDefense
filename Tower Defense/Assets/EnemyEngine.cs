using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyEngine : MonoBehaviour
{
    private GameManager GameManager;
    [SerializeField] private Transform[] _pathingNodes; // An array storing the positions where the enemy will change course, pulled from nodes placed in the scene, set in inspector
    [SerializeField] private TMP_Text LivesText; // The reference to change the text on the UI Lives Counter, set in inspector
    private int _nodeProgress; // Keeps track of where the enemy is heading next (pathing)
    public int NumberOfLives; // Tracks the number of lives each enemy has, default value will increase with game length, if reduced to 0 enemy is killed
    private int CashBounty; // Tracks the amount of money each enemy will drop, default value will increase with game length, used to buy/upgrade towers
    private float MoveSpeed = 0.005f; // The speed at which the enemy moves

    private void Start() // Note: Enemies will be always generated at _pathingNodes[0].position
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        SetStartingLives();
        SetStartingValue();
    }

    private void Update()
    {
        EnemyMovementHandler();
    }

    private void SetStartingLives()
    {
        NumberOfLives = GameManager.EnemyLives;
        LivesText.text = NumberOfLives.ToString();
    }
    private void SetStartingValue()
    {
        CashBounty = GameManager.EnemyValue;
    }
    private void EnemyMovementHandler()
    {
        if(_nodeProgress == _pathingNodes.Length - 1)
        {
            EnemyEscape();
            return;
        }

        transform.position += MoveSpeed * GetNormalizedDirection();

        if (Vector3.Distance(transform.position, _pathingNodes[_nodeProgress + 1].position) < 0.01f) // If the enemy reaches a node, begin pathing for the next one.
        {
            transform.position = _pathingNodes[_nodeProgress + 1].position;
            _nodeProgress++;
        }
    }
    private Vector3 GetNormalizedDirection()
    {
        Vector3 OutputVector;
        OutputVector = _pathingNodes[_nodeProgress + 1].position - transform.position;
        return Vector3.Normalize(OutputVector);
    }
    private void EnemyEscape()
    {
        //reduce player's life count
        Destroy(gameObject);
    }
}
