using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionPredict : MonoBehaviour
{
    Transform PredictedLocation;
    public Rigidbody ObjectToPredictRigidBody;

    public int memorySize = 10;
    Vector3[] LastVelocities;
    private int m_LastVelocitiesIndex = 0;

    private Vector3 rememberPredict;
    private void Start()
    {
        PredictedLocation = GetComponent<Transform>();
        LastVelocities = new Vector3[memorySize];
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 velocity = ObjectToPredictRigidBody.velocity;

        Vector3 predict = ObjectToPredictRigidBody.position + velocity;

        rememberPredict += predict;
        rememberPredict /= 2;

        //Debug.Log(rememberPredict);
        PredictedLocation.transform.position = rememberPredict;
    }
}
