using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meal : MonoBehaviour
{
    public bool foodPoison = false;
    public bool drinkPoison = false;
    public main Main;
    // Start is called before the first frame update
    void Start()
    {
        foodPoison = Random.Range(0f, 1f) < 0.25f;
        drinkPoison = Random.Range(0f, 1f) < 0.25f;

        Main = GameObject.Find("Game Manager").GetComponent<main>();


        int rn = -1;
        bool correct = false;
        while (!correct)
        {
            rn = Random.Range(0, Main.sauces.Length);
            bool p = Main.poisonSauces[rn];
            correct = (p == foodPoison);
        }
        transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().sprite = Main.sauces[rn];

        rn = -1;
        correct = false;
        while (!correct) {
            rn = Random.Range(0, Main.drinks.Length);
            bool p = Main.poisonDrinks[rn];
            correct = (p == drinkPoison);
        }
        transform.GetChild(2).GetChild(0).GetComponent<SpriteRenderer>().color = Main.drinks[rn];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
