using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    GameObject useText = null;

    public bool isActive { get; set; } = false;

    ////////////////////////////////////////////////////////////////

    public void Initialize()
    {
        useText = GameObject.Find("UseText");
        if (useText == null)
        {
            Debug.LogError("No use text found!");
            return;
        }
        useText.SetActive(false);
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

        if (usable != null && useText != null)
        {
            bool isHold = usable.hold;
            bool eKeyPressed = isHold ? Input.GetKey(KeyCode.E) : Input.GetKeyDown(KeyCode.E);

            if (eKeyPressed)
            {
                usable.Use();
            }

            useText.SetActive(true);
        }
        else if (useText != null)
        {
            useText.SetActive(false);
        }
    }

    ////////////////////////////////////////////////////////////////
    //
    //                      TRIGGERS
    //
    ////////////////////////////////////////////////////////////////

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Usable>() != null)
        {
            if (usable != null)
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