using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class NoteHangerUsable : NetworkBehaviour, IUsable
{
    ////////////////////////////////////////////////////////////////

    public NoteHanger noteHanger = null;

    public bool hold { get; set; } = false;

    ////////////////////////////////////////////////////////////////

    public void Use()
    {
        if( UIManager.Instance.orderList.Count == 0 )
            return;
        int activeNoteObjects = 0;
        foreach (NetworkObject noteObj in noteHanger.noteObjList)
        {
            if (noteObj.gameObject.activeSelf == true)
                activeNoteObjects++;
        }
        if (activeNoteObjects >= noteHanger.noteObjList.Count)
            return;

        Order order = UIManager.Instance.orderList[UIManager.Instance.orderList.Count - 1];
        noteHanger.TakeOrder( order );
        UIManager.Instance.RemoveOrder( order );
    }

    ////////////////////////////////////////////////////////////////
}