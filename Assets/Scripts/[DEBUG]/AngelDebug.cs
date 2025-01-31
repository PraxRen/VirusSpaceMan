using UnityEngine;

public class AngelDebug : MonoBehaviour
{
    [SerializeField] private Transform transform1;
    [SerializeField] private Transform transform2;

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(transform1.position, 0.2f);
    //    Gizmos.DrawLine(transform1.position, transform1.position + transform1.forward);

    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawSphere(transform2.position, 0.2f);
    //    Gizmos.DrawLine(transform2.position, transform2.position + transform2.forward);
    //}

    public void Update()
    {
        //Debug.Log(SimpleUtils.SignedAngleBetween(transform1.forward, transform2.forward, Vector3.up));

        //Debug.Log(Vector3.Dot(transform1.forward, transform2.forward));
        Debug.Log(Application.persistentDataPath);
    }
}