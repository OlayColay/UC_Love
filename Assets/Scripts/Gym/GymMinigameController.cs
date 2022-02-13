using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GymMinigameController : MonoBehaviour
{
    private Gym gymControls;
    private Coroutine currentMicrogame;
    public string[] microgames = new string[3] {"LiftMicrogame", "PunchMicrogame", "PushupMicrogame"};
    public float timeBetweenMicrogames = 1f;
    private int curGame = 0;

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
    public GameObject LeftClick;
    public GameObject RightClick;
    private int punchSide; // 0 is left, 1 is right
    private int curPunches = 0;

    // Pushup microgame variables
    public int pushupTarget = 3; // How many pushups until success
    public int pushupSpeed = 5; // How fast the slider travels
    public int pushupThreshold = 7500; // Where the player has to press the key. Don't set above 10000
    public Slider pushupSlider;
    public Slider pushupArea;
    private int pushupMax = 10000;
    private int curPushups = 0;

    // Start is called before the first frame update
    void Start()
    {
        gymControls = new Gym();

        gymControls.GymActions.Lift.started += ctx => Lift();
        gymControls.GymActions.Jab.started += ctx => Jab();
        gymControls.GymActions.Cross.started += ctx => Cross();
        gymControls.GymActions.Pushup.started += ctx => Pushup();

        // Randomize order of microgames
        int n = microgames.Length;
        System.Random rng = new System.Random();
        while (n > 1) 
        {
            int k = rng.Next(n--);
            string temp = microgames[n];
            microgames[n] = microgames[k];
            microgames[k] = temp;
        }

        currentMicrogame = StartCoroutine(microgames[0]);
        curGame = 0;
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

        gymControls.GymActions.Lift.Disable();
        liftSlider.gameObject.SetActive(false);

        liftSlider.value = liftAmount;
        if (liftAmount >= liftTarget)
        {
            Debug.Log("Succeeded at lifting!");

            curGame++;
            if (curGame < microgames.Length)
            {
                yield return new WaitForSeconds(timeBetweenMicrogames);
                currentMicrogame = StartCoroutine(microgames[curGame]);
            }
        }
        else
        {
            Debug.Log("Failed at lifting!");
        }
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
        NewPunch();

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

            curGame++;
            if (curGame < microgames.Length)
            {
                yield return new WaitForSeconds(timeBetweenMicrogames);
                currentMicrogame = StartCoroutine(microgames[curGame]);
            }
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
            LeftClick.SetActive(false);
            curPunches++;
            if (curPunches < punchTarget)
            {
                NewPunch();
            }
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
            RightClick.SetActive(false);
            curPunches++;
            if (curPunches < punchTarget)
            {
                NewPunch();
            }
        }
        else
        {
            PunchFail();
        }
    }

    void NewPunch()
    {
        punchSide = YarnFunctions.RandomRange(0, 1);
        Debug.Log((punchSide == 0) ? "Punch left!" : "Punch right!");

        if (punchSide == 0)
        {
            LeftClick.SetActive(true);
        }
        else
        {
            RightClick.SetActive(true);
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
    #region Pushup

    IEnumerator PushupMicrogame()
    {
        Debug.Log("Pushup microgame start!");

        // Initialize variables
        gymControls.GymActions.Pushup.Enable();
        curPushups = 0;
        pushupSlider.maxValue = pushupMax;
        pushupArea.maxValue = pushupMax;
        pushupArea.value = pushupMax - pushupThreshold;
        pushupSlider.gameObject.SetActive(true);
        pushupArea.gameObject.SetActive(true);

        // Repeat every frame until punch target is reached or time limit is exceeded
        while (curPushups < pushupTarget && pushupSlider.value < pushupMax)
        {
            pushupSlider.value += pushupSpeed;
            yield return null; // Wait a frame before repeating
        }
        
        pushupSlider.gameObject.SetActive(false);
        pushupArea.gameObject.SetActive(false);

        if (curPushups >= pushupTarget)
        {
            Debug.Log("Succeeded at pushups!");
            gymControls.GymActions.Pushup.Disable();

            curGame++;
            if (curGame < microgames.Length)
            {
                yield return new WaitForSeconds(timeBetweenMicrogames);
                currentMicrogame = StartCoroutine(microgames[curGame]);
            }
        }
        else
        {
            PushupFail();
        }
    }

    void Pushup()
    {
        if (pushupSlider.value >= pushupThreshold)
        {
            Debug.Log("Nice pushup!");
            pushupSlider.value = 0;
            curPushups++;
        }
        else
        {
            PushupFail();
        }
    }

    void PushupFail()
    {
        StopCoroutine(currentMicrogame);
        gymControls.GymActions.Pushup.Disable();
        
        pushupSlider.gameObject.SetActive(false);
        pushupArea.gameObject.SetActive(false);

        Debug.Log("Failed at pushups!");
    }

    #endregion
}
