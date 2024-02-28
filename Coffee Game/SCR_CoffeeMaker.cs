using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_CoffeeMaker : MonoBehaviour
{
    public RaycastHit hit;
    Ingredients ingredient;
    public GameObject cup;
    public GameObject cupFull;
    bool emptyCupInHand;
    bool fullCupInHand;

    public List<Ingredients.Ingredient> drinkInHand = new List<Ingredients.Ingredient>();

    void Start()
    {
        emptyCupInHand = false;
        fullCupInHand = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if ((Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 8)))
            {
                if(hit.transform.gameObject.tag == "Customer")
                {
                    //Give the customer a drink
                    hit.transform.gameObject.GetComponent<Customer>().RecieveDrink(drinkInHand);
                }
                else if((hit.transform.gameObject.tag == "Cup") && !emptyCupInHand && !fullCupInHand)
                {
                    cup.SetActive(true);
                    emptyCupInHand = true;
                    //pick up a cup
                }
                else if(hit.transform.gameObject.tag == "Interact")
                {
                    //If the raycast hit is an interactive object (Ingredient), add that to our coffee
                    ingredient = hit.transform.gameObject.GetComponent<Ingredients>();
                    fullCupInHand=true;
                    AddIngredient(ingredient);
                }
                else if(hit.transform.gameObject.tag == "Bin")
                {
                    //Throw away drink
                    RemoveDrink();
                }
                
            }
            
            if(drinkInHand.Count > 0)
            {
                //If we have stuff in cup make it look correct
                emptyCupInHand = false;
                fullCupInHand = true;
                cup.SetActive(false);
                cupFull.SetActive(true);
            }

        }
    }


    void AddIngredient(Ingredients ingredientToAdd)
    {
        if((emptyCupInHand) || (fullCupInHand))
        {
            //Add the ingredient we click on
            Debug.Log(ingredientToAdd.ingredient);
            drinkInHand.Add(ingredientToAdd.ingredient);
        }
    }

    public void RemoveDrink()
    {
        //Get rid of drink in hand
        cup.SetActive(false);
        cupFull.SetActive(false);
        fullCupInHand = false;
        emptyCupInHand = false;
        drinkInHand.Clear();

    }
}
