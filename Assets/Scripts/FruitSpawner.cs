using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{

	[SerializeField] private GameObject berry;
	[SerializeField] private GameObject apple;
	[SerializeField] private GameObject pear;
	[SerializeField] private AudioSource[] _hitSound;
	private int randomFruit;
	
    void Start()
    {
		Spawn();
		_hitSound = GetComponents<AudioSource>();
    }
	
	/*public void Update()
	{
		if (GameObject.FindGameObjectsWithTag("Fruit") == null)
		{
			Spawn();
		}
	}*/
	
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
	}
	
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
