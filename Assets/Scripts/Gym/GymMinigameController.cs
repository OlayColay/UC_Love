using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GymMinigameController : MonoBehaviour
{
    private Gym gymControls;

    // Lift microgame variables
    public int liftTarget = 10000;
    public int liftAmount = 0;
    public int liftGainPerPress = 500;
    public int liftLossPerFrame = 1;
    public float liftTime = 5f;
    public Slider liftSlider;

    // Start is called before the first frame update
    void Start()
    {
        gymControls = new Gym();

        gymControls.GymActions.Lift.started += ContextMenu => Lift();

        StartCoroutine(LiftMicrogame());
    }

    IEnumerator LiftMicrogame()
    {
        Debug.Log("Lifting minigame start!");

        // Initialize variables
        gymControls.GymActions.Lift.Enable();
        liftAmount = 0;
        float currentTime = 0f;
        liftSlider.maxValue = liftTarget;
        liftSlider.gameObject.SetActive(true);

        // Repeat every frame until lift target is reached or time limit is exceeded
        while (liftAmount < liftTarget && currentTime < liftTime)
        {
            liftAmount = Math.Max(liftAmount - liftLossPerFrame, 0);
            liftSlider.value = liftAmount;
            currentTime += Time.deltaTime;
            yield return null; // Wait a frame before repeating
        }

        liftSlider.value = liftAmount;
        if (liftAmount >= liftTarget)
        {
            Debug.Log("Succeeded at lifting!");
        }
        else
        {
            Debug.Log("Failed at lifting!");
        }

        gymControls.GymActions.Lift.Disable();
    }

    void Lift()
    {
        Debug.Log("Heave!");
        liftAmount += liftGainPerPress;
    }
}
