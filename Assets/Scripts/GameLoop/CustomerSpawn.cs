using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Object;
using FishNet.Serializing;
using FishNet.Managing.Server;
using UnityEngine.AI;

public class CustomerSpawn : NetworkBehaviour, IInitialize
{
    NavMeshAgent agent;
    Seat seat;

    public CustomerUsable customerUsable = null;

    public Order order { get; set; } = null;

    bool seated = false;

    public bool isActive { get; set; } = true;

    public void Initialize()
    {
        agent = GetComponent<NavMeshAgent>();
        customerUsable.customerSpawn = this;
        customerUsable.enabled = false;
        isActive = true;
    }

    public void Deinitialize()
    {
        isActive = false;
    }

    void Update()
    {
        if (isActive == false)
            return;

        CheckForSeated();
    }

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

    public void SetDestination(Seat seat)
    {
        this.seat = seat;
        agent.SetDestination(seat.GetPosition);
    }

    public void TeleportToSeat()
    {
        customerUsable.enabled = true;
        agent.enabled = false;
        transform.position = seat.GetPosition;
        transform.rotation = seat.GetRotation;

        List<Food> foods = new List<Food>();
        foods.Add( new Food("Hot dog", 5.0f) );
        order = new Order(seat.id, foods);
    }
}
