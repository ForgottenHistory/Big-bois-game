using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NoteHangerUsable : MonoBehaviour, IUsable
{
    // Note hanger to set the order on
    public NoteHanger noteHanger;
    public bool hold { get; } = false;

    /////////////////////////////////////////////////////////////////////////////////////
    // On player interact with note hanger, take the last order in hand and set it on the note hanger
    public void Use()
    {
        // Find active order in hand using LINQ
        OrderObject lastOrderObjectInHand = UIManager.Instance.orderObjects.FirstOrDefault(o => o.isActiveAndEnabled);
        if(lastOrderObjectInHand != null)
        {
            if ( lastOrderObjectInHand.OrderIsActive == true )
            {
                Order lastOrderInHand = lastOrderObjectInHand.order;
                noteHanger.SetNote( lastOrderInHand );
                UIManager.Instance.RemoveOrder( lastOrderInHand );
            }
        }
    }
 
    /////////////////////////////////////////////////////////////////////////////////////
}
