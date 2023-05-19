using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OrderObject : MonoBehaviour
{
    /////////////////////////////////////////////////////////////////////////////////////

    public TextMeshProUGUI orderText;
    public TextMeshProUGUI tableText;

    public Order order { get; set;}

    public bool OrderIsActive { get; set; } = false;

    /////////////////////////////////////////////////////////////////////////////////////

    public void SetOrderTexts(Order order)
    {   
        this.order = order;
        orderText.text = "";
        foreach (Food f in order.foods)
        {
            orderText.text += "- " + f.name + "\n";
        }
        tableText.text = "Table\n" + order.customerID.ToString();
        OrderIsActive = true;
    }
    
    public void ClearOrderTexts()
    {
        orderText.text = "";
        tableText.text = "";
        OrderIsActive = false;
    }

    /////////////////////////////////////////////////////////////////////////////////////
}
