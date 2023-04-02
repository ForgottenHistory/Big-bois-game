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

    IUsable usable = null;
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
        MonoBehaviour[] components = other.GetComponents<MonoBehaviour>();
        IUsable otherUsable = null;

        foreach (MonoBehaviour component in components)
        {
            Debug.Log(typeof(IUsable) + " Is enabled " + component.enabled);
            if (component is IUsable && component.enabled)
            {
                otherUsable = (IUsable)component;
                Debug.Log("found usable");
                break;
            }
        }

        if (otherUsable != null)
        {
            if (usable != null)
            {
                MonoBehaviour usableMonoBehaviour = (MonoBehaviour)usable;
                MonoBehaviour otherUsableMonoBehaviour = (MonoBehaviour)otherUsable;

                if (otherUsableMonoBehaviour.isActiveAndEnabled)
                {
                    //is closer than current usable?
                    if (Vector3.Distance(other.transform.position, transform.position) < Vector3.Distance(usableMonoBehaviour.transform.position, transform.position))
                    {
                        usable = otherUsable;
                    }
                }
            }
            else
            {
                usable = otherUsable;
            }
        }
    }

    ////////////////////////////////////////////////////////////////

private void OnTriggerStay(Collider other)
{
    MonoBehaviour[] components = other.GetComponents<MonoBehaviour>();
    IUsable otherUsable = null;

    foreach (MonoBehaviour component in components)
    {
        if (component is IUsable && component.enabled)
        {
            otherUsable = (IUsable)component;
            break;
        }
    }

    if (otherUsable != null)
    {
        if (usable != null)
        {
            MonoBehaviour usableMonoBehaviour = (MonoBehaviour)usable;
            MonoBehaviour otherUsableMonoBehaviour = (MonoBehaviour)otherUsable;

            //compare usables in area
            if (Vector3.Distance(other.transform.position, transform.position) < Vector3.Distance(usableMonoBehaviour.transform.position, transform.position))
            {
                usable = otherUsable;
            }
        }
        else
        {
            usable = otherUsable;
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