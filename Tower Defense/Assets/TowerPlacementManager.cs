using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacementManager : MonoBehaviour
{
    public static bool S_PlacementValid;
    [SerializeField] private GameObject _cursorTowerHolo;
    [SerializeField] private GameObject _basicTower;
    private bool _isPlacing;
    [SerializeField] private GameObject _placementButton;
    [SerializeField] private GameObject _cancelPlacementButton;
    private GameObject TowerParent; // Keeps the towers organized under a parent object

    public void StartTowerPlacement()
    {
        TowerParent = GameObject.Find("Towers");
        _isPlacing = true;
        _cursorTowerHolo.SetActive(true);
        Cursor.visible = false;
        ButtonToggle(true); //Replaces the "place tower" button with the "cancel placement" button.
    }
    public void StopTowerPlacement()
    {
        _isPlacing = false;
        _cursorTowerHolo.SetActive(false);
        Cursor.visible = true;
        ButtonToggle(false); //Replaces the "cancel placement" button with the "place tower" button.
    }
    public void Update()
    {
        if (!_isPlacing | !S_PlacementValid)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0) && GameManager.S_PlayerCash >= 50)
        {
            GameManager.SetMoney(GameManager.S_PlayerCash - 50);
            Instantiate(_basicTower, PlacementHoloEngine.S_CursorWorldPos, Quaternion.identity, TowerParent.transform);
            StopTowerPlacement();
        }
    }
    private void ButtonToggle(bool isPlacing)
    {
        _placementButton.SetActive(!isPlacing);
        _cancelPlacementButton.SetActive(isPlacing);
    }
}
