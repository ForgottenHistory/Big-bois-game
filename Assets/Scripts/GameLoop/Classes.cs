public class Food {

    public Food (string name, float price) {
        this.name = name;
        this.price = price;
    }

    public string name;
    public float price;
}

public class Order {

    public Order (int customerID, List<Food> foods) {
        this.customerID = customerID;
        this.foods = foods;
    }
    List<Food> foods = new List<Food>();
    int customerID;
}

public class Seat
{
    public Seat(GameObject seatObject)
    {
        this.seatObject = seatObject;
        position = seatObject.transform.position;
        rotation = seatObject.transform.rotation;
    }

    public Vector3 GetPosition { get { return position; } }
    public Quaternion GetRotation { get { return rotation; } }

    public bool isOccupied { get; set; } = false;
    public GameObject seatObject { get; set; }

    Vector3 position;
    Quaternion rotation;
}
