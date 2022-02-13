using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GymMinigameController : MonoBehaviour
{
    private Gym gymControls;
    private Coroutine currentMicrogame;

    // Lift microgame variables
    public int liftTarget = 10000;
    public int liftAmount = 0;
    public int liftGainPerPress = 500;
    public int liftLossPerFrame = 1;
    public float liftTime = 5f;
    public Slider liftSlider;

    // Punch microgame variables
    public int punchTarget = 3; // How many punches until success
    public float punchTime = 3f;
    private int punchSide; // 0 is left, 1 is right
    private int curPunches = 0;

    // Start is called before the first frame update
    void Start()
    {
        gymControls = new Gym();

        gymControls.GymActions.Lift.started += ctx => Lift();
        gymControls.GymActions.Jab.started += ctx => Jab();
        gymControls.GymActions.Cross.started += ctx => Cross();

        currentMicrogame = StartCoroutine(PunchMicrogame());
    }

    #region Lift

    IEnumerator LiftMicrogame()
    {
        Debug.Log("Lifting microgame start!");

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

    #endregion
    #region Punch

    IEnumerator PunchMicrogame()
    {
        Debug.Log("Punch microgame start!");

        // Initialize variables
        gymControls.GymActions.Jab.Enable();
        gymControls.GymActions.Cross.Enable();
        curPunches = 0;
        float currentTime = 0f;
        punchSide = YarnFunctions.RandomRange(0, 1); // Damn, I'm so clever for this
        Debug.Log((punchSide == 0) ? "Punch left!" : "Punch right!");

        // Repeat every frame until punch target is reached or time limit is exceeded
        while (curPunches < punchTarget && currentTime < punchTime)
        {
            currentTime += Time.deltaTime;
            yield return null; // Wait a frame before repeating
        }

        if (curPunches >= punchTarget)
        {
            Debug.Log("Succeeded at punching!");
            gymControls.GymActions.Jab.Disable();
            gymControls.GymActions.Cross.Disable();
        }
        else
        {
            PunchFail();
        }
    }

    // Punch from the left
    void Jab()
    {
        if (punchSide == 0)
        {
            curPunches++;
            punchSide = YarnFunctions.RandomRange(0, 1);
            Debug.Log((punchSide == 0) ? "Punch left!" : "Punch right!");
        }
        else
        {
            PunchFail();
        }
    }

    // Punch from the right
    void Cross()
    {
        if (punchSide == 1)
        {
            curPunches++;
            punchSide = YarnFunctions.RandomRange(0, 1);
            Debug.Log((punchSide == 0) ? "Punch left!" : "Punch right!");
        }
        else
        {
            PunchFail();
        }
    }

    void PunchFail()
    {
        StopCoroutine(currentMicrogame);
        gymControls.GymActions.Jab.Disable();
        gymControls.GymActions.Cross.Disable();

        Debug.Log("Failed at punching!");
    }

    #endregion
}
