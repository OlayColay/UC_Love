using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public GameObject enemyChaser;
    public GameObject enemyRunner;
    public GameObject enemyLitterer;
    public GameObject enemySlacker;
    public int minEnemyCount = 5;
    public int maxEnemyCount = 10;

    //public GameObject layout1;
    // ...

    void Awake()
    {

        //        goal.GetComponent<SpriteRenderer>().enabled = false;
    
        //select a map layout randomly?
            //maybe harder maps only appear on hard difficulty?
        //some positions will have guaranteed enemy spots, placed in the scene


        SpawnEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnEnemies() //difficulty argument
    {
        var enemyTypes = new GameObject[] {enemyChaser, enemyRunner, enemyLitterer, enemySlacker};
        var spawnIndexList = new List<int>();
        int currentCount = 0;
        int enemyCount = Random.Range(minEnemyCount, maxEnemyCount);

        GameObject[] spawnPositions = GameObject.FindGameObjectsWithTag("Spawn");
        spawnIndexList.AddRange(Enumerable.Range(0, spawnPositions.Length));
        spawnIndexList = Shuffle(spawnIndexList);

        if (minEnemyCount > spawnPositions.Length)
        {
            Debug.Log("too many enemies (min), not enough spawn positions");
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
    
    static List<int> Shuffle (List<int> aList)
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
