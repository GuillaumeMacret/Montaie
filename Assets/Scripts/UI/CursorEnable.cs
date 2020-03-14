using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorEnable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!Cursor.visible) Cursor.visible = true;
        Debug.Log(Cursor.visible);
    }
}
