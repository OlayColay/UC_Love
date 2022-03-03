using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    void Start()
    {
        //Destroy(gameObject, 3);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Destroy(gameObject);
    }
}
