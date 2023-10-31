using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeRotator : MonoBehaviour
{
	private GameObject spawner;
	private Vector3 startMousePos;
	private Vector3 newMousePos;
	private Vector3 rotateMousePos;
	private int maxWorms;
	public int currentWorms;
	public float rotateSpeed;
	private bool dragged;
	
	void Awake()
	{
		spawner = GameObject.Find("FruitSpawner");
		maxWorms = Random.Range(3, 7);
		currentWorms = maxWorms;
	}
	
	void OnMouseDown()
	{
		dragged = true;
		Debug.Log("Dragged");
		startMousePos = Input.mousePosition;
	}
	
	void OnMouseUp()
	{
		dragged = false;
	}

    void Update()
    {	
		if (currentWorms == 0)
		{
			Destroy(gameObject);
		}
		
        if (dragged == true)
		{
			newMousePos = Input.mousePosition;
			rotateMousePos = (Vector3.Normalize(startMousePos - newMousePos) * rotateSpeed);
        	this.transform.Rotate(-rotateMousePos.y * Time.deltaTime, rotateMousePos.x * Time.deltaTime, 0, Space.World);
        }
    }
	
	void OnDestroy()
	{
		spawner.GetComponent<FruitSpawner>().Spawn();
	}
}