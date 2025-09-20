using UnityEngine;

public class SideScrolling : MonoBehaviour
{
    private Transform player;

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
}
