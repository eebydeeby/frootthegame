using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WormText : MonoBehaviour
{
	public TextMeshProUGUI wormText;
	private GameObject fruit;
	private int wormNumber;

    // Update is called once per frame
    void LateUpdate()
    {
		fruit = GameObject.FindGameObjectWithTag("Fruit");
		if (fruit != null){
			wormNumber = fruit.GetComponent<ShapeRotator>().currentWorms;
	        wormText.text = "Worms Remaining: " + wormNumber.ToString();
		}
    }
}
