using UnityEngine;
using UnityEngine.InputSystem;

public class CafeMinigame : MonoBehaviour
{
    public Cafe controls;

    public string[] currentOrder;       // An order is made up of several ingredient strings
    public string nextIngredient;       // The ingredient string that is expected next from the player
    private int i;                      // Iterates over the order
    private bool orderStarted = false;  // Ingredient inputs won't be recognized until order started (feel free to change this, just my interpretation)
    
    void Start()
    {
        if (controls == null)
        {
            controls = new Cafe();
            controls.Enable();

            controls.player.Start.performed += ctx => StartOrder();

            controls.player.Blend.performed += ctx => AddIngredient(ctx.action.name);
            controls.player.Shake.performed += ctx => AddIngredient(ctx.action.name);
            controls.player.Stir.performed += ctx => AddIngredient(ctx.action.name);
            controls.player.Ice.performed += ctx => AddIngredient(ctx.action.name);
            controls.player.Espresso.performed += ctx => AddIngredient(ctx.action.name);
            controls.player.Caramel.performed += ctx => AddIngredient(ctx.action.name);
            controls.player.Vanilla.performed += ctx => AddIngredient(ctx.action.name);
            controls.player.Peppermint.performed += ctx => AddIngredient(ctx.action.name);
            controls.player.Hazelnut.performed += ctx => AddIngredient(ctx.action.name);
            controls.player.Mocha.performed += ctx => AddIngredient(ctx.action.name);
            controls.player.BlackTea.performed += ctx => AddIngredient(ctx.action.name);
            controls.player.GreenTea.performed += ctx => AddIngredient(ctx.action.name);
            controls.player.ChaiTea.performed += ctx => AddIngredient(ctx.action.name);
            controls.player.PassionTea.performed += ctx => AddIngredient(ctx.action.name);
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
            controls.player.Hot.performed += ctx => AddIngredient(ctx.action.name);
            controls.player.Iced.performed += ctx => AddIngredient(ctx.action.name);
        }

        // Test code:
        NewOrder(new string[]{"Espresso", "Caramel", "Skim Milk", "Stir"});
    }

    public void NewOrder(string[] newOrder)
    {
        currentOrder = newOrder;
        i = 0;
        nextIngredient = currentOrder[i];
    }

    void StartOrder()
    {
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
            return;
        }

        // Debug.Log("Attempting to add " + attemptedIngredient);

        if (attemptedIngredient == nextIngredient)
        {
            Debug.Log("Successfully added " + attemptedIngredient + " to the order!");

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
            Debug.Log("Wrong ingredient!\nYou added: " + attemptedIngredient + "\tNeeded ingredient: " + nextIngredient);

            // Add stuff that happens if you get an ingredient wrong here
        }
    }
}
