using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTower : MonoBehaviour
{
    private Collider[] s_ObjectsinRange;
    private GameObject _currentTarget;
    private int _attackSpeed = 1; // Delay between attacks in seconds.
    private int _attackDamage = 1; // Damage done to an enemy on attack.
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShootingSystem());
    }

    private IEnumerator ShootingSystem()
    {
        while (true)
        {
            AcquireTarget();
            if (_currentTarget != null)
            {
                _currentTarget.GetComponent<EnemyEngine>().TakeDamage(_attackDamage);
            }
            yield return new WaitForSeconds(_attackSpeed);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void AcquireTarget() // Finds target among those nearby
    {
        s_ObjectsinRange = Physics.OverlapCapsule(transform.position, transform.position + new Vector3(0, 0, -10), 5f);
        int stepmax = 0;
        foreach (Collider col in s_ObjectsinRange)
        {
            if (col.gameObject.tag == "Enemy" && col.gameObject.GetComponent<EnemyEngine>().StepsTaken > stepmax)
            {
                stepmax = col.gameObject.GetComponent<EnemyEngine>().StepsTaken;
                _currentTarget = col.gameObject;
            }
        }
        if (stepmax == 0)
        {
            _currentTarget = null;
        }
    }

}
