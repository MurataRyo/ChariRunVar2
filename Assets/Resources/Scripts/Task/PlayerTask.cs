using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTask : MonoBehaviour, IGravity
{
    //バランス調整用
    private const float GRAVITY_SIZE = -9.81f;  //重力加速度
    private const float SPEED = 3f;             //スピード
    private const float JUMP_POWER = 5f;        //ジャンプ力

    private const float JUMP_INTERVAL = 0.3f;   //ジャンプ直後には設置判定を行わないための時間
    private float jumpTimer = 0f;               //↑の計測用

    private float gravity = 0f;                 //受けている重力
    private bool isGround = false;              //設置しているかどうか

    //色々取得用
    private Rigidbody2D rBody;
    private BoxCollider2D boxCol;

    // Start is called before the first frame update
    private void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        boxCol = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        isGround = Groundif();
        GravityChange();

        if (jumpTimer > 0)
        {
            jumpTimer -= Time.deltaTime;
        }

        if (Button.JumpButton())
        {
            JumpAction();
        }

        rBody.velocity = new Vector2(SPEED, Gravity());
    }

    //設置判定※Rayでとっている
    private bool Groundif()
    {
        Vector2 castSize = new Vector2(boxCol.size.x, 0.01f) / 2;
        foreach (RaycastHit2D hit2D in Physics2D.BoxCastAll(transform.position, castSize, 0f, Vector2.down,0.01f))
        {
            if (hit2D.collider.gameObject.tag == GetTag.Block)
            {
                return true;
            }
        }
        return false;
    }

    //Jump可能かどうか
    private bool jumpIf()
    {
        if (!IsGround())
            return false;

        return true;
    }

    private void JumpAction()
    {
        if (!jumpIf())
            return;

        jumpTimer = JUMP_INTERVAL;
        gravity = JUMP_POWER;
    }

    #region インターフェイスの実装

    #region IsGround

    public float Gravity()
    {
        return gravity;
    }

    public float GravitySize()
    {
        return GRAVITY_SIZE;
    }

    public void GravityChange()
    {
        //設置判定ではないときかジャンプ直後だと重力を受ける
        gravity = IsGround() && jumpTimer <= 0 ? 0f : gravity + GRAVITY_SIZE * Time.deltaTime;
    }

    public bool IsGround()
    {
        return isGround;
    }

    #endregion

    #endregion
}
