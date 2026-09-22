using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;
    public Vector3 Offset;
    private float smoothTime = 0.1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 deiredPos = Target.position + Offset;
        this.transform.position = Vector3.Lerp(deiredPos, this.transform.position, smoothTime * Time.deltaTime);

    }
}
