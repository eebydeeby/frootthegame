using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    private GameObject spawner;
    private FruitSpawner spawnerScript;
    public float heartCountDown;
    public GameObject particle;
	public ParticleSystem ps;

    void Awake()
    {
        heartCountDown = 1.0f;
    }

    void Start()
    {
        particle = GameObject.Find("HeartPop");
		spawner = GameObject.Find("FruitSpawner");
		spawnerScript = spawner.GetComponent<FruitSpawner>();
    }

    void OnMouseDown()
    {
    	particle.transform.position = this.transform.position;
		particle.GetComponent<ParticleSystem>().Play();
        spawnerScript.lives++;
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
    	if (spawnerScript.timerIsRunning == true) // Handles countdown sequence
		{
			if (heartCountDown > 0)
			{
				heartCountDown -= Time.deltaTime;			
			}
			else
			{
                Destroy(gameObject);
			}
        }
    }

    void LateUpdate()
    {

    }
}
