using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;

public class CustomerUsable : NetworkBehaviour, IUsable
{
    ////////////////////////////////////////////////////////////////

    public CustomerSpawn customerSpawn { get; set; } = null;

    [SyncVar]
    public bool hasTakenOrder = false;

    public bool hold { get; set; } = false;

    ////////////////////////////////////////////////////////////////

    public void Use()
    {   
        if( hasTakenOrder == true )
            return;

        UIManager.Instance.TakeOrder( customerSpawn.order );
        SetOrderTakenServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetOrderTakenServerRpc()
    {
        hasTakenOrder = true;
    }

    ////////////////////////////////////////////////////////////////
}
