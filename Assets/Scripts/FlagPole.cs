using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FlagPole : MonoBehaviour
{
    public Transform flag, poleBottom, castle;
    public float speed = 6f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(MoveTo(flag, poleBottom.position));
            StartCoroutine(LevelCompleteSequence(other.transform));
        }
    }

    private IEnumerator LevelCompleteSequence(Transform player)
    {
        player.GetComponent<PlayerMovement>().enabled = false;

        yield return MoveTo(player, poleBottom.position);
        yield return MoveTo(player, player.position - Vector3.right);
        yield return MoveTo(player, player.position + Vector3.right + Vector3.down);
        yield return MoveTo(player, castle.position);

        player.gameObject.SetActive(false);

        yield return new WaitForSeconds(2f);

        GameManager.instance.NextLevel();
    }

    private IEnumerator MoveTo(Transform subject, Vector3 pos)
    {
        while (Vector3.Distance(subject.position, pos) > 0.125f)
        {
            subject.position = Vector3.MoveTowards(subject.position, pos, speed * Time.deltaTime);
            yield return null;
        }

        subject.position = pos;
    }
}