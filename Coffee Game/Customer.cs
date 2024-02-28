using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public string wantedDrink;
    public bool recievedDrink;
    public TMP_Text orderText;
    int drinkNum;
    public CustomerSpawner spawner;
    drinks drink;
    SCR_CoffeeMaker player;

    int drinkID;
    public List<Ingredients.Ingredient> drinkWanted = new List<Ingredients.Ingredient>();
    void Start()
    {

    }

    enum drinks
    {
        //name of drinks
        Espresso,
        Creamed_Espresso,
        Creamed_Americano,
        Creamed_Caramel_Americano,
        Americano,
        Creamed_Americano_with_Caramel_Drizzle,
        Caramel_Americano,
        Creamed_Espresso_with_Caramel_Drizzle,
        Caramel_Espresso,
        Hot_Chocolate
    }

    private void OnEnable()
    {
        drink = (drinks)Random.Range(0, 10);
        drinkID = (int)drink;


        switch (drinkID)
        {
            //This is a temporary way i've been storing the potential coffees a customer might want, i've decided to change this soon to use scriptable objects instead so that it's modular
            case 0:
                drinkWanted.Add(Ingredients.Ingredient.ground_coffee);
                drinkWanted.Add(Ingredients.Ingredient.boiling_water);
                break;
            case 1:
                drinkWanted.Add(Ingredients.Ingredient.ground_coffee);
                drinkWanted.Add(Ingredients.Ingredient.boiling_water);
                drinkWanted.Add(Ingredients.Ingredient.cream);
                break;
            case 2:
                drinkWanted.Add(Ingredients.Ingredient.ground_coffee);
                drinkWanted.Add(Ingredients.Ingredient.boiling_water);
                drinkWanted.Add(Ingredients.Ingredient.milk);
                drinkWanted.Add(Ingredients.Ingredient.cream);
                break;
            case 3:
                drinkWanted.Add(Ingredients.Ingredient.ground_coffee);
                drinkWanted.Add(Ingredients.Ingredient.caramel_syrup);
                drinkWanted.Add(Ingredients.Ingredient.boiling_water);
                drinkWanted.Add(Ingredients.Ingredient.milk);
                drinkWanted.Add(Ingredients.Ingredient.cream);
                break;
            case 4:
                drinkWanted.Add(Ingredients.Ingredient.ground_coffee);
                drinkWanted.Add(Ingredients.Ingredient.boiling_water);
                drinkWanted.Add(Ingredients.Ingredient.milk);
                break;
            case 5:
                drinkWanted.Add(Ingredients.Ingredient.ground_coffee);
                drinkWanted.Add(Ingredients.Ingredient.boiling_water);
                drinkWanted.Add(Ingredients.Ingredient.milk);
                drinkWanted.Add(Ingredients.Ingredient.cream);
                drinkWanted.Add(Ingredients.Ingredient.caramel_syrup);
                break;
            case 6:
                drinkWanted.Add(Ingredients.Ingredient.ground_coffee);
                drinkWanted.Add(Ingredients.Ingredient.caramel_syrup);
                drinkWanted.Add(Ingredients.Ingredient.boiling_water);
                drinkWanted.Add(Ingredients.Ingredient.milk);
                break;
            case 7:
                drinkWanted.Add(Ingredients.Ingredient.ground_coffee);
                drinkWanted.Add(Ingredients.Ingredient.boiling_water);
                drinkWanted.Add(Ingredients.Ingredient.cream);
                drinkWanted.Add(Ingredients.Ingredient.caramel_syrup);
                break;
            case 8:
                drinkWanted.Add(Ingredients.Ingredient.ground_coffee);
                drinkWanted.Add(Ingredients.Ingredient.caramel_syrup);
                drinkWanted.Add(Ingredients.Ingredient.boiling_water);
                break;
            case 9:
                drinkWanted.Add(Ingredients.Ingredient.chocolate_powder);
                drinkWanted.Add(Ingredients.Ingredient.boiling_water);
                drinkWanted.Add(Ingredients.Ingredient.milk);
                drinkWanted.Add(Ingredients.Ingredient.cream);
                drinkWanted.Add(Ingredients.Ingredient.chocolate_powder);
                break;




        }

        void OnDisable()
        {
            drinkWanted.Clear();
        }
      


        
    }

    public void RecieveDrink(List<Ingredients.Ingredient> heldDrink)
    {
        if(heldDrink.Count == drinkWanted.Count)
        {
            if(drinksAreSame(heldDrink, drinkWanted))
            {
                recievedDrink = true;
                player.RemoveDrink();
                Destroy(gameObject);

            }
            else
            {
                Debug.Log("Wrong Drink");
            }
        }
        else
        {
            Debug.Log("Wrong Drink (Not same amount of ingredients");

        }
    }

    bool drinksAreSame(List<Ingredients.Ingredient> A, List<Ingredients.Ingredient> B)
    {
        //Checks each ingredient to make sure they're the same.
        for (int i = 0; i < A.Count; i++)
        {
            if (A[i] != B[i])
            {
                //If ingredients aren't right, it returns false
                return false;
            }
        }
        //Otherwise it returns true
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        orderText.text = drink.ToString();
        //Sets the text above my head to be the drink I want
        if (recievedDrink)
        {
            //Destroys me when i have my drink
            Destroy(gameObject);
        }

       
            
    }
}
