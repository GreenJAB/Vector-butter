using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class list : MonoBehaviour
{
    public main Main;
    public GameObject itemPrefab;
    public bool showPoison;
    // Start is called before the first frame update
    void Start()
    {
        showPoison = true;
        if (showPoison) transform.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text="Poison";
        transform.GetChild(0).GetComponent<Canvas>().worldCamera = GameObject.Find("Camera").GetComponent<Camera>();
        Main = GameObject.Find("Game Manager").GetComponent<main>();

        //https://stackoverflow.com/questions/12627449/pack-squares-into-a-rectangle
        float rectWidth = 1.4f;
        float rectHeight = 1.2f;
        float tableRatio = rectWidth / rectHeight;

        int numItems = 1;
        if (showPoison) { numItems = Mathf.Max(Main.numPoisonSauces, Main.numPoisonDrinks);
        } else { numItems = Mathf.Max(Main.sauces.Length-Main.numPoisonSauces, Main.drinks.Length - Main.numPoisonDrinks); }
        float columns = Mathf.Sqrt(numItems * tableRatio);
        float rows = columns / tableRatio;

        columns = Mathf.Ceil(columns); 
        rows = Mathf.Ceil(rows);

        float squareSize = rectWidth / columns;
        
        int j = -1;
        for (int i = 0; i < (showPoison?Main.numPoisonSauces: Main.sauces.Length - Main.numPoisonSauces); i++){
            GameObject m = Instantiate<GameObject>(itemPrefab);
            m.transform.SetParent(transform.GetChild(1), false);
            m.name = "Food " + i;
            m.transform.localPosition = new Vector3((i % columns) * squareSize + squareSize / 2, -(int)(i / columns) * squareSize - squareSize / 2, -0.012f);
            m.transform.localScale = new Vector3(squareSize*0.8f,squareSize * 0.8f, 1);

            j++;
            while (Main.poisonSauces[j] ==!showPoison) j++;
            m.GetComponent<SpriteRenderer>().sprite = Main.sauces[j];
        }

        j = -1;
        for (int i = 0; i < (showPoison ? Main.numPoisonDrinks : Main.drinks.Length - Main.numPoisonDrinks); i++){
            GameObject m = Instantiate<GameObject>(itemPrefab);
            m.transform.SetParent(transform.GetChild(2), false);
            m.name = "Food " + i;
            m.transform.localPosition = new Vector3((i % columns) * squareSize+ squareSize/2, -(int)(i / columns) * squareSize - squareSize / 2, -0.012f);
            m.transform.localScale = new Vector3(squareSize * 0.8f, squareSize * 0.8f, 1);

            j++;
            while (Main.poisonDrinks[j] == !showPoison) j++;
            m.GetComponent<SpriteRenderer>().color = Main.drinks[j];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
