using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointController : MonoBehaviour
{
	private GameObject parentObject;
	private GameObject spawner;
	private bool eating;
	public Vector3 target;
	[SerializeField] private Material greenSkin;
	[SerializeField] private Material orangeSkin;
	[SerializeField] private Material blueSkin;
	
    // Start is called before the first frame update
    void Start()
    {
		parentObject = this.transform.parent.gameObject;
        target = GameObject.Find("Main Camera").transform.position;
		Invoke(nameof(ChangeToEating), Random.Range(0, 7));
    }

	void OnMouseDown()
	{
		if (eating == true) {
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
	}

	void Update()
	{
		changeSkin();
	}

    // Update is called once per frame
    void LateUpdate()
    {
		transform.LookAt(target);
    }
	
	void changeSkin()
	{
		if (!eating) {
			gameObject.GetComponent<Renderer>().material = orangeSkin;
		}
		else {
			gameObject.GetComponent<Renderer>().material = greenSkin;
		}
	}
	
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