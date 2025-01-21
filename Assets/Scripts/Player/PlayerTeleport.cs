using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerTeleport : MonoBehaviour
    {
    private void OnTriggerEnter(Collider other)
    {
        // Busca el componente TeleportPoint en el trigger
        TeleportPoint teleportPoint = other.GetComponent<TeleportPoint>();

        if (teleportPoint != null)
        {
            // Mueve al jugador a la posición del TeleportPoint
            Teleport(teleportPoint.teleportLocation);
        }
    }

    private void Teleport(Vector3 targetPosition)
        {
        transform.position = targetPosition;
        Debug.Log($"Teleportado a: {targetPosition}");
        }
    }
}


