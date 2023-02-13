using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotatoPotato : MonoBehaviour
{
    public float degPerSec = 90f;

    // Start is called before the first frame update
    void Start()
    {
        transform.DOBlendableRotateBy(new Vector3(0f, 0f, degPerSec), 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
    }
}
