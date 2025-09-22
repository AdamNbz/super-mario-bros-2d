using UnityEngine;
using System.Collections;

public class BlockItem : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        CircleCollider2D physicsCollider = GetComponent<CircleCollider2D>();
        BoxCollider2D triggerCollider = GetComponent<BoxCollider2D>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        rb.bodyType = RigidbodyType2D.Kinematic;
        physicsCollider.enabled = false;
        triggerCollider.enabled = false;
        spriteRenderer.enabled = false;

        yield return new WaitForSeconds(0.25f);

        spriteRenderer.enabled = true;

        float elapsed = 0f, duration = 0.5f;

        Vector3 startPos = transform.localPosition, endPos = transform.localPosition + Vector3.up * 0.5f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            transform.localPosition = Vector3.Lerp(startPos, endPos, t);
            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = endPos;

        rb.bodyType = RigidbodyType2D.Dynamic;
        physicsCollider.enabled = true;
        triggerCollider.enabled = true;
    }
}
