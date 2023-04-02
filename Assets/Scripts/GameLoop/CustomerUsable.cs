using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class CustomerUsable : NetworkBehaviour, IUsable
{
    ////////////////////////////////////////////////////////////////

    public CustomerSpawn customerSpawn { get; set; } = null;
    public bool hasTakenOrder { get; set; } = false;

    public bool hold { get; set; } = false;

    ////////////////////////////////////////////////////////////////

    public void Use()
    {
        UIManager.Instance.TakeOrder( customerSpawn.order );
        hasTakenOrder = true;
    }

    ////////////////////////////////////////////////////////////////
}
