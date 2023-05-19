
using System.Collections.Generic;
using UnityEngine;

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
