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
	private int score;
	private int lives;
	private int level;

    // Gets parent object and attaches current worm count variable to text on screen
    void LateUpdate()
    {
		fruit = GameObject.FindGameObjectWithTag("Fruit");
		spawner = GameObject.Find("FruitSpawner");
		if (fruit != null){
			timeLeft = spawner.GetComponent<FruitSpawner>().countdown;
			score = spawner.GetComponent<FruitSpawner>().score;
			lives = spawner.GetComponent<FruitSpawner>().lives;
			level = spawner.GetComponent<FruitSpawner>().checkpointLevel;
	        wormText.text = "Lives: " + lives + "\nTime Left: " + timeLeft.ToString() +
				"\nLevel: " + level + "\nScore: " + score;
		}
    }
}
