using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; // for array of sprites
using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;

public class CafeMinigameController : MonoBehaviour
{
    public Cafe controls;

    public float time = 15f;
    public bool showControls = true;

    public static bool gameFinished = false;
    public static bool gameWon = false;

    public string[] currentOrder;       // An order is made up of several ingredient strings
    public string nextIngredient;       // The ingredient string that is expected next from the player
    private int i;                      // Iterates over the order
    private bool orderStarted = false;  // Ingredient inputs won't be recognized until order started (feel free to change this, just my interpretation)
    private int drinksFinished = 0;

    public Sprite[] Sprites;
    public Sprite[] FinishedDrinks;
    
    public Transform Blender;
    public TextMeshProUGUI Words;
    public TextMeshProUGUI Timer;
    
    // 2D array that is used for the order randomizer in CreateOrder()
    private string[][] layers = new string[][] {
        new string[] {"Almond Milk", "Whole Milk", "Oat Milk", "Skim Milk"},
        new string[] {"Espresso"},
        new string[] {"Creme Base", "Coffee Base"},
        new string[] {"Matcha"},
        new string[] {"Caramel", "Strawberry", "Hazelnut", "Mocha", "Peppermint", "Vanilla"}
    };

    void AddToBlender(string ingredient)
    {
        var result = Array.Find<Sprite>(Sprites, element => element.name == ingredient);

        switch(ingredient){
            case "Almond Milk":
            case "Whole Milk":
            case "Oat Milk":
            case "Skim Milk":
                Blender.GetChild(1).GetComponent<SpriteRenderer>().sprite = result;
            break;
            case "Espresso":
                Blender.GetChild(2).GetComponent<SpriteRenderer>().sprite = result;
            break;
            case "Creme Base":
            case "Coffee Base":
                Blender.GetChild(3).GetComponent<SpriteRenderer>().sprite = result;
            break;
            case "Matcha":
                Blender.GetChild(4).GetComponent<SpriteRenderer>().sprite = result;
            break;
            case "Ice":
                Blender.GetChild(5).GetComponent<SpriteRenderer>().sprite = result;
            break;
            case "Caramel":
            case "Strawberry":
            case "Hazelnut":
            case "Mocha":
            case "Peppermint":
            case "Vanilla": 
                Blender.GetChild(6).GetComponent<SpriteRenderer>().sprite = result;
            break;
            case "Blend":
                DOTween.Clear();
                Blender.GetChild(7).GetComponent<SpriteRenderer>().sprite = result;
                Blender.GetChild(7).GetComponent<SpriteRenderer>().color = Color.white;
                Blender.GetChild(7).GetComponent<SpriteRenderer>().DOColor(Color.clear,1f).SetDelay(1f);
            break;
        }
    }

    void FinishOrder()
    {
        orderStarted = false;

        //clear sprites from blender child...
        for(int i = 1; i < 7; i++)
        {
            Blender.GetChild(i).GetComponent<SpriteRenderer>().sprite = null;
        }

        // add cup and children
        Blender.GetChild(8).GetComponent<SpriteRenderer>().color = Color.white;
        var res = Array.Find<Sprite>(FinishedDrinks, element => element.name == currentOrder[currentOrder.Length-2]);
        Blender.GetChild(8).GetChild(0).GetComponent<SpriteRenderer>().sprite = res;

        drinksFinished++;
    }

    void Start()
    {
        time = 15f; // Hard-coding because I don't care
        gameFinished = false;
        Timer.text = time.ToString("0.00");

        if (controls != null)
        {
            return;
        }

        controls = new Cafe();
        controls.Enable();

        controls.player.Start.performed += ctx => StartOrder();

        controls.player.Blend.performed += ctx => AddIngredient(ctx.action.name);
        controls.player.Ice.performed += ctx => AddIngredient(ctx.action.name);
        controls.player.Espresso.performed += ctx => AddIngredient(ctx.action.name);
        controls.player.Caramel.performed += ctx => AddIngredient(ctx.action.name);
        controls.player.Vanilla.performed += ctx => AddIngredient(ctx.action.name);
        controls.player.Peppermint.performed += ctx => AddIngredient(ctx.action.name);
        controls.player.Hazelnut.performed += ctx => AddIngredient(ctx.action.name);
        controls.player.Mocha.performed += ctx => AddIngredient(ctx.action.name);
        controls.player.Strawberry.performed += ctx => AddIngredient(ctx.action.name);
        controls.player.Matcha.performed += ctx => AddIngredient(ctx.action.name);
        controls.player.CremeBase.performed += ctx => AddIngredient(ctx.action.name);
        controls.player.CoffeeBase.performed += ctx => AddIngredient(ctx.action.name);
        controls.player.WholeMilk.performed += ctx => AddIngredient(ctx.action.name);
        controls.player.SkimMilk.performed += ctx => AddIngredient(ctx.action.name);
        controls.player.OatMilk.performed += ctx => AddIngredient(ctx.action.name);
        controls.player.AlmondMilk.performed += ctx => AddIngredient(ctx.action.name);
    }

