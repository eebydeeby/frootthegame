using UnityEngine;
using Unity.Netcode;

public class NetworkVariableManager : NetworkBehaviour
{
    public NetworkVariable<bool> is_connected = new NetworkVariable<bool>(false);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
