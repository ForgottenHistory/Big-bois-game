using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Object;

public class TestConnection : NetworkBehaviour
{

    public override void OnStartClient()
    {
        base.OnStartClient();
        /* This is called on each client when the object
        * becomes visible to them. Networked values such as
        * Owner, ObjectId, and SyncTypes will already be
        * synchronized prior to this callback. */
        Debug.Log("Client started");
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        /* This callback is performed first. It occurs
        * when the object is initialized on the server.
        * Once called Owner, ObjectId, and much more are
        * already set.
        * OnStartServer is best used for initializing
        * server aspects of your script, such as getting
        * components only the server would need.
        * It can also be useful for setting values based on
        * the current state of your game. If you change synchronized
        * values within this method, such as SyncTypes,
        * those changes will be delivered to clients
        * when this object spawns for them.
        * Eg: perhaps you want to set a players name, which is
        * a SyncVar. You can do that here and it will be set
        * for clients when the object spawns on their side. */

        /* When using OnStartServer keep in mind that observers have
        * not yet been built for this object. If you were to send an ObserversRpc
        * for example it would not be delivered to any clients. You can however
        * still use an ObserversRpc and set BufferLast to true if you wish
        * clients to get it when the object is spawned for them. Another option
        * is to use OnSpawnServer, displayed below, and send a TargetRpc to the
        * connection which the object is spawning. */
    }
}
