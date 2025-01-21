using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour
{
    // Ubicación a la que teletransportará
    public Vector3 teleportLocation; 

    private void OnDrawGizmos()
    {
        
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(teleportLocation, 0.5f);
    }
}

