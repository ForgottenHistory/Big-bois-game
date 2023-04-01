using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerUsable : Usable
{
    public CustomerSpawn customerSpawn { get; set; } = null;
    public bool hasTakenOrder { get; set; } = false;
    public CustomerUsable(bool hold) : base(hold)
    {
    }

    public override void Use()
    {
        UIManager.Instance.TakeOrder( customerSpawn.order );
        hasTakenOrder = true;
    }
}
