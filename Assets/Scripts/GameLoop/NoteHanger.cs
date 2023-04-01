using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteHanger : MonoBehaviour
{
    List<GameObject> noteObjList = new List<GameObject>();
    List<Order> orderList = new List<Order>();

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