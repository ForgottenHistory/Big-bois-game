using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : Usable
{
    Order order;

    public Note(bool hold) : base(hold)
    {
    }

    public void SetOrder(Order order)
    {
        this.order = order;
        UIManager.Instance.RemoveOrder(order);
    }

    public void RemoveOrder()
    {
        UIManager.Instance.TakeOrder(order);
        order = null;
        gameObject.SetActive(false);
    }

    public override void Use()
    {
        RemoveOrder();
    }
}