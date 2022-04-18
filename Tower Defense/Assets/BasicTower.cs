using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTower : MonoBehaviour
{
    private Collider[] s_ObjectsinRange;
    private GameObject _currentTarget;
    private int _attackSpeed = 1; // Delay between attacks in seconds.
    private int _attackDamage = 1; // Damage done to an enemy on attack.'
    private LineRenderer LaserShot; // Strictly visual feedback on the tower's shots.
    // Start is called before the first frame update
    void Start()
    {
        LaserShot = GetComponent<LineRenderer>();
        LaserShot.SetPosition(0, new Vector3(transform.position.x, transform.position.y, -1));
        StartCoroutine(ShootingSystem());
    }

    private IEnumerator ShootingSystem()
    {
        while (true)
        {
            AcquireTarget();
            if (_currentTarget != null)
            {
                _currentTarget.GetComponent<EnemyEngine>().TakeDamage(_attackDamage); // Shoots the enemy
                LaserShotVisuals(true);
                yield return new WaitForSeconds(0.1f);
                LaserShotVisuals(false);
                yield return new WaitForSeconds(_attackSpeed - 0.125f);
            }
            yield return new WaitForSeconds(0.025f);
        }
        
    }

    void LaserShotVisuals(bool isVisible)
    {
        if (!isVisible)
        {
            LaserShot.enabled = false;
            return;
        }
        LaserShot.enabled = true;
        LaserShot.SetPosition(1, new Vector3(_currentTarget.transform.position.x, _currentTarget.transform.position.y, -1));


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
