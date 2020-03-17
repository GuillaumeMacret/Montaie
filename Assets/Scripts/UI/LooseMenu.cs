using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LooseMenu : MonoBehaviour
{
    public GameObject entities;
    private void OnEnable()
    {
        entities.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
