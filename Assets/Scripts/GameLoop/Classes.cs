
using System.Collections.Generic;
using UnityEngine;

/////////////////////////////////////////////////////////////////////////////////////

public class Food {
    
    public Food() { } // Default constructor
    public Food (string name, float price) {
        this.name = name;
        this.price = price;
    }

    public string name;
    public float price;
}

/////////////////////////////////////////////////////////////////////////////////////

public class Order {

    public Order() { } // Default constructor
    public Order (int customerID, List<Food> foods) {
        this.customerID = customerID;
        this.foods = foods;
    }
    public List<Food> foods = new List<Food>();
    public int customerID;
}

/////////////////////////////////////////////////////////////////////////////////////

public class Seat
{
    public Seat(GameObject seatObject, int id)
    {
        this.seatObject = seatObject;
        position = seatObject.transform.position;
        rotation = seatObject.transform.rotation;
        this.id = id;
    }

    public Vector3 GetPosition { get { return position; } }
    public Quaternion GetRotation { get { return rotation; } }

    public bool isOccupied { get; set; } = false;
    public GameObject seatObject { get; set; }

    public int id { get; set; }

    Vector3 position;
    Quaternion rotation;
}

/////////////////////////////////////////////////////////////////////////////////////
