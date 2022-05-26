using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GymMinigameController : MonoBehaviour
{
    public string[] microgames = new string[3] {"LiftMicrogame", "PunchMicrogame", "PushupMicrogame"};
    public float timeBetweenMicrogames = 1f;
    public static bool minigameDone = false;
    public static bool minigameWon = false;
    public AudioSource audioSource;

    private Gym gymControls;
    private Coroutine currentMicrogame;
    private int curGame = 0;

    // Lift microgame variables
    public int liftTarget = 1000;
    public int liftAmount = 0;
    public int liftGainPerPress = 50;
    public int liftLossPerFrame = 1;
    public float liftTime = 5f;
    public Slider liftSlider;
    public Slider handleSlider;

    // Punch microgame variables
    public int punchTarget = 3; // How many punches until success
    public float punchTime = 3f;
    public GameObject LeftClick;
    public GameObject RightClick;
    public AudioClip[] punchSFX;
    private int punchSide; // 0 is left, 1 is right
    private int curPunches = 0;

    // Pushup microgame variables
    public int pushupTarget = 3; // How many pushups until success
    public int pushupSpeed = 5; // How fast the slider travels
    public int pushupThreshold = 750; // Where the player has to press the key. Don't set above 10000. The higher it is, the harder it is to time
    public Slider pushupSlider;
    public Slider pushupArea;
    [SerializeField] private int pushupMax = 1000;
    private int curPushups = 0;

    // Art
    public Image background;
    public Image pushupBackground;
    public Image punchingBag;
    public Sprite[] punchingBagSprites;
    public Image weight1;
    public Image weight2;
    public Image keyboard;
    public Image handle;

    /// <summary> Constructor of the Gym minigame where difficulties are set <summary>
    public void NewGymMinigame(int liftGainPerPress, int punchTarget, float punchTime, int pushupSpeed, int pushupThreshold)
    {
        this.liftGainPerPress = liftGainPerPress;
        this.punchTarget = punchTarget;
        this.punchTime = punchTime;
        this.pushupSpeed = pushupSpeed;
        this.pushupThreshold = pushupThreshold;

        minigameDone = false;
        minigameWon = false;

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

        background.DOFade(1f, 1f);
        currentMicrogame = StartCoroutine(microgames[0]);
        curGame = 0;
    }

    IEnumerator MicrogameWon()
    {
        curGame++;
        if (curGame < microgames.Length)
        {
            yield return new WaitForSeconds(timeBetweenMicrogames);
            currentMicrogame = StartCoroutine(microgames[curGame]);
        }
        else
        {
            // Debug.Log("You win!");
            GainMoney();
            background.DOFade(0f, 1f).OnComplete( () => minigameDone = true);
            minigameWon = true;
        }
    }

    private void GainMoney()
    {
        int liftMoney = 5 * (liftTarget + 100*liftLossPerFrame - 50*(int)liftTime)/liftGainPerPress;
        int punchMoney = (int)(100 * (punchTarget/punchTime));
        int pushupMoney = (int)(pushupTarget*pushupSpeed / (5*(float)(pushupMax - pushupThreshold)/pushupMax));
        Inventory.ChangeMoney(liftMoney + punchMoney + pushupMoney);
        // Debug.Log("Money gained: " + liftMoney + ' ' + punchMoney + ' ' + pushupMoney);
    }

    #region Lift

    IEnumerator LiftMicrogame()
    {
        // Debug.Log("Lifting microgame start!");
        weight1.DOFade(1f, 1f);
        weight2.DOFade(1f, 1f);
        handle.DOFade(1f, 1f);
        yield return new WaitForSecondsRealtime(1f);
        keyboard.enabled = true;

        // Initialize variables
        gymControls.GymActions.Lift.Enable();
        liftAmount = 0;
        float currentTime = 0f;
        liftSlider.maxValue = liftTarget;
        handleSlider.maxValue = liftTarget;

        // Repeat every frame until lift target is reached or time limit is exceeded
        while (liftAmount < liftTarget && currentTime < liftTime)
        {
            liftAmount = Math.Max(liftAmount - liftLossPerFrame, 0);
            liftSlider.value = liftAmount;
            handleSlider.value = liftAmount;
            currentTime += Time.deltaTime;
            yield return new WaitForFixedUpdate(); // Wait a frame before repeating
        }

        gymControls.GymActions.Lift.Disable();

        liftSlider.value = liftAmount;
        handleSlider.value = liftAmount;
        if (liftAmount >= liftTarget)
        {
            // Debug.Log("Succeeded at lifting!");
            weight1.DOFade(0f, 1f);
            weight2.DOFade(0f, 1f);
            handle.DOFade(0f, 1f);
            keyboard.enabled = false;

            StartCoroutine(MicrogameWon());
        }
        else
        {
            // Debug.Log("Failed at lifting!");

            background.DOFade(0f, 1.5f).OnComplete( () => minigameDone = true);
            minigameWon = false;
        }

        yield return new WaitForSecondsRealtime(1f);
    }

    void Lift()
    {
        // Debug.Log("Heave!");
        liftAmount += liftGainPerPress;
    }

    #endregion
    #region Punch

    IEnumerator PunchMicrogame()
    {
        // Debug.Log("Punch microgame start!");
        punchingBag.DOFade(1f, 1f);
        yield return new WaitForSecondsRealtime(1f);

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
            yield return new WaitForFixedUpdate(); // Wait a frame before repeating
        }

        if (curPunches >= punchTarget)
        {
            audioSource.PlayOneShot(punchSFX[1]);

            // Debug.Log("Succeeded at punching!");
            gymControls.GymActions.Jab.Disable();
            gymControls.GymActions.Cross.Disable();
            
            punchingBag.DOFade(0f, 1f);

            StartCoroutine("MicrogameWon");
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
            punchingBag.sprite = punchingBagSprites[2];

            LeftClick.SetActive(false);
            curPunches++;
            if (curPunches < punchTarget)
            {
                audioSource.PlayOneShot(punchSFX[0]);
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
            punchingBag.sprite = punchingBagSprites[1];

            RightClick.SetActive(false);
            curPunches++;
            if (curPunches < punchTarget)
            {
                audioSource.PlayOneShot(punchSFX[0]);
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
        // Debug.Log((punchSide == 0) ? "Punch left!" : "Punch right!");

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

        // Debug.Log("Failed at punching!");

        background.DOFade(0f, 1f).OnComplete( () => minigameDone = true);
        minigameWon = false;
    }

    #endregion
    #region Pushup

    IEnumerator PushupMicrogame()
    {
        // Debug.Log("Pushup microgame start!");
        pushupBackground.DOFade(1f, 1f);
        yield return new WaitForSecondsRealtime(1f);

        // Initialize variables
        gymControls.GymActions.Pushup.Enable();
        curPushups = 0;
        pushupSlider.maxValue = pushupMax;
        pushupArea.maxValue = pushupMax;
        pushupArea.value = pushupMax - pushupThreshold;
        pushupSlider.gameObject.SetActive(true);
        pushupArea.gameObject.SetActive(true);

        pushupBackground.transform.DOScale(2f, 10f);

        // Repeat every frame until pushup target is reached or time limit is exceeded
        while(curPushups < pushupTarget && pushupSlider.value < pushupMax)
        {
            pushupSlider.value += pushupSpeed;
            keyboard.enabled = (pushupSlider.value >= pushupThreshold);
            yield return new WaitForFixedUpdate(); // Wait a frame before repeating
        }
        
        pushupSlider.gameObject.SetActive(false);
        pushupArea.gameObject.SetActive(false);
        keyboard.enabled = false;

        if (curPushups >= pushupTarget)
        {
            // Debug.Log("Succeeded at pushups!");
            gymControls.GymActions.Pushup.Disable();
            pushupBackground.DOFade(0f, 1f);
            yield return new WaitForSecondsRealtime(1f);

            StartCoroutine(MicrogameWon());
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
            // Debug.Log("Nice pushup!");
            pushupSlider.value = 0;
            curPushups++;

            DOTween.Clear();
            pushupBackground.transform.DOScale(1f, 0.5f).OnComplete(() => pushupBackground.transform.DOScale(2f, 10f));
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

        // Debug.Log("Failed at pushups!");

        background.DOFade(0f, 1f).OnComplete( () => minigameDone = true);
        minigameWon = false;
    }

    #endregion
}
