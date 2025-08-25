using UnityEngine;


public class CameraFollow : MonoBehaviour
{


    public Transform target;
    public Vector3 offset = new Vector3(0f, 0f, -10f);

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime * 5f);
          //  transform.position = target.position + offset;
        }
    }

}