    public void NewOrder(string[] newOrder)
    {
        currentOrder = newOrder;
        i = 0;
        nextIngredient = currentOrder[i];
    }

    private string[] CreateOrder()
    {
        // Dynamic array
        List<string> order = new List<string>();

        // Milk
        int rnd = YarnFunctions.RandomRange(0,layers[0].Length-1);
        order.Add(layers[0][rnd]);

        // Espresso (Optional)
        rnd = YarnFunctions.RandomRange(0,layers[1].Length);
        if (rnd < layers[1].Length) order.Add(layers[1][rnd]);

        // Base
        rnd = YarnFunctions.RandomRange(0,layers[2].Length-1);
        order.Add(layers[2][rnd]);

        // Matcha Powder (Optional)
        rnd = YarnFunctions.RandomRange(0,layers[3].Length);
        if (rnd < layers[3].Length) order.Add(layers[3][rnd]);

        // Ice
        order.Add("Ice");

        // Topping
        rnd = YarnFunctions.RandomRange(0,layers[4].Length-1);
        order.Add(layers[4][rnd]);

        // Blend
        order.Add("Blend");

        return order.ToArray();
    }    

    void StartOrder()
    {
        // Make sure there's an order to start
        if (orderStarted) 
        { 
            return;
        }
        orderStarted = true;

        // Create random order and pass it to NewOrder()
        NewOrder(CreateOrder());

        // Show START art
        DOTween.Clear();
        Blender.GetChild(7).GetComponent<SpriteRenderer>().sprite = Sprites[16];
        Blender.GetChild(7).GetComponent<SpriteRenderer>().color = Color.white;
        Blender.GetChild(7).GetComponent<SpriteRenderer>().DOColor(Color.clear,1f).SetDelay(1f);

        // Remove cup and children
        Blender.GetChild(8).GetComponent<SpriteRenderer>().color = Color.clear;
        Blender.GetChild(8).GetChild(0).GetComponent<SpriteRenderer>().sprite = null;

        Words.text = "Add " + currentOrder[0];
        if (showControls)
        {
            Words.text += " (" + controls.FindAction(currentOrder[i]).GetBindingDisplayString(0) + ")";
        }
    }

    void AddIngredient(string attemptedIngredient)
    {
        // If the order isn't started, nothing should happen when we try to add an ingredient
        if (!orderStarted)
        {
            // Debug.Log("Order not started! Press SPACE");
            return;
        }

        // Debug.Log("Attempting to add " + attemptedIngredient);

        if (attemptedIngredient == nextIngredient)
        {
            // Debug.Log("Successfully added " + attemptedIngredient + " to the order!");

            // Add stuff that happens when an ingredient is correct here
            AddToBlender(attemptedIngredient);
  
            i++;
            if (i < currentOrder.Length)
            {
                nextIngredient = currentOrder[i];
                Words.text = "Add " + currentOrder[i];
                if (showControls)
                {
                    Words.text += " (" + controls.FindAction(currentOrder[i]).GetBindingDisplayString(0) + ")";
                }
            }
            else
            {
                // Add stuff that happens when you successfully finish an order here
                FinishOrder();
                Words.text = "START (Press space)";

                // Debug.Log("Order finished!");
                orderStarted = false;
                currentOrder = null;
            }
        }
        else
        {
            Debug.Log("Wrong ingredient!\tYou added: " + attemptedIngredient + "\tNeeded ingredient: " + nextIngredient);

            // Add stuff that happens if you get an ingredient wrong here
            //clear sprites from blender child...
            for(int i = 1; i < 7; i++)
            {
                Blender.GetChild(i).GetComponent<SpriteRenderer>().sprite = null;
            }
            
            i = 0;
            if (i < currentOrder.Length)
            {
                nextIngredient = currentOrder[i];
                Words.text = "Add " + currentOrder[i];
                if (showControls)
                {
                    Words.text += " (" + controls.FindAction(currentOrder[i]).GetBindingDisplayString(0) + ")";
                }
            }
        }
    }

    private void Update()
    {
        if (!orderStarted)
        {
            return;
        }

        time -= Time.deltaTime;
        Timer.text = time.ToString("0.00");

        if (time <= 0f)
        {
            Timer.text = "0.00";
            controls.player.Disable();
            orderStarted = false;
            CalculateMoney();
        }
        else if (time < 5f)
        {
            Timer.color = Color.red;
        }
    }

    private void CalculateMoney()
    {
        if (drinksFinished >= 2)
        {
            Inventory.ChangeMoney(100 + (drinksFinished-2)*100);
            gameWon = true;
        }

        DOTween.Clear();
        gameFinished = true;
    }
}
