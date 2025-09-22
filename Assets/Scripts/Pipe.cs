using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pipe : MonoBehaviour
{
    public Transform connection;
    public InputActionReference enterAction;
    public Vector3 enterDirection = Vector3.down, exitDirection = Vector3.zero;

    private void OnEnable()
    {
        if (enterAction != null) enterAction.action.Enable();
    }

    private void OnDisable()
    {
        if (enterAction != null) enterAction.action.Disable();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (connection && other.CompareTag("Player"))
        {
            if ((enterAction != null && enterAction.action.WasPressedThisFrame()) ||
            (Keyboard.current != null && Keyboard.current.sKey.wasPressedThisFrame)) StartCoroutine(Enter(other.transform));
        }
    }

    private IEnumerator Enter(Transform player)
    {
        player.GetComponent<PlayerMovement>().enabled = false;

        Vector3 enteredPos = transform.position + enterDirection;
        Vector3 enteredScale = Vector3.one * 0.5f;

        yield return Move(player, enteredPos, enteredScale);
        yield return new WaitForSeconds(1f);

        Camera.main.GetComponent<SideScrolling>().SetUnderground(connection.position.y < 0f);

        if (exitDirection != Vector3.zero)
        {
            player.position = connection.position - exitDirection;
            yield return Move(player, connection.position + exitDirection, Vector3.one);
        }
        else
        {
            player.position = connection.position;
            player.localScale = Vector3.one;
        }

        player.GetComponent<PlayerMovement>().enabled = true;
    }

    private IEnumerator Move(Transform player, Vector3 endPos, Vector3 endScale)
    {
        float elapsed = 0f, duration = 1f;

        Vector3 startPos = player.position, startScale = player.localScale;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            player.position = Vector3.Lerp(startPos, endPos, t);
            player.localScale = Vector3.Lerp(startScale, endScale, t);
            elapsed += Time.deltaTime;

            yield return null;
        }

        player.position = endPos;
        player.localScale = endScale;
    }
}
