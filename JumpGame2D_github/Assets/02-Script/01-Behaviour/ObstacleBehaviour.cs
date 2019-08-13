using UnityEngine;

public class ObstacleBehaviour : MonoBehaviour
{
    public static ObstacleBehaviour instance;
    public float RotateSpeed;
    Quaternion rotation;
    public Vector3 rotationVector;
    Transform m_transform;

    private void Awake()
    {
        instance = this;
        m_transform = transform;
       // Debug.Log(transform.rotation);*/
    }

    private void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        rotationVector.z = RotateSpeed * Time.deltaTime;
        m_transform.Rotate(rotationVector);
    }

    public void StopRotate()
    {
        //gameObject.GetComponent<ObstacleBehaviour>().enabled = false;

        Debug.Log("j");
    }


}
