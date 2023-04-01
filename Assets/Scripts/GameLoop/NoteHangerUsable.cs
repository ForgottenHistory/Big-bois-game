using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteHangerUsable : Usable
{
    public NoteHanger noteHanger = null;
    public NoteHangerUsable(bool hold) : base(hold)
    {
    }

    public override void Use()
    {
        Order order = UIManager.Instance.orderList[UIManager.Instance.orderList.Count - 1];
        noteHanger.TakeOrder( order );
        UIManager.Instance.RemoveOrder( order );
    }
}