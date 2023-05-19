using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public class OrderManager : NetworkBehaviour
{
    /////////////////////////////////////////////////////////////////////////////////////
    
    [SyncObject]
    public readonly SyncList<Order> orders = new SyncList<Order>();

    /////////////////////////////////////////////////////////////////////////////////////

    public void AddFoodToOrder(int customerID, Order order, Food food)
    {
        order.foods.Add(food);
    }

    /////////////////////////////////////////////////////////////////////////////////////

    [Server]
    public Order GetNewOrderObserverRpc(int customerID)
    {
        Order order = new Order(customerID, new List<Food>());
        AddFoodToOrder(customerID, order, new Food { name = "Burger", price = 5.99f });
        orders.Add(order);
        
        return order;
    }
}

/////////////////////////////////////////////////////////////////////////////////////

public struct Food
{
    public string name;
    public float price;
}

/////////////////////////////////////////////////////////////////////////////////////

public struct Order
{
    public enum ORDER_STATUS
    {
        NOT_TAKEN,
        TAKEN,
        HANGING,
        DELIVERED
    }

    public Order(int customerID, List<Food> foods)
    {
        orderStatus = ORDER_STATUS.NOT_TAKEN;
        this.customerID = customerID;
        this.foods = foods;
    }

    public int customerID;
    public List<Food> foods;
    public ORDER_STATUS orderStatus;
}

/////////////////////////////////////////////////////////////////////////////////////
