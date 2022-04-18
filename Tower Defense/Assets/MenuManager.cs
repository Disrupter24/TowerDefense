using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public LayerMask ClickableLayers;
    public GameObject UpgradeMenu;
    private GameObject _recentClick;
    //Components that need to be updated with tower stats
    [SerializeField] private TMP_Text AttackSpeedCost;
    [SerializeField] private Text AttackSpeed;
    [SerializeField] private TMP_Text AttackDamageCost;
    [SerializeField] private Text AttackDamage;
    [SerializeField] private TMP_Text SaleCost;
    [SerializeField] private TMP_Text DamageDealt;
    private BasicTower CurrentTower;

    void Update()
    {
        if (CurrentTower != null)
        {
            UpdateTowerMenu(CurrentTower);
        }
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit raycastHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out raycastHit, 10f, ClickableLayers))
            {
                if (raycastHit.transform == null)
                {
                    Debug.Log("nullcast");
                    return;
                }
                if ((raycastHit.transform.gameObject == _recentClick) && (UpgradeMenu.activeSelf))
                {
                    UpgradeMenu.SetActive(false);
                    return;
                }
                if (raycastHit.transform.gameObject.tag == "Tower")
                {
                    _recentClick = raycastHit.transform.gameObject;
                    UpgradeMenuOpening(_recentClick);
                    return;
                }
                if (raycastHit.transform.gameObject.tag == "UI")
                {
                    return;
                }
            }
        }
    }
    void UpgradeMenuOpening(GameObject ClickedTower)
    {
        if(ClickedTower.transform.parent.gameObject.GetComponent<BasicTower>() != null)
        {
            CurrentTower = ClickedTower.transform.parent.gameObject.GetComponent<BasicTower>();
            UpdateTowerMenu(ClickedTower.transform.parent.gameObject.GetComponent<BasicTower>());
        }
        
        if (ClickedTower.transform.position.x > 0) // If tower is on the right, open menu on the left
        {
            UpgradeMenu.GetComponent<RectTransform>().anchoredPosition = new Vector3(-400, 0, 0);
            UpgradeMenu.SetActive(true);
        }
        else // If tower is on the left, open menu on the right
        {
            UpgradeMenu.GetComponent<RectTransform>().anchoredPosition = new Vector3(400, 0, 0);
            UpgradeMenu.SetActive(true);
        }

    }
    public void UpdateTowerMenu(BasicTower TowerScript)
    {
        AttackSpeedCost.text = TowerScript.SpeedCost.ToString() + "$";
        AttackSpeed.text = TowerScript.AttackSpeed.ToString();
        AttackDamageCost.text = TowerScript.DamageCost.ToString() + "$";
        AttackDamage.text = TowerScript.AttackDamage.ToString();
        SaleCost.text = TowerScript.SaleCost.ToString() + "$";
        DamageDealt.text = TowerScript.TowerScore.ToString() + " total damage dealt";
    }
    public void UpgradeTowerSpeed()
    {
        if(GameManager.S_PlayerCash < CurrentTower.SpeedCost)
        {
            return;
        }
        GameManager.SetMoney(GameManager.S_PlayerCash -= CurrentTower.SpeedCost);
        CurrentTower.SaleCost += Mathf.RoundToInt(0.5f * CurrentTower.SpeedCost);
        CurrentTower.AttackSpeed *= (0.99f);
        CurrentTower.SpeedCost += (100);
    }
    public void UpgradeTowerDamage()
    {
        if (GameManager.S_PlayerCash < CurrentTower.DamageCost)
        {
            return;
        }
        GameManager.SetMoney(GameManager.S_PlayerCash -= CurrentTower.DamageCost);
        CurrentTower.SaleCost += Mathf.RoundToInt(0.5f * CurrentTower.DamageCost);
        CurrentTower.AttackDamage += (1);
        CurrentTower.SpeedCost += (100);
    }
    public void SellTower()
    {
        GameManager.SetMoney(GameManager.S_PlayerCash += CurrentTower.SaleCost);
        Destroy(CurrentTower.gameObject);
    }
    public void CloseMenu()
    {
        UpgradeMenu.SetActive(false);
    }
}
