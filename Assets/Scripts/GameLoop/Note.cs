using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class Note : NetworkBehaviour, IUsable
{
    ////////////////////////////////////////////////////////////////

    Order order;

    public bool hold { get; set; } = false;

    ////////////////////////////////////////////////////////////////

    public void SetOrder(Order order)
    {
        this.order = order;
        UIManager.Instance.RemoveOrder(order);
    }

    ////////////////////////////////////////////////////////////////

    /*
    public void RemoveOrder()
    {
        UIManager.Instance.TakeOrder(order);
        order = null;
        gameObject.SetActive(false);
    }
    */

    ////////////////////////////////////////////////////////////////

    public void Use()
    {
        RemoveOrder();
    }

    ////////////////////////////////////////////////////////////////

    public void RemoveOrder()
    {
        if (IsServer)
        {
            RpcRemoveOrderOnClient();
        }
        else
        {
            CmdRemoveOrderOnClient();
        }
    }

    ////////////////////////////////////////////////////////////////

    [ServerRpc(RequireOwnership = false)]
    private void CmdRemoveOrderOnClient()
    {
        RpcRemoveOrderOnClient();
    }

    ////////////////////////////////////////////////////////////////

    [ObserversRpc]
    private void RpcRemoveOrderOnClient()
    {
        UIManager.Instance.TakeOrder(order);
        order = null;
        gameObject.SetActive(false);
    }

    ////////////////////////////////////////////////////////////////
}