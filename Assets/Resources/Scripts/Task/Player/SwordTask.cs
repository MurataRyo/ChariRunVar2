using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordTask : MonoBehaviour
{
    private const float SWORD_TIME = 0.3f;
    private const float INTERVAL_TIME = 0f;
    private GameObject player;

    public virtual void Start()
    {
        player = transform.parent.gameObject;
        transform.localPosition = new Vector3(0.273f, 0.45f, 0f);
        StartCoroutine(ATK());
    }

    private IEnumerator ATK()
    {
        while (true)
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, -179f, 180f * Time.deltaTime / SWORD_TIME);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, angle);

            yield return null;

            if (angle == -179f)
                yield return StartCoroutine(InterVal());
        }
    }

    private IEnumerator InterVal()
    {
        foreach(Collider2D col in GetComponents<Collider2D>())
        {
            col.enabled = false;
        }

        GetComponent<SpriteRenderer>().enabled = false;

        yield return new WaitForSeconds(INTERVAL_TIME);

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.GetComponent<EnemyTask>() != null)
        {

        }
    }
}
