using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butterfly : MonoBehaviour
{
    private float speed;
    private float amplitude;
    private float phase;
    private float t = 0f;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(3f, 5f);
        amplitude = Random.Range(2f, 5f);
        phase = Random.Range(20f, 50f);
        transform.position = new Vector2(Random.Range(0.2f, 0.8f) * Screen.width, -Random.Range(0f, 1f) * Screen.height);
    }

    void FixedUpdate()
    {
        t += speed / phase;
        transform.position += new Vector3(Mathf.Sin(t) * amplitude, speed);
    }
}
