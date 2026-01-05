using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    [Header("Таргет камеры")]
    public Transform target;

    [Range(0, 1)]
    public float smooth = 0.15f;
    public Vector3 offset = new Vector3(0, 0, -10);

    private Vector3 currentVelocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null) return;
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smooth);
    }
}