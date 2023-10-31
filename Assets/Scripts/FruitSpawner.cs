// Script for reacting to the current worm count - plays SFX and spawns fruit
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{

	[SerializeField] private GameObject berry;
	[SerializeField] private GameObject apple;
	[SerializeField] private GameObject pear;
	//Loads in prefab fruit to spawn
	
	[SerializeField] private AudioSource[] _hitSound;
	//Audio source to control
	
	public float countdown = 5f;
	private bool timerIsRunning = true;
	
	private int randomFruit;
	
	// Spawns a fruit at the start of the scene
    void Start()
    {
		Spawn();
		_hitSound = GetComponents<AudioSource>();
    }
	
	void Update()
	{
		if (timerIsRunning == true)
		{
			if (countdown > 0)
			{
				countdown -= Time.deltaTime;
				Debug.Log("Game NOT over!");
				Debug.Log(countdown);					
			}
			else
			{
				timerIsRunning = false;
				Destroy(gameObject);
				Debug.Log("Game over!");
			}
		}
	}

	// Spawns random prefab
	public void Spawn()
	{
		randomFruit = Random.Range(1,4);
		switch (randomFruit)
		{
		case 1:
			Instantiate(pear);
			break;
		case 2:
			Instantiate(apple);
			break;
		case 3:
			Instantiate(berry);
			break;		
		}
		countdown = 5f;
	}
	
	// Handles SFX t0 be played -- called by other scripts
	public void playHit(int soundID)
	{
		switch (soundID)
		{
			case 1:
				_hitSound[0].Play();
				break;
			case 2:
				_hitSound[1].Play();
				break;
		}
		
	}

}
