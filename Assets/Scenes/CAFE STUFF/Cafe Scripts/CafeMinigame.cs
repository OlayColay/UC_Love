using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; // for array of sprites
using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;

public class CafeMinigame : MonoBehaviour
{
    public Cafe controls;

    public string[] currentOrder;       // An order is made up of several ingredient strings
    public string nextIngredient;       // The ingredient string that is expected next from the player
    private int i;                      // Iterates over the order
    private bool orderStarted = false;  // Ingredient inputs won't be recognized until order started (feel free to change this, just my interpretation)

    // Make orders a global 
    //public array of sprites called "images" - go back into unity and assign
    //CO-routine for blend - inside coroutine wait 1 second then do fade

    public Sprite[] Sprites;
    public Sprite[] FinishedDrinks;
    
    public Transform Blender;
    public TextMeshProUGUI Words;
    
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
                Blender.GetChild(7).GetComponent<SpriteRenderer>().sprite = result;
                Blender.GetChild(7).GetComponent<SpriteRenderer>().color = Color.white;
                Blender.GetChild(7).GetComponent<SpriteRenderer>().DOColor(Color.clear,2.5f);
            break;
        }
    }

    void FinishOrder()
    {
        //clear sprites from blender child...
        for(int i = 1; i < 7; i++)
        {
            Blender.GetChild(i).GetComponent<SpriteRenderer>().sprite = null;
        }

        // add cup and children
        Blender.GetChild(8).GetComponent<SpriteRenderer>().color = Color.white;
        Debug.Log(currentOrder);
        var res = Array.Find<Sprite>(FinishedDrinks, element => element.name == currentOrder[currentOrder.Length-2]);
        Blender.GetChild(8).GetChild(0).GetComponent<SpriteRenderer>().sprite = res;
    }

    void Start()
    {
        if (controls == null)
        {
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
            controls.player.WhippedCream.performed += ctx => AddIngredient(ctx.action.name);
            controls.player.CremeBase.performed += ctx => AddIngredient(ctx.action.name);
            controls.player.CoffeeBase.performed += ctx => AddIngredient(ctx.action.name);
            controls.player.WholeMilk.performed += ctx => AddIngredient(ctx.action.name);
            controls.player.SkimMilk.performed += ctx => AddIngredient(ctx.action.name);
            controls.player.OatMilk.performed += ctx => AddIngredient(ctx.action.name);
            controls.player.AlmondMilk.performed += ctx => AddIngredient(ctx.action.name);
            controls.player.JavaChips.performed += ctx => AddIngredient(ctx.action.name);
            controls.player.Sprinkles.performed += ctx => AddIngredient(ctx.action.name);
            controls.player.Cinnamon.performed += ctx => AddIngredient(ctx.action.name);
            controls.player.CaramelDrizzle.performed += ctx => AddIngredient(ctx.action.name);
            controls.player.Cherry.performed += ctx => AddIngredient(ctx.action.name);
            controls.player.Frappucino.performed += ctx => AddIngredient(ctx.action.name);
        }

        // Create random order and pass it to NewOrder()
        NewOrder(CreateOrder());
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
        Blender.GetChild(7).GetComponent<SpriteRenderer>().sprite = Sprites[16];
        Blender.GetChild(7).GetComponent<SpriteRenderer>().color = Color.white;
        Blender.GetChild(7).GetComponent<SpriteRenderer>().DOColor(Color.clear,2.5f);
        // Make sure there's an order to start
        if (currentOrder == null || currentOrder.Length == 0)
        {
            return; 
        }

        Debug.Log("Starting order!");
        orderStarted = true;

        Words.text = "Add " + currentOrder[0];

        // Add other stuff that happens when you start an order here
    }

    void AddIngredient(string attemptedIngredient)
    {
        // If the order isn't started, nothing should happen when we try to add an ingredient
        if (!orderStarted)
        {
            Debug.Log("Order not started! Press SPACE");
            return;
        }

        // Debug.Log("Attempting to add " + attemptedIngredient);

        if (attemptedIngredient == nextIngredient)
        {
            Debug.Log("Successfully added " + attemptedIngredient + " to the order!");

            // Add stuff that happens when an ingredient is correct here
            AddToBlender(attemptedIngredient);
  
            i++;
            if (i < currentOrder.Length)
            {
                nextIngredient = currentOrder[i];
                Words.text = "Add " + currentOrder[i];
            }
            else
            {
                // Add stuff that happens when you successfully finish an order here
                FinishOrder();

                Debug.Log("Order finished!");
                orderStarted = false;
                currentOrder = null;
            }
        }
        else
        {
            Debug.Log("Wrong ingredient!\tYou added: " + attemptedIngredient + "\tNeeded ingredient: " + nextIngredient);

            // Add stuff that happens if you get an ingredient wrong here
        }
    }
}
