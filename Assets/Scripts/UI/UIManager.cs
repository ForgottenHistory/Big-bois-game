using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////
    // PUBLIC VARIABLES
    ////////////////////////////////////////////////////////////////

    public static UIManager Instance { get; private set; }

    public List<OrderObject> orderObjects = new List<OrderObject>();

    ////////////////////////////////////////////////////////////////
    // PRIVATE VARIABLES
    ////////////////////////////////////////////////////////////////

    OrderManager orderManager;

    ////////////////////////////////////////////////////////////////
    // STARTUP
    ////////////////////////////////////////////////////////////////

    void Awake()
    {
        Instance = this;
    }

    ////////////////////////////////////////////////////////////////
    // ORDER MANAGEMENT
    ////////////////////////////////////////////////////////////////

    public void TakeOrder(Order order)
    {
        // if(orderManager == null)
        //     orderManager = GameObject.Find("GameManager").GetComponent<OrderManager>();

        foreach (OrderObject orderObject in orderObjects)
        {
            if (orderObject.OrderIsActive == false)
            {
                orderObject.SetOrderTexts(order);
                orderObject.gameObject.SetActive(true);
                break;
            }
        }
    }

    ////////////////////////////////////////////////////////////////

    public void RemoveOrder(Order order)
    {
        foreach (OrderObject orderObject in orderObjects)
        {
            if (orderObject.order.customerID == order.customerID)
            {
                orderObject.ClearOrderTexts();
                orderObject.gameObject.SetActive(false);
                break;
            }
        }
    }

    ////////////////////////////////////////////////////////////////
}
