// Displays current number of worms on screen
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WormText : MonoBehaviour
{
	public TextMeshProUGUI wormText;
	private GameObject fruit;
	private int wormNumber;

    // Gets parent object and attaches current worm count variable to text on screen
    void LateUpdate()
    {
		fruit = GameObject.FindGameObjectWithTag("Fruit");
		if (fruit != null){
			wormNumber = fruit.GetComponent<ShapeRotator>().currentWorms;
	        wormText.text = "Worms Remaining: " + wormNumber.ToString();
		}
    }
}
