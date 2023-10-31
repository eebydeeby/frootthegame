using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitManager : MonoBehaviour
{
	[HideInInspector] public bool dragged = false;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }
	
	void OnMouseDown()
	{
		dragged = true;
	}
	
	void OnMouseUp()
	{
		dragged = false;
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
