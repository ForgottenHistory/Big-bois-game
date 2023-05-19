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

    ////////////////////////////////////////////////////////////////

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
        CheckUsableObject();
        UpdateUseText();
    }

    ////////////////////////////////////////////////////////////////
    // METHODS
    ////////////////////////////////////////////////////////////////

    private void CheckUsableObject()
    {
        if (usable != null && ((MonoBehaviour)usable).gameObject.activeSelf == false)
        {
            usable = null;
        }
        if (usable != null)
        {
            bool isHold = usable.hold;
            bool eKeyPressed = isHold ? Input.GetKey(KeyCode.E) : Input.GetKeyDown(KeyCode.E);
            if (eKeyPressed)
            {
                usable.Use();
            }
        }
    }

    ////////////////////////////////////////////////////////////////'
    // UPDATE USE TEXT
    ////////////////////////////////////////////////////////////////

    private void UpdateUseText()
    {
        if (usable != null && useTextObj != null)
        {
            UpdateUseTextForCustomerUsable();
            UpdateUseTextForNonCustomerUsable();
            useTextObj.SetActive(true);
        }
        else if (useTextObj != null)
        {
            useTextObj.SetActive(false);
        }
    }

    ////////////////////////////////////////////////////////////////
    // UPDATE USE TEXT FOR CUSTOMER USABLE
    ////////////////////////////////////////////////////////////////

    private void UpdateUseTextForCustomerUsable()
    {
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
    }

    ////////////////////////////////////////////////////////////////
    // UPDATE USE TEXT FOR NON CUSTOMER USABLE
    ////////////////////////////////////////////////////////////////

    private void UpdateUseTextForNonCustomerUsable()
    {
        if (!(usable is CustomerUsable))
        {
            useText.text = "Press [E] to use";
        }
    }

    ////////////////////////////////////////////////////////////////
    //
    //                      TRIGGERS
    //
    ////////////////////////////////////////////////////////////////

    ////////////////////////////////////////////////////////////////
    // ON TRIGGER ENTER
    ////////////////////////////////////////////////////////////////

    private void OnTriggerEnter(Collider other)
    {
        MonoBehaviour[] components = other.GetComponents<MonoBehaviour>();
        IUsable otherUsable = GetOtherUsable(components);
        if (otherUsable != null)
        {
            UpdateCurrentUsable(otherUsable, other.gameObject);
        }
    }

    ////////////////////////////////////////////////////////////////

    private IUsable GetOtherUsable(MonoBehaviour[] components)
    {
        foreach (MonoBehaviour component in components)
        {
            if (component is IUsable && component.enabled)
            {
                return (IUsable)component;
            }
        }
        return null;
    }

    ////////////////////////////////////////////////////////////////

    private void UpdateCurrentUsable(IUsable otherUsable, GameObject other)
    {
        if (usable != null)
        {
            MonoBehaviour usableMonoBehaviour = (MonoBehaviour)usable;
            MonoBehaviour otherUsableMonoBehaviour = (MonoBehaviour)otherUsable;
            if (otherUsableMonoBehaviour.isActiveAndEnabled && IsCloser(other.gameObject, usableMonoBehaviour.gameObject))
            {
                usable = otherUsable;
            }
        }
        else
        {
            usable = otherUsable;
        }
    }

    ////////////////////////////////////////////////////////////////
    // ON TRIGGER STAY
    ////////////////////////////////////////////////////////////////

    private void OnTriggerStay(Collider other)
    {
        IUsable otherUsable = GetOtherUsable(other);
        if (otherUsable != null)
        {
            UpdateCurrentUsable(otherUsable);
        }
    }

    ////////////////////////////////////////////////////////////////

    private IUsable GetOtherUsable(Collider other)
    {
        MonoBehaviour[] components = other.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour component in components)
        {
            if (component is IUsable && component.enabled)
            {
                return (IUsable)component;
            }
        }
        return null;
    }

    ////////////////////////////////////////////////////////////////

    private void UpdateCurrentUsable(IUsable otherUsable)
    {
        if (usable != null)
        {
            MonoBehaviour usableMonoBehaviour = (MonoBehaviour)usable;
            MonoBehaviour otherUsableMonoBehaviour = (MonoBehaviour)otherUsable;
            if (IsCloser(otherUsableMonoBehaviour.gameObject, usableMonoBehaviour.gameObject))
            {
                usable = otherUsable;
            }
        }
        else
        {
            usable = otherUsable;
        }
    }

    ////////////////////////////////////////////////////////////////

    private void OnTriggerExit(Collider other)
    {
        usable = null;
    }

    ////////////////////////////////////////////////////////////////
    // IS CLOSER CHECK
    ////////////////////////////////////////////////////////////////

    private bool IsCloser(GameObject firstObject, GameObject secondObject)
    {
        return Vector3.Distance(firstObject.transform.position, transform.position) 
        < Vector3.Distance(secondObject.transform.position, transform.position);
    }
    
    ////////////////////////////////////////////////////////////////
}