using UnityEngine;


public class mono : MonoBehaviour


{
    public Rigidbody myRigidBody;
    public GameObject cubeX;
    public GameObject cubeZ;
    //float rotationX = transform.eulerAngles.x;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.rotation = cubeX.transform.rotation;
        //transform.rotation.z = cubeX.transform.rotation.z;
        //myRigidBody.linearVelocity = Vector3.up * 1;
        //eulerAngles.x = cubeX.eulerAngles.x;
        //rotationX = cubeZ.transform.rotation;
        //myRigidBody.transform.Rotate(Vector3.right);
        //cubeX.transform.Rotate(Vector3.right);
        //print(transform.rotation);
    }
}
