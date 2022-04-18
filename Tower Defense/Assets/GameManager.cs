using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int EnemyValue; //The value of money to assign as a prize to new enemy spawns
    public int EnemyLives; // The number of lives to assign an enemy on spawn
    void Start()
    {
        EnemyLives = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
