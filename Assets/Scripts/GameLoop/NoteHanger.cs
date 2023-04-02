using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class NoteHanger : NetworkBehaviour
{
    public List<GameObject> noteObjList { get; set; } = new List<GameObject>();
    UIManager uiManager;

    void Start()
    {
        foreach (Transform child in transform)
        {
            noteObjList.Add(child.gameObject);
            child.gameObject.SetActive(false);
        }

        uiManager = UIManager.Instance;
    }

    public void TakeOrder(Order order)
    {
        foreach (GameObject noteObj in noteObjList)
        {
            if (noteObj.activeSelf == false)
            {
                noteObj.SetActive(true);
                noteObj.GetComponent<Note>().SetOrder(order);
                break;
            }
        }
    }
}