using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootArea : MonoBehaviour
{
    public EnemyControler enemyControler;

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("Trigger enter");
        if(other.tag == "Player")
        {
            //Debug.Log("Player");
            enemyControler.TryFire();
        }
    }
}
