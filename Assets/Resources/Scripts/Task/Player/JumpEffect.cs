using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpEffect : MonoBehaviour
{
    float alpha = 1f;
    const float ALPHA_TIME = 1f;
    SpriteRenderer spriteRenderer;
    GameObject player;
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag(GetTag.Player);
        transform.position = player.transform.position + new Vector3(0f, -0.1f, 0f);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        alpha = Mathf.MoveTowards(alpha, 0f, ALPHA_TIME * Time.deltaTime);

        if (alpha <= 0f)
        {
            Destroy(gameObject);
        }

        AlphaChange(alpha);
    }

    void AlphaChange(float a)
    {
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, a);
    }
}
