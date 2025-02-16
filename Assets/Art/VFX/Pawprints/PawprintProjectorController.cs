using UnityEngine;
using System.Collections;

/*
    Pawprint Spawner Script

    Purpose:
    - Spawns a pawprint decal at the location of a trigger collider when it enters a collision.
    - Uses raycasting to ensure the pawprint is placed on the terrain.

    What Still Needs to Be Done:
    - **Correct Rotation:** Pawprints do not currently align with the character’s movement direction.
    - **Conditional Spawning:** Pawprints should only appear on specific terrain textures (e.g., dirt, sand, snow).
    - **Placement Verification:** Ensure pawprints do not clip into the ground or float above the surface.

    Notes:
    - This system is currently a **basic implementation** to visualize footprints.
    - Programmers need to refine the logic for terrain detection and correct rotation.
*/

public class PawprintProjectorController : MonoBehaviour
{
    public GameObject pawprintPrefab; // Prefab for the pawprint decal
    public float decalLifetime = 5f; // Time before the decal is destroyed

    private void OnTriggerEnter(Collider other)
    {
        SpawnPawprint(transform.position);
    }

    private void SpawnPawprint(Vector3 position)
    {
        Debug.Log("[PawprintSpawner] Spawning pawprint at " + position);

        // Raycast downward to adjust placement on the ground
        RaycastHit hit;
        if (Physics.Raycast(position + Vector3.up * 0.5f, Vector3.down, out hit, 1f))
        {
            position = hit.point;

            // Instantiate pawprint with the prefab's original rotation (currently incorrect)
            GameObject pawprint = Instantiate(pawprintPrefab, position, pawprintPrefab.transform.rotation);

            Debug.Log("[PawprintSpawner] Final Pawprint Rotation: " + pawprint.transform.rotation.eulerAngles);

            Destroy(pawprint, decalLifetime);
        }
        else
        {
            Debug.LogError("[PawprintSpawner] ERROR: Raycast failed, could not find ground position!");
        }
    }
}
