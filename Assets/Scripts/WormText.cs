// Displays current number of worms on screen
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WormText : MonoBehaviour
{
	public TextMeshProUGUI wormText;
	private GameObject fruit;
	private GameObject spawner;
	private int wormNumber;
	private float timeLeft;

    // Gets parent object and attaches current worm count variable to text on screen
    void LateUpdate()
    {
		fruit = GameObject.FindGameObjectWithTag("Fruit");
		spawner = GameObject.Find("FruitSpawner");
		if (fruit != null){
			wormNumber = fruit.GetComponent<ShapeRotator>().currentWorms;
			timeLeft = spawner.GetComponent<FruitSpawner>().countdown;
	        wormText.text = "Worms Remaining: " + wormNumber.ToString() + "\nTime Left: " + timeLeft.ToString();
		}
    }
}
