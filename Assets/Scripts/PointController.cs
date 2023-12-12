// Controls "worm points" (currently orbs) where worms will appear, handles player interaction with "worms"
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointController : MonoBehaviour
{
	private GameObject parentObject; // Calls back to parent fruit
	private GameObject spawner; // Refers to fruit spawner object
	private bool eating; // Variable to handle if worms are "eating" (vulnerable state)
	public Vector3 target; // Camera vector
	
	[SerializeField] private Material greenSkin;
	[SerializeField] private Material orangeSkin;
	[SerializeField] private Material blueSkin;
	// Different appearances for different states of worm points
	
    void Start()
    {
		parentObject = this.transform.parent.gameObject;
        target = GameObject.Find("Main Camera").transform.position;
		Invoke(nameof(ChangeToEating), Random.Range(0, 7)); // Starts countdown to "eating" state
    }

	/* Handles input from player when clicking on worm point.
	If the worm is "eating," reduces the current number of worms on the parent fruit.
	Once it reaches zero, plays a different sound effect.
	Immediately sets worm to not-eating state if clicked while eating.*/
	void OnMouseDown()
	{
		if (eating == true && Time.timeScale != 0) {
			this.transform.parent.gameObject.GetComponent<ShapeRotator>().currentWorms--;
			if (this.transform.parent.gameObject.GetComponent<ShapeRotator>().currentWorms > 0)
			{
				GameObject.Find("FruitSpawner").GetComponent<FruitSpawner>().playHit(1);
			}
			else if (this.transform.parent.gameObject.GetComponent<ShapeRotator>().currentWorms == 0)
			{
				GameObject.Find("FruitSpawner").GetComponent<FruitSpawner>().playHit(2);
			}
			eating = false;
		}
		else if (eating == false && Time.timeScale != 0) {
			GameObject.Find("FruitSpawner").GetComponent<FruitSpawner>().playHit(7);
			this.transform.parent.gameObject.GetComponent<ShapeRotator>().fruitCountdown -= 5.0f;
		}
	}

	void Update()
	{
		changeSkin();
	}

	// Points worm points so they are always looking at camera (will make more sense with animated sprites)
    void LateUpdate()
    {
		transform.LookAt(target);
    }
	
	// Checks current state of worm and changes appearance accordingly
	void changeSkin() 
	{
		if (!eating) {
			gameObject.GetComponent<Renderer>().material = orangeSkin;
		}
		else {
			gameObject.GetComponent<Renderer>().material = greenSkin;
		}
	}
	
	// Changes state of worm
	void ChangeToEating()
	{
		eating = true;
		Invoke(nameof(ChangeToNotEating), Random.Range(1, 3));
	}
	
	void ChangeToNotEating()
	{
		eating = false;
		Invoke(nameof(ChangeToEating), Random.Range(3, 7));
	}
}