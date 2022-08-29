using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DundertaleManager : MonoBehaviour
{
    public static bool minigameDone = false;
    
    public GameObject butterfly;

    private int die = 0;
    private float timeLeft = 1f;
    private bool done = false;

    void Awake()
    {
        minigameDone = false;
        die = Random.Range(0, 0);

        switch (die)
        {
        case 0:
        timeLeft = 4f;
            for (int i = 0; i <= 6; i++)
            {
                GameObject.Instantiate(butterfly, this.transform);
            }
            break;
        }
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (!done && timeLeft <= 0f)
        {
            done = true;
            Finish();
        }
    }

    void Finish()
    {
        switch (die)
        {
        case 0:
            Debug.Log("Done!");
            break;
        }

        minigameDone = true;
    }
}
