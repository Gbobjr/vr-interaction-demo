
using UnityEngine;

public class mono : MonoBehaviour


{
    public Rigidbody myRigidBody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        myRigidBody.linearVelocity = Vector3.up * 1;
    }
}
