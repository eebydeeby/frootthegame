// Handles status of fruit, namely rotation and worm status
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeRotator : MonoBehaviour
{
	[SerializeField] private GameObject spawner; // Refers to fruit spawner object
	private int difficulty;
	public int fruitOrder;
	private Vector3 startMousePos; // Handles where the mouse was at upon first click
	private Vector3 newMousePos; // Handles current mouse position
	private Vector3 rotateMousePos; // Difference between previous two variables, used to calculate rotation
	private int maxWorms; // Starting number of worms for each fruit
	public int currentWorms; // Current number of worms
	public float rotateSpeed; // How fast the fruit should rotate
	private bool dragged; // If the fruit is being dragged or no, ie, if player clicked on fruit
	private bool gameIsShuttingDown;
	public GameObject restarterObject;
	public LoseButton restarter;
	
	void Awake()
	{
		restarterObject = GameObject.Find("ButtonController");
		restarter = restarterObject.GetComponent<LoseButton>();
		spawner = GameObject.Find("FruitSpawner");
		difficulty = spawner.GetComponent<FruitSpawner>().difficulty;
		maxWorms = Random.Range(3, 7); // Random number of worms to start
		currentWorms = maxWorms;
	}

	void Start()
	{
		if (fruitOrder == 0){
			Destroy(gameObject);
		}
		SetPosition();
	}
	
	void OnMouseDown()
	{
		dragged = true;
		startMousePos = Input.mousePosition;
	}
	
	// Releases fruit
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
		
		// If fruit is being dragged, start rotating the fruit according to difference between curent and original position of cursor.
        if (dragged == true)
		{
			newMousePos = Input.mousePosition;
			rotateMousePos = (Vector3.Normalize(startMousePos - newMousePos) * rotateSpeed);
        	this.transform.Rotate(-rotateMousePos.y * Time.deltaTime, rotateMousePos.x * Time.deltaTime, 0, Space.World);
        }
    }
	
	// Once all the worms are cleared, destroy the fruit and spawn a new one. 	
	void OnDestroy()
	{
		if (this.gameIsShuttingDown == false && restarter.isGameRestarting == false){ spawner.GetComponent<FruitSpawner>().Spawn(); }
	}

	void SetPosition()
	{
		if (fruitOrder == 0 || fruitOrder == 1){
			this.transform.Translate(0, 0, 0);
		}
		if (fruitOrder == 2){
			this.transform.Translate(-8, 0, 0);
		}
		if (fruitOrder == 3){
			this.transform.Translate(8, 0, 0);
		}
	}

    void OnApplicationQuit()
    {
    	gameIsShuttingDown = true;
    }
}