// Displays current number of worms on screen
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;

public class WormText : NetworkBehaviour
{
	public TextMeshProUGUI wormText;
	private ShapeRotator fruit;
	private GameObject spawner;
	private NetworkVariable<float> fruitTime = new NetworkVariable<float>(0.0f);
	private NetworkVariable<int> fruitWorms = new NetworkVariable<int>(0);
	private NetworkVariable<int> wormNumber = new NetworkVariable<int>(0);
	private NetworkVariable<float> timeLeft = new NetworkVariable<float>(0.0f);
	private NetworkVariable<int> score = new NetworkVariable<int>(0);
	private NetworkVariable<int> lives = new NetworkVariable<int>(0);
	private NetworkVariable<int> level = new NetworkVariable<int>(0);

    // Gets parent object and attaches current worm count variable to text on screen
    void LateUpdate()
    {
		if (GameObject.FindGameObjectWithTag("Fruit").GetComponent<ShapeRotator>() != null){
			fruit = GameObject.FindGameObjectWithTag("Fruit").GetComponent<ShapeRotator>();
			spawner = GameObject.Find("FruitSpawner");
		}
		
		if (fruit != null){
			//timeLeft = spawner.GetComponent<FruitSpawner>().countdown;
			if (IsHost) {
			score.Value = spawner.GetComponent<FruitSpawner>().score.Value;
			lives.Value = spawner.GetComponent<FruitSpawner>().lives.Value;
			level.Value = spawner.GetComponent<FruitSpawner>().checkpointLevel;
			fruitTime.Value = fruit.fruitCountdown;
			fruitWorms.Value = fruit.currentWorms;
			}
	        wormText.text = "Lives: " + lives.Value +
				"\nLevel: " + level.Value + "\nScore: " + score.Value +
				("\nTime left: " + fruitTime.Value.ToString("#.00") + "\nWorms left: " + fruitWorms.Value);
		}
    }
}