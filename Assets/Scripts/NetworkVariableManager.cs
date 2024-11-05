using UnityEngine;
using Unity.Netcode;

public class NetworkVariableManager : NetworkBehaviour
{
    public NetworkVariable<bool> is_connected = new NetworkVariable<bool>(false);
}
