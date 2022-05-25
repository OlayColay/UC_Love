using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public string difficulty = "hard";

    public GameObject enemyChaser;
    public GameObject enemyRunner;
    public GameObject enemyLitterer;
    public GameObject enemySlacker;
    public int minEnemyCount = 15;
    public int maxEnemyCount = 20;

    public void SetEnemyDifficulty(string difficulty)
    {
        this.difficulty = difficulty;
        // Debug.Log("plaza difficulty: " + difficulty);

        // (speed, activationrange)
        //easy      150, 8
        //medium    200, 10
        //hard      270, 12
        enemyChaser.GetComponent<EnemyChaser>().SetDifficulty(difficulty);

        // (activationRange, starttimeBetweenShots) projectilespeed?
        //easy      12, 3
        //medium    15, 2
        //hard      16, 1.5
        enemyLitterer.GetComponent<EnemyLitterer>().SetDifficulty(difficulty);

        // (speed) 
        //easy      25
        //medium    35
        //hard      50
        enemySlacker.GetComponent<EnemySlacker>().SetDifficulty(difficulty);
        
        // (activationRange)
        //easy      13
        //medium    15
        //hard      17    
        enemyRunner.GetComponent<EnemyRunner>().SetDifficulty(difficulty);
    }

    public void SpawnEnemies(string difficulty)
    {
        //easy: 10-15
        //medium: 15-20
        //hard: 20-25
        if (difficulty == "easy")
        {
            minEnemyCount = 10;
            maxEnemyCount = 15;
        }
        else if (difficulty == "medium")
        {
            minEnemyCount = 15;
            maxEnemyCount = 20;
        }
        else if (difficulty == "hard")
        {
            minEnemyCount = 20;
            maxEnemyCount = 25;
        }

        var enemyTypes = new GameObject[] {enemyChaser, enemyRunner, enemyLitterer, enemySlacker};
        var spawnIndexList = new List<int>();
        int currentCount = 0;
        int enemyCount = Random.Range(minEnemyCount, maxEnemyCount);

        // Debug.Log("Plaza enemies spawned: " + enemyCount);

        GameObject[] spawnPositions = GameObject.FindGameObjectsWithTag("Spawn");
        spawnIndexList.AddRange(Enumerable.Range(0, spawnPositions.Length));
        spawnIndexList = Shuffle(spawnIndexList);

        if (minEnemyCount > spawnPositions.Length)
        {
            // Debug.Log("min number of enemies too large, not enough spawn positions");
        }

        while (currentCount <= enemyCount)
        {
            GameObject currentPoint = spawnPositions[spawnIndexList[currentCount]];
            Instantiate(enemyTypes[Random.Range(0, enemyTypes.Length)], currentPoint.transform.position, Quaternion.identity);
            currentCount++;
        }

        //hide spawn positions
        foreach (GameObject spawnPos in spawnPositions)
        {
            spawnPos.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
    
    private static List<int> Shuffle (List<int> aList)
    {
        // Fisher Yates Card Deck Shuffle
        System.Random _random = new System.Random ();

        int myGO;
        int n = aList.Count;
        for (int i = 0; i < n; i++)
        {
            // NextDouble returns a random number between 0 and 1.
            int r = i + (int)(_random.NextDouble() * (n - i));
            myGO = aList[r];
            aList[r] = aList[i];
            aList[i] = myGO;
        }
        return aList;
    }
}
