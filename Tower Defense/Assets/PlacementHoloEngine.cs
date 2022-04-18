using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlacementHoloEngine : MonoBehaviour
{
    private static Collider[] s_ObjectsinHolo;
    public static Vector3 S_CursorWorldPos;
    [SerializeField] private Material _validMat;
    [SerializeField] private Material _invalidMat;
    void Update()
    {
        S_CursorWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - new Vector3(0, 0, -10);
        transform.position = S_CursorWorldPos; // Keeps the hologram on the cursor, and away from the camera.
        s_ObjectsinHolo = Physics.OverlapCapsule(S_CursorWorldPos, S_CursorWorldPos + new Vector3(0, 0, -10), 0.55f);
        foreach (Collider col in s_ObjectsinHolo)
        {
            if (col.gameObject.tag == "Path" | col.gameObject.tag == "Tower" | GameManager.S_PlayerCash < 50)
            {
                gameObject.GetComponent<MeshRenderer>().material = _invalidMat;
                TowerPlacementManager.S_PlacementValid = false;
                return;
            }
            gameObject.GetComponent<MeshRenderer>().material = _validMat;
            TowerPlacementManager.S_PlacementValid = true;
        }
    }
}
