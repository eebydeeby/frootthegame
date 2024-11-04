// Controls "worm points" (currently orbs) where worms will appear, handles player interaction with "worms"
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PointController : NetworkBehaviour
{
	private GameObject parentObject; // Calls back to parent fruit
	private GameObject spawner; // Refers to fruit spawner object
	private NetworkVariable<bool> eating = new NetworkVariable<bool>(false); // Variable to handle if worms are "eating" (vulnerable state)
	public Vector3 target; // Camera vector
	private NetworkObject localPlayer;
	
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

	void Awake(){
		localPlayer = NetworkManager.LocalClient.PlayerObject;
	}

	/* Handles input from player when clicking on worm point.
	If the worm is "eating," reduces the current number of worms on the parent fruit.
	Once it reaches zero, plays a different sound effect.
	Immediately sets worm to not-eating state if clicked while eating.*/
	
	void OnMouseDown()
	{
		if (IsClient == true && IsHost == false){
			Debug.Log("Clicked by client!");
			ReduceWormsServerRpc();
			if (eating.Value == true && Time.timeScale != 0) {
				
				if (this.transform.parent.gameObject.GetComponent<ShapeRotator>().currentWorms > 0)
				{
					//GameObject.Find("FruitSpawner").GetComponent<FruitSpawner>().playHit(1);
					return;
				}
				else if (this.transform.parent.gameObject.GetComponent<ShapeRotator>().currentWorms == 0)
				{
					//GameObject.Find("FruitSpawner").GetComponent<FruitSpawner>().playHit(2);
					return;
				}
				RegisterClickServerRpc();
			}
			else if (eating.Value == false && Time.timeScale != 0) {
				//GameObject.Find("FruitSpawner").GetComponent<FruitSpawner>().playHit(7);
				ReduceCountdownServerRpc();
			}
		}
	}

	[ServerRpc(RequireOwnership = false)]
	public void RegisterClickServerRpc(){
		if (IsHost){
		eating.Value = false;
		Debug.Log("Eating value changed!");
		}
	}

	[ServerRpc(RequireOwnership = false)]
	public void ReduceWormsServerRpc(){
		this.transform.parent.gameObject.GetComponent<ShapeRotator>().currentWorms--;
	}

	[ServerRpc(RequireOwnership = false)]
	public void ReduceCountdownServerRpc(){
		this.transform.parent.gameObject.GetComponent<ShapeRotator>().fruitCountdown -= 5.0f;
	}

	void Update()
	{
		{
			changeSkin();
		}
	}

	// Points worm points so they are always looking at camera (will make more sense with animated sprites)
    void LateUpdate()
    {
		transform.LookAt(target);
    }
	
	// Checks current state of worm and changes appearance accordingly
	void changeSkin() 
	{
		if (IsHost){
			changeSkinHost();
			changeSkinClientRpc();
		}
	}
	
	void changeSkinHost(){
		if (eating.Value == false && IsHost) {
			gameObject.GetComponent<Renderer>().material = orangeSkin;
		}
		else if (eating.Value == true && IsHost) {
			gameObject.GetComponent<Renderer>().material = greenSkin;
		}
	}


	[ClientRpc]
	void changeSkinClientRpc(){
		if (eating.Value == false && !IsHost) {
			gameObject.GetComponent<Renderer>().material = orangeSkin;
		}
		else if (eating.Value == true && !IsHost) {
			gameObject.GetComponent<Renderer>().material = greenSkin;
		}
		return;
	}

	// Changes state of worm
	void ChangeToEating()
	{
		eating.Value = true;
		Invoke(nameof(ChangeToNotEating), Random.Range(1, 3));
		return;
	}
	
	void ChangeToNotEating()
	{
		eating.Value = false;
		Invoke(nameof(ChangeToEating), Random.Range(3, 7));
		return;
	}
}