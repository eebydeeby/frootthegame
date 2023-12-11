// Handles status of fruit, namely rotation and worm status
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ShapeRotator : MonoBehaviour
{
	[SerializeField] private GameObject spawner; // Refers to fruit spawner object
	[SerializeField] private FruitSpawner spawnerScript;
	private int difficulty;
	public int fruitOrder;
	private Vector3 startMousePos; // Handles where the mouse was at upon first click
	private Vector3 newMousePos; // Handles current mouse position
	private Vector3 rotateMousePos; // Difference between previous two variables, used to calculate rotation
	private int maxWorms; // Starting number of worms for each fruit
	public int currentWorms; // Current number of worms
	[SerializeField] private float fruitCountdown;
	public float rotateSpeed; // How fast the fruit should rotate
	private bool dragged; // If the fruit is being dragged or no, ie, if player clicked on fruit
	private bool gameIsShuttingDown;
	public GameObject restarterObject;
	public LoseButton restarter;
	public GameObject fruitText;
	public TMP_Text fruitString;
	
	void Awake()
	{
		restarterObject = GameObject.Find("ButtonController");
		restarter = restarterObject.GetComponent<LoseButton>();
		spawner = GameObject.Find("FruitSpawner");
		spawnerScript = spawner.GetComponent<FruitSpawner>();
		fruitText = this.gameObject.transform.GetChild(0).GetChild(0).gameObject;
		fruitString = fruitText.GetComponent<TMP_Text>();
		difficulty = spawnerScript.difficulty;
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
			Destroy(this.gameObject);
		}
		
		// If fruit is being dragged, start rotating the fruit according to difference between curent and original position of cursor.
        if (dragged == true)
		{
			newMousePos = Input.mousePosition;
			rotateMousePos = (Vector3.Normalize(startMousePos - newMousePos) * rotateSpeed);
        	this.transform.Rotate(-rotateMousePos.y * Time.deltaTime, rotateMousePos.x * Time.deltaTime, 0, Space.World);
        }

		if (spawnerScript.timerIsRunning == true) // Handles countdown sequence
		{
			if (fruitCountdown > 0)
			{
				fruitCountdown -= Time.deltaTime;			
			}
			else
			{
				if (spawnerScript.lives > 0 && currentWorms > 0) // Checks if player has more than 0 lives when countdown is over...
				{
					spawnerScript.lives++;
					spawnerScript.playHit(6);
					fruitCountdown = 200 - (spawnerScript.checkpointLevel * 2); // Resets timer
				}
				else // ...And loads game over scene if not
				{
					spawnerScript.timerIsRunning = false;
					Destroy(gameObject);
					SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
				}
			}
		}
    }

	void LateUpdate()
	{
		fruitText.transform.rotation = Quaternion.Euler(0,0,0);
		fruitText.transform.position = new Vector3(((this.gameObject.transform.position.x)),
			((this.gameObject.transform.position.y)-4), 0);
		fruitString.text = "Time left: " + fruitCountdown.ToString("#.00") + "\nWorms left: ";
	}
	
	// Once all the worms are cleared, destroy the fruit and spawn a new one. 	
	void OnDestroy()
	{
		spawnerScript.score++;
		if (this.gameIsShuttingDown == false && restarter.isGameRestarting == false){
			switch (fruitOrder)
			{
				case 1:
					spawner.GetComponent<FruitSpawner>().isSlotOneTaken = false;
					break;
				case 2:
					spawner.GetComponent<FruitSpawner>().isSlotTwoTaken = false;
					break;
				case 3:
					spawner.GetComponent<FruitSpawner>().isSlotThreeTaken = false;
					break;
			}
			spawner.GetComponent<FruitSpawner>().orderFruit();
			spawner.GetComponent<FruitSpawner>().Spawn();
		}
	}

	void SetPosition()
	{
		if (fruitOrder == 0 || fruitOrder == 1){
			switch (difficulty)
			{	
				case 1:
					this.transform.Translate(0, 0, 0);
					break;
				case 2:
					this.transform.Translate(-4, 0, 0);
					break;
				case 3:
					this.transform.Translate(-8, 0, 0);
					break;
			}
		}
		if (fruitOrder == 2){
			switch (difficulty)
			{	
				case 2:
					this.transform.Translate(4, 0, 0);
					break;
				case 3:
					this.transform.Translate(0, 0, 0);
					break;
			}
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