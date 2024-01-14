using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class main : MonoBehaviour
{
    public meal mealPrefab;
    public float Timer = 0;
    public bool listUp = false;
    private float listY = 2.5f;
    public list listPrefab;
    public GameObject list;
    public GameObject meals;
    public GameObject black;
    public meal[] meallist;
    public bool alive = true;
    public int mealOn = 0;
    public float mealsX = 0;

    int Length;
    public Sprite[] sauces;
    public Color[] drinks;

    public int numPoisonSauces = 2;
    public int numPoisonDrinks = 2;

    public bool[] poisonSauces;
    public bool[] poisonDrinks;

    public int[] results;
    public int[] lresults;
    public int health = 5;
    public GameObject[] buttons;

    int day = 0;
    // Start is called before the first frame update
    void Start()
    {
        Length = 2;
        results = new int[4];
        lresults = new int[4];

        nextDay();
    }

    // Update is called once per frame
    void Update()
    {
        list = GameObject.Find("List");
        listY += (listUp ? -1 : 1) * 10 * Time.deltaTime;
        listY = Mathf.Min(Mathf.Max(listY, 0), 2.5f);
        list.transform.position = new Vector3(0,-listY,-0.5f);

        if (Timer < 60) { 
            mealsX += 0.5f * Time.deltaTime;
            if (mealsX > mealOn) {
                mealsX = mealOn;
                if (mealOn < Length) enableButtons(listY!=2.5f? 1:2);
            }
            meals.transform.position = new Vector3(mealsX * -4, 0, 0);
        }

        Timer -= Time.deltaTime;
        if (Timer < 60) {
            black.SetActive(false);
            GameObject.Find("Timer").GetComponent<TMPro.TextMeshProUGUI>().text = "Time Left: " + (int)Timer;
        }  else {
            GameObject.Find("Timer").GetComponent<TMPro.TextMeshProUGUI>().text = "";
        }
        
        if (Timer<0) {
            black.SetActive(true);
            black.transform.GetChild(0).GetComponent<TMPro.TextMeshPro>().text = "Fired";
            black.transform.GetChild(1).GetComponent<TMPro.TextMeshPro>().text = lresults[0] + lresults[1] + " happy citizens, " + lresults[2] + " got poisoned and " + lresults[3] + " went hungry";
            GameObject.Find("Timer").GetComponent<TMPro.TextMeshProUGUI>().text = "";
            enableButtons(0);
        }
    }


    public void randomisePoisons() {
        poisonSauces = new bool[sauces.Length];
        int poisonCount = 0;
        for (int i = 0; i < poisonSauces.Length; i++) {
            poisonSauces[i] = Random.Range(0f, 1f) < (2 - poisonCount) / (poisonSauces.Length - i + 0.0f);
            if (poisonSauces[i]) poisonCount++;
        }

        poisonDrinks = new bool[drinks.Length];
        poisonCount = 0;
        for (int i = 0; i < poisonDrinks.Length; i++) {
            poisonDrinks[i] = Random.Range(0f, 1f) < (2 - poisonCount) / (poisonDrinks.Length - i + 0.0f);
            if (poisonDrinks[i]) poisonCount++;
        }
    }

    public void generateMeals() {
        meallist = new meal[Length];
        for (int i = 0; i < Length; i++) {
            meal m = meallist[i] = Instantiate<meal>(mealPrefab);
            m.transform.SetParent(meals.transform, false);
            m.name = "Meal " + i;
            m.transform.localPosition += Vector3.right * 4 * i;
        }
        GameObject.Find("Canvas2").transform.localPosition = Vector3.right * 4 * Length;
    }

    public void generateList() {
        if (list != null) Destroy(list.gameObject);
        list l = Instantiate<list>(listPrefab);
        l.name = "List";
    }


    public void onListClick() {
        listUp = !listUp;
    }
    public void onSafeClick() {
        if (meallist[mealOn].foodPoison || meallist[mealOn].drinkPoison) results[2]++; else results[0]++;
        nextMeal();
    }
    public void onPoisonClick() {
        if (meallist[mealOn].foodPoison || meallist[mealOn].drinkPoison) results[1]++; else results[3]++;
        nextMeal();
    }

    public void onExitClick() {
        SceneManager.LoadScene(0);
    }

    public void onNextClick()
    {
        if (mealOn == Length) nextDay();
    }

    public void nextMeal() {
        if (mealOn < Length) {
            mealOn++;
            enableButtons(0);
        }
    }

    public void enableButtons(int b)
    {
        buttons[0].SetActive(b>1);
        buttons[1].SetActive(b>1);
        buttons[2].SetActive(b>0);
    }


    public void nextDay() {

        if (lresults[2] + lresults[3] > 4) {
            black.SetActive(true);
            black.transform.GetChild(0).GetComponent<TMPro.TextMeshPro>().text = "Fired";
            black.transform.GetChild(1).GetComponent<TMPro.TextMeshPro>().text = lresults[0] + lresults[1] + " happy citizens, " + lresults[2] + " got poisoned and " + lresults[3] + " went hungry";
            enableButtons(0);
            Timer = 1000000;

        } else {
            day++;
            black.SetActive(true);
            black.transform.GetChild(0).GetComponent<TMPro.TextMeshPro>().text = "Day " + day;
            if (day != 1) black.transform.GetChild(1).GetComponent<TMPro.TextMeshPro>().text = results[0] + results[1] + " happy citizens, " + results[2] + " got poisoned and " + results[3] + " went hungry";
            else black.transform.GetChild(1).GetComponent<TMPro.TextMeshPro>().text = "";

            foreach (Transform child in meals.transform) {
                if (child.name != "Canvas2") Destroy(child.gameObject);
            }

            Length++;

            randomisePoisons();
            generateMeals();
            generateList();
            enableButtons(0);

            Timer = 63;
            mealOn = 0;
            mealsX = 0;
            meals.transform.position = new Vector3(10 * -4, 0, 0);

            for (int i = 0; i < 4; i++) lresults[i] += results[i];
            results = new int[4];

            GameObject.Find("Health").GetComponent<TMPro.TextMeshProUGUI>().text = "Health: " + (5- (lresults[2] + lresults[3]));

        }

    }

}
