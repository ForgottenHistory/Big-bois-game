using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Object;
using FishNet.Managing.Server;
using UnityEngine.AI;
using System.Linq;

public class CustomerManager : NetworkBehaviour, IInitialize
{
    public NetworkObject customer;

    public Transform spawnPointsParent;
    public Transform seatPointsParent;

    public bool isActive { get; set; } = true;

    /////////////////////////////////////////////////////////////////////////////////////

    List<CustomerSpawn> customers = new List<CustomerSpawn>();
    List<Seat> seats = new List<Seat>();

    ServerManager serverManager;
    Vector3 spawnPoint = Vector3.zero;

    /////////////////////////////////////////////////////////////////////////////////////

    public void Initialize()
    {
        if (seatPointsParent != null)
        {
            foreach (Transform t in seatPointsParent)
            {
                seats.Add(new Seat(t.gameObject));
            }
        }
        else
        {
            Debug.LogError("No seat points parent set!");
        }

        spawnPoint = spawnPointsParent.position;
        serverManager = InstanceFinder.ServerManager;
    }

    /////////////////////////////////////////////////////////////////////////////////////

    public void Deinitialize()
    {
        isActive = false;
    }

    public void SpawnCustomer()
    {
        if (isActive)
        {
            // find all unoccupied seats with linq
            List<Seat> unoccupiedSeats = seats.Where(s => !s.isOccupied).ToList();
            if (unoccupiedSeats.Count == 0)
            {
                Debug.Log("No unoccupied seats!");
                return;
            }

            NetworkObject no = Instantiate(customer, spawnPoint, Quaternion.identity);
            ServerManager.Spawn(no);
            CustomerSpawn cs = no.GetComponent<CustomerSpawn>();
            customers.Add(cs);

            // find random seat that is not occupied
            Seat seat = null;
            while (seat == null)
            {
                int seatIndex = Random.Range(0, seats.Count);
                seat = seats[seatIndex];
                if (seat.isOccupied)
                {
                    seat = null;
                }
            }

            cs.Initialize();
            cs.SetDestination(seat);
            seat.isOccupied = true;
        }
    }

    public void RemoveCustomer(CustomerSpawn cs)
    {
        customers.Remove(cs);
    }
}
