using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; // for array of sprites
using System;
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
    public Transform Blender;
    public TextMeshProUGUI Words;
    

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
            case "Matcha Powder":
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
    void Start()
    {
        if (controls == null)
        {
            controls = new Cafe();
            controls.Enable();

            controls.player.Start.performed += ctx => StartOrder();

            controls.player.Blend.performed += ctx => AddIngredient(ctx.action.name);
//            controls.player.Shake.performed += ctx => AddIngredient(ctx.action.name);
//            controls.player.Stir.performed += ctx => AddIngredient(ctx.action.name);
            controls.player.Ice.performed += ctx => AddIngredient(ctx.action.name);
            controls.player.Espresso.performed += ctx => AddIngredient(ctx.action.name);
            controls.player.Caramel.performed += ctx => AddIngredient(ctx.action.name);
            controls.player.Vanilla.performed += ctx => AddIngredient(ctx.action.name);
            controls.player.Peppermint.performed += ctx => AddIngredient(ctx.action.name);
            controls.player.Hazelnut.performed += ctx => AddIngredient(ctx.action.name);
            controls.player.Mocha.performed += ctx => AddIngredient(ctx.action.name);
//            controls.player.BlackTea.performed += ctx => AddIngredient(ctx.action.name);
//            controls.player.GreenTea.performed += ctx => AddIngredient(ctx.action.name);
//            controls.player.ChaiTea.performed += ctx => AddIngredient(ctx.action.name);
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
     //       controls.player.Hot.performed += ctx => AddIngredient(ctx.action.name);
    //        controls.player.Iced.performed += ctx => AddIngredient(ctx.action.name);
        }

        // Test code:
        NewOrder(new string[]{"Espresso", "Caramel", "Skim Milk", "Blend"});
    }

    public void NewOrder(string[] newOrder)
    {
        currentOrder = newOrder;
        i = 0;
        nextIngredient = currentOrder[i];
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
            AddToBlender(attemptedIngredient);
            // Add stuff that happens when an ingredient is correct here

            i++;
            if (i < currentOrder.Length)
            {
                nextIngredient = currentOrder[i];
            }
            else
            {
                Debug.Log("Order finished!");
                orderStarted = false;
                currentOrder = null;

                // Add stuff that happens when you successfully finish an order here
            }
        }
        else
        {
            Debug.Log("Wrong ingredient!\tYou added: " + attemptedIngredient + "\tNeeded ingredient: " + nextIngredient);

            // Add stuff that happens if you get an ingredient wrong here
        }
    }
}
