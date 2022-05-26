using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Credits : MonoBehaviour
{
    public float endPosY = 1000f;
    public float duration = 30f;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<RectTransform>().DOAnchorPosY(endPosY, duration).SetEase(Ease.Linear).OnComplete(() => SceneManager.LoadScene(0));
    }
}
