using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUsable : MonoBehaviour, IInitialize
{
    ////////////////////////////////////////////////////////////////
    //
    //                      PLAYER USABLE
    //
    // use the one nearest
    //
    ////////////////////////////////////////////////////////////////

    Usable usable = null;
    GameObject useTextObj = null;
    TextMeshProUGUI useText = null;

    public bool isActive { get; set; } = false;

    ////////////////////////////////////////////////////////////////

    public void Initialize()
    {
        useTextObj = GameObject.Find("UseText");
        if (useTextObj == null)
        {
            Debug.LogError("No use text found!");
            return;
        }
        useText = useTextObj.GetComponent<TextMeshProUGUI>();
        useTextObj.SetActive(false);

        isActive = true;
    }

    public void Deinitialize()
    {
    }

    ////////////////////////////////////////////////////////////////
    // UPDATE
    ////////////////////////////////////////////////////////////////

    private void Update()
    {
        if (isActive == false)
            return;

        if (usable != null && useTextObj != null)
        {
            bool isHold = usable.hold;
            bool eKeyPressed = isHold ? Input.GetKey(KeyCode.E) : Input.GetKeyDown(KeyCode.E);

            if (eKeyPressed)
            {
                usable.Use();
            }

            if (usable is CustomerUsable)
            {
                CustomerUsable customerUsable = (CustomerUsable)usable;
                if (customerUsable.hasTakenOrder == false)
                {
                    useText.text = "Press [E] to take order";
                }
                else
                {
                    useText.text = "You already took this order!";
                }
            }
            else
            {
                useText.text = "Press [E] to use";
            }

            useTextObj.SetActive(true);
        }
        else if (useTextObj != null)
        {
            useTextObj.SetActive(false);
        }
    }

    ////////////////////////////////////////////////////////////////
    //
    //                      TRIGGERS
    //
    ////////////////////////////////////////////////////////////////

    private void OnTriggerEnter(Collider other)
    {   
        Usable otherUsable = other.GetComponent<Usable>();
        if (otherUsable != null)
        {
            if (usable != null && otherUsable.isActiveAndEnabled == true)
            {
                //is closer than current usable?
                if (Vector3.Distance(other.transform.position, transform.position) < Vector3.Distance(usable.transform.position, transform.position))
                {
                    usable = other.GetComponent<Usable>();
                }
            }
            else
            {
                usable = other.GetComponent<Usable>();
            }
        }
    }

    ////////////////////////////////////////////////////////////////

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Usable>() != null)
        {
            if (usable != null)
            {
                //compare usables in area
                if (Vector3.Distance(other.transform.position, transform.position) < Vector3.Distance(usable.transform.position, transform.position))
                {
                    usable = other.gameObject.GetComponent<Usable>();
                }
            }
            else
            {
                usable = other.GetComponent<Usable>();
            }
        }
    }

    ////////////////////////////////////////////////////////////////

    private void OnTriggerExit(Collider other)
    {
        usable = null;
    }

    ////////////////////////////////////////////////////////////////
}