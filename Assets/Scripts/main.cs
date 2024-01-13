using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main : MonoBehaviour
{
    public meal mealPrefab;
    public float Timer = 0;
    public bool listUp = false;
    private float listY = 2.5f;
    public list listPrefab;
    public GameObject list;
    public GameObject meals;
    public meal[] meallist;
    public bool alive = true;
    public int mealOn = 0;
    public float mealsX = 0;
    public bool canClick = true;


    public Sprite[] sauces;
    public Color[] drinks;

    public int numPoisonSauces = 2;
    public int numPoisonDrinks = 2;

    public bool[] poisonSauces;
    public bool[] poisonDrinks;
    // Start is called before the first frame update
    void Start()
    {
        poisonSauces = new bool[sauces.Length];
        int poisonCount = 0;
        for (int i = 0; i < poisonSauces.Length; i++) {
            poisonSauces[i] = Random.Range(0f, 1f) < (2 - poisonCount) / (poisonSauces.Length - i + 0.0f);
            if (poisonSauces[i]) poisonCount++;
        }

        poisonDrinks = new bool[drinks.Length];
        poisonCount = 0;
        for (int i = 0; i < poisonDrinks.Length; i++) {
            poisonDrinks[i] = Random.Range(0f, 1f) < (2 - poisonCount) / (poisonDrinks.Length - i+0.0f);
            if (poisonDrinks[i]) poisonCount++;
        }


        int Length = 5;
        meallist = new meal[Length];
        for (int i = 0; i < Length; i++)
        {
            meal m = meallist[i] = Instantiate<meal>(mealPrefab);
            m.transform.SetParent(meals.transform, false);
            m.name = "Meal " + i;
            m.transform.localPosition += Vector3.right * 4 * i;
        }


        list l = Instantiate<list>(listPrefab);
        l.name = "List";
        list = GameObject.Find("List");


    }

    // Update is called once per frame
    void Update()
    {
        listY += (listUp ? -1 : 1) * 10 * Time.deltaTime;
        listY = Mathf.Min(Mathf.Max(listY, 0), 2.5f);
        list.transform.position = new Vector3(0,-listY,-0.5f);

        if (!canClick) {
            mealsX += 0.5f * Time.deltaTime;
            if (mealsX > mealOn) {
                mealsX = mealOn;
                canClick = true;
            }
            meals.transform.position = new Vector3(mealsX * -4, 0, 0);
        }
    }

    public void onListClick()
    {
        if (canClick) {
            Debug.Log("List");
            listUp = !listUp;
        }
    }
    public void onSafeClick()
    {
        if (canClick){
            Debug.Log("Safe");
            nextMeal();
        }
    }
    public void onPoisonClick()
    {
        if (canClick) {
            Debug.Log("Poison");
            nextMeal();
        }
    }

    public void nextMeal()
    {
        mealOn++;
        canClick = false;
    }

}
