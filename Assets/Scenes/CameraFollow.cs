using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Цель, за которой следует камера (Hero1)
    public Vector3 offset = new Vector3(0, 0, -10); // Смещение камеры
    public float smoothSpeed = 0.125f; // Скорость сглаживания

    void Start()
    {
        if (target == null)
        {
            Debug.LogWarning("CameraFollow: Target (Hero1) is not assigned!");
        }
        else
        {
            Debug.Log("CameraFollow: Target set to " + target.name);
        }
    }

    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}