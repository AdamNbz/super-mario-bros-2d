using UnityEngine;

public class SideScrolling : MonoBehaviour
{
    private Transform player;

    public float height = 6.5f, undergroundHeight = -9.5f;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        Vector3 camPos = transform.position;
        camPos.x = Mathf.Max(camPos.x, player.position.x);
        transform.position = camPos;
    }

    public void SetUnderground(bool underground)
    {
        Vector3 camPos = transform.position;
        camPos.y = underground ? undergroundHeight : height;
        transform.position = camPos;
    }
}
