using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Object;
using FishNet.Serializing;
using FishNet.Managing.Server;
using UnityEngine.AI;
using FishNet.Object.Synchronizing;
using System.Linq;

public class CustomerSpawn : NetworkBehaviour, IInitialize
{

    ////////////////////////////////////////////////////////////////

    NavMeshAgent agent;
    Seat seat;

    OrderManager orderManager;

    ////////////////////////////////////////////////////////////////

    public CustomerUsable customerUsable = null;

    public Order order = new Order();

    bool seated = false;

    public bool isActive { get; set; } = true;

    public int customerID { get; set; } = -1;

    ////////////////////////////////////////////////////////////////
    // INIT & DEINIT
    ////////////////////////////////////////////////////////////////

    public void Initialize()
    {
        agent = GetComponent<NavMeshAgent>();
        customerUsable.customerSpawn = this;
        customerUsable.enabled = false;
        isActive = true;

        orderManager = GameObject.Find("GameManager").GetComponent<OrderManager>();
    }

    ////////////////////////////////////////////////////////////////

    public void Deinitialize()
    {
        isActive = false;
    }

    ////////////////////////////////////////////////////////////////
    // UPDATE
    ////////////////////////////////////////////////////////////////

    void Update()
    {
        if (isActive == false)
            return;

        CheckForSeated();
    }

    ////////////////////////////////////////////////////////////////
    // SEATING
    ////////////////////////////////////////////////////////////////

    void CheckForSeated()
    {
        if (seat != null && seated == false)
        {
            float distance = Vector3.Distance(transform.position, seat.GetPosition);
            if (distance < 1.5f)
            {
                TeleportToSeat();
                seated = true;
            }
        }
    }

    ////////////////////////////////////////////////////////////////

    public void SetDestination(Seat seat)
    {
        this.seat = seat;
        agent.SetDestination(seat.GetPosition);
    }

    ////////////////////////////////////////////////////////////////

    public void TeleportToSeat()
    {
        customerUsable.enabled = true;
        agent.enabled = false;
        transform.position = seat.GetPosition;
        transform.rotation = seat.GetRotation;

        order = orderManager.GetNewOrderObserverRpc(customerID);
    }
 
    ////////////////////////////////////////////////////////////////
}
