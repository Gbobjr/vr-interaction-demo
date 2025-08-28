using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class mono : MonoBehaviour


{
    public Rigidbody myRigidBody;
    public GameObject cubeX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //myRigidBody.linearVelocity = Vector3.up * 1;
        transform.rotation = cubeX.transform.rotation;
        //myRigidBody.transform.Rotate(Vector3.right);
        //cubeX.transform.Rotate(Vector3.right);
        print(transform.rotation);
    }
}
