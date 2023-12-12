using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watch : MonoBehaviour
{
    private GameObject spawner;
    private FruitSpawner spawnerScript;
    private float watchCountDown;
    private GameObject particle; // Particle effect
    private int speed;

    void Awake()
    {
        watchCountDown = 10.0f; // Time before powerup despawns
    }

    void Start() 
    {
        if (Random.Range(0f, 2f) > 1) // Spawns powerup on either side of speed and sets direction for movement
        {
            speed = -1;
            gameObject.transform.position = new Vector3(32.0f, (Random.Range(-8.0f, 8.0f)), 12.3f);
        }
        else
        {
            speed = 1;
            gameObject.transform.position = new Vector3(-28f, (Random.Range(-8.0f, 8.0f)), 12.3f);
        }
        particle = GameObject.Find("HeartPop");
		spawner = GameObject.Find("FruitSpawner");
		spawnerScript = spawner.GetComponent<FruitSpawner>();
    }

    void OnMouseDown()
    {
        particle.transform.position = this.transform.position;
		particle.GetComponent<ParticleSystem>().Play();
        Time.timeScale = 0.1f; // Slows down game delta time
        spawnerScript.slowTime = 1.0f; // Fruit spawner checks if this reaches 0 and sets game speed back to normal if so
        Destroy(gameObject);
        spawnerScript.playHit(7);
    }

    // Update is called once per frame
    void Update()
    {
    	if (spawnerScript.timerIsRunning == true) // Handles countdown sequence
		{
			if (watchCountDown > 0f)
			{
				watchCountDown -= Time.deltaTime;			
			}
			else
			{
                Destroy(gameObject);
			}
        }
    }

    void LateUpdate()
    {
        gameObject.transform.position = new Vector3((gameObject.transform.position.x + (10 * Time.deltaTime * speed)),
            gameObject.transform.position.y, gameObject.transform.position.z);
    }
}
