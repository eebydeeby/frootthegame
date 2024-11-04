using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LobbyText : MonoBehaviour
{
    [SerializeField] public GameObject nvm;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (nvm.GetComponent<NetworkVariableManager>().is_connected.Value == true)
        {
            this.gameObject.transform.GetChild(1).GetComponent<TMP_Text>().SetText("Players connected!\nHost must start game.");
            this.gameObject.transform.GetChild(3).GetChild(0).GetComponent<TMP_Text>().faceColor = Color.green;
        }
        else
        {
            this.gameObject.transform.GetChild(1).GetComponent<TMP_Text>().SetText("Waiitng on players...\nPick \"Host\" or \"Client.\"");
        }        
    }
}
