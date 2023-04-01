using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public List<Order> orderList { get; set; } = new List<Order>();
    public List<OrderObject> orderObjects = new List<OrderObject>();

    void Awake()
    {
        Instance = this;
    }

    public void TakeOrder( Order order ) {
        foreach( OrderObject orderObject in orderObjects ) {
            if( orderObject.OrderIsActive == false ) {
                orderObject.SetOrderTexts( order );
                orderObject.gameObject.SetActive(true);
                orderList.Add(order);
                break;
            }
        }
    }

    public void RemoveOrder( Order order ) {
        foreach( OrderObject orderObject in orderObjects ) {
            if( orderObject.order == order ) {
                orderObject.OrderIsActive = false;
                orderObject.gameObject.SetActive(false);
                orderList.Remove(order);
                break;
            }
        }
    }
}
