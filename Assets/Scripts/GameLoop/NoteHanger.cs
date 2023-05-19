using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using System.Linq;

public class NoteHanger : NetworkBehaviour, IInitialize
{
    /////////////////////////////////////////////////////////////////////////////////////

    public Transform notesParent;

    UIManager uiManager;
    List<NoteHangerObject> noteHangerObjects = null;

    /////////////////////////////////////////////////////////////////////////////////////

    public bool isActive { get; set; } = false;

    /////////////////////////////////////////////////////////////////////////////////////

    public void Deinitialize()
    {
        isActive = false;
    }
 
    /////////////////////////////////////////////////////////////////////////////////////
 
    public void Initialize()
    {
        isActive = true;
        noteHangerObjects = new List<NoteHangerObject>();

        foreach (Transform child in notesParent)
        {
            NoteHangerObject noteHangerObject = child.GetComponent<NoteHangerObject>();
            noteHangerObjects.Add(noteHangerObject);
            child.gameObject.SetActive(false);
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////

    public void SetNote(Order order)
    {
        foreach (NoteHangerObject noteHangerObject in noteHangerObjects)
        {
            if (noteHangerObject.gameObject.activeSelf == false)
            {
                noteHangerObject.Activate(order);
                break; 
            }
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////

}
