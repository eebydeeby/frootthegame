using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;


public class LobbyManager : NetworkBehaviour
{    
    [SerializeField] private float CountDown;
    private GameObject nvm;    

    public void Start(){
        Debug.Log(GetFullPath(transform));
    }

    private void OnClientConnected(ulong clientId)
    {
        print("Client connected with id: " + clientId);
        if (IsOwnedByServer && IsLocalPlayer) {
            print("I'm the server.");
        }
        else print ("I'm not the server");
        
        if (!IsOwner){
            nvm = GameObject.Find("/NetworkVariableManager"); 
            nvm.GetComponent<NetworkVariableManager>().is_connected.Value = true;
        }
    }

    private void OnClientDisconnected(ulong clientId)
    {
        if (!IsOwner)
        {
            nvm = GameObject.Find("/NetworkVariableManager"); 
            nvm.GetComponent<NetworkVariableManager>().is_connected.Value = false;
            print("Player disconnected.");
        }
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        }
    }

    public override void OnNetworkSpawn()
    {
        CountDown = 3.0f;

        if(IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        }
    }

    // Recursive function to get the full path
    private string GetFullPath(Transform current)
    {
        // Base case: if there's no parent, we're at the root
        if (current.parent == null)
            return current.name;

        // Recursive case: get the parent's path and add this object's name
        return GetFullPath(current.parent) + "/" + current.name;
    }
}