using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBehaviour : MonoBehaviour
{
    public float RotateSpeed;
    Quaternion rotation;


    private void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
     //   rotation.eulerAngles=new Vector3(0,0,20*Time.deltaTime);
        transform.Rotate(new Vector3(0, 0, RotateSpeed * Time.deltaTime));
    }




}
