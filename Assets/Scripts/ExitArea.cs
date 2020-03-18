using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitArea : MonoBehaviour
{
    public GameObject restartMenu;
    public GameObject gameEntities;
    public GameObject MenuCamera;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Player won ! TODO");
            restartMenu.SetActive(true);
            gameEntities.SetActive(false);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            MenuCamera.SetActive(true);
        }
    }
}
