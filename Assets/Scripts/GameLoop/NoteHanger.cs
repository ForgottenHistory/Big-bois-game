using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class NoteHanger : NetworkBehaviour, IInitialize
{
    ////////////////////////////////////////////////////////////////
    // Summary:
    // This class is used to hold notes for players to pick up
    ////////////////////////////////////////////////////////////////

    public List<NetworkObject> noteObjList { get; set; } = new List<NetworkObject>();
    public bool isActive { get; set; }

    List<Note> noteList = new List<Note>();
    UIManager uiManager;

    ////////////////////////////////////////////////////////////////
    
    public void Initialize()
    {
        Debug.Log("NoteHanger.OnStartClient()");
        foreach (Transform child in transform)
        {
            noteObjList.Add(child.GetComponent<NetworkObject>());
            noteList.Add(child.GetComponent<Note>());
            child.gameObject.SetActive(false);
        }

        uiManager = UIManager.Instance;
    }

    public void Deinitialize()
    {

    }

    ////////////////////////////////////////////////////////////////

    /*
    public void TakeOrder(Order order)
    {
        for (int i = 0; i < noteObjList.Count; i++)
        {
            if (noteObjList[i].gameObject.activeSelf == false)
            {
                noteObjList[i].gameObject.SetActive(true);
                noteList[i].SetOrder(order);
                break;
            }
        }
    }
    */

    ////////////////////////////////////////////////////////////////

    public void TakeOrder(Order order)
    {
        if (IsServer)
        {
            SetOrderOnServer(order);
        }
        else
        {
            CmdSetOrderOnServer(order);
        }
    }

    ////////////////////////////////////////////////////////////////

    [ServerRpc(RequireOwnership = false)]
    private void CmdSetOrderOnServer(Order order)
    {
        SetOrderOnServer(order);
    }

    ////////////////////////////////////////////////////////////////

    [Server]
    private void SetOrderOnServer(Order order)
    {
        foreach (NetworkObject noteObj in noteObjList)
        {
            if (!noteObj.gameObject.activeSelf)
            {
                noteObj.gameObject.SetActive(true);
                Note note = noteObj.GetComponent<Note>();
                note.SetOrder(order);
                break;
            }
        }
    }
    
    ////////////////////////////////////////////////////////////////
}