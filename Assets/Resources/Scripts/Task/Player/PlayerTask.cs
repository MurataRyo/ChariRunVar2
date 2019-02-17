using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTask : MonoBehaviour, IGravity
{
    #region プレハブ
    private GameObject swordPrefab;
    private GameObject jumpEffect;
    #endregion

    //バランス調整用
    private const float GRAVITY_SIZE = -9.81f;      //重力加速度
    private const float SPEED = 3f;                 //スピード
    private const float JUMP_POWER = 7f;            //ジャンプ力
    private const float END_JUMP_POWER_MAX = 4f;    //終了時のジャンプ力の最大

    private const float JUMP_INTERVAL = 0.3f;       //ジャンプ直後には設置判定を行わないための時間
    private float jumpTimer = 0f;                   //↑の計測用

    private float gravity = 0f;                     //受けている重力
    private bool isGround = false;                  //設置しているかどうか

    public int skyJumpNum;
    public int skyJumpNumMax = 5;

    private const float GROUND_RAY_RANGE = 0.05f;

    //色々取得用
    private Rigidbody2D rBody;
    private BoxCollider2D boxCol;
    private ButtonTask buttonTask;

    // Start is called before the first frame update
    private void Start()
    {
        skyJumpNum = 0;
        rBody = GetComponent<Rigidbody2D>();
        boxCol = GetComponent<BoxCollider2D>();
        buttonTask = Utility.GetButton();
        swordPrefab = Resources.Load<GameObject>(GetPath.GamePrefab + "/Sword");
        jumpEffect = Resources.Load<GameObject>(GetPath.GamePrefab + "/JumpEffect");
    }

    // Update is called once per frame
    private void Update()
    {
        isGround = Groundif();
        GravityChange();
        SwprdIf();

        if (jumpTimer > 0)
        {
            jumpTimer -= Time.deltaTime;
        }
        JumpAction();

        rBody.velocity = new Vector2(SPEED, Gravity());
    }

    #region 剣関係
    //生成できるかどうか
    private void SwprdIf()
    {
        if (!buttonTask.ButtonDownIf(ButtonTask.Name.Attack))
            return;

        foreach (Transform child in transform)
        {
            if (child.tag == GetTag.Sword)
                return;
        }

        CreateSword();
    }

    //生成
    private void CreateSword()
    {
        GameObject go = Instantiate(swordPrefab);
        go.transform.parent = transform;
    }
    #endregion

    #region 動き関係
    //設置判定※Rayでとっている
    private bool Groundif()
    {
        //上に飛んでいるかジャンプ直後なら必ず空中判定
        if (Gravity() > 0f || jumpTimer > 0)
            return false;

        Vector2 castSize = new Vector2(boxCol.size.x, 0.01f) / 2;
        float rayRange = rBody.velocity.y * Time.fixedDeltaTime > GROUND_RAY_RANGE ? rBody.velocity.y * Time.fixedDeltaTime : GROUND_RAY_RANGE;
        foreach (RaycastHit2D hit2D in Physics2D.BoxCastAll(transform.position, castSize, 0f, Vector2.down, rayRange))
        {
            if (hit2D.collider.gameObject.tag == GetTag.Block)
            {
                if (!IsGround())
                    SkyToGround(hit2D);

                return true;
            }
        }
        return false;
    }

    //空中から地上にいった時
    private void SkyToGround(RaycastHit2D hit)
    {
        JumpReset();
        //当たった場所の少し上からレイを飛ばして地面の一番上に行くようにする
        //                                                                          1は1ブロックの最大が1mだから
        foreach (RaycastHit2D hit2D in Physics2D.RaycastAll(hit.point + new Vector2(0f, 1f), Vector2.down, 1f))
        {
            if (hit.collider == hit2D.collider)
            {
                transform.position = new Vector2(transform.position.x, hit2D.point.y);
                return;
            }
        }
    }

    //ジャンプ回数リセット
    private void JumpReset()
    {
        skyJumpNum = 0;
    }

    private bool jumpIf()
    {
        if (!IsGround() && skyJumpNum >= skyJumpNumMax)
            return false;

        return true;
    }

    private void JumpAction()
    {
        //ボタンを押したときに判定
        if (!buttonTask.ButtonDownIf(ButtonTask.Name.Jump) ||
        //ジャンプ可能かどうか判定
            !jumpIf())
            return;

        //空中ジャンプの判定
        if (!IsGround())
        {
            SkyJump();
        }
        Jump();
    }

    private void Jump()
    {
        jumpTimer = JUMP_INTERVAL;
        gravity = JUMP_POWER;
    }

    //空中ジャンプ
    private void SkyJump()
    {
        Instantiate(jumpEffect);
        skyJumpNum++;
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        OnHedBlock(col);
    }

    //頭に物が当たっているかどうかの判定当たっていれば重力を０にする
    private void OnHedBlock(Collision2D col)
    {
        if (Gravity() < 0)
            return;
        foreach (ContactPoint2D point2D in col.contacts)
        {
            if (point2D.normal.y < 0)
            {
                gravity = 0;
                break;
            }
        }
    }

    #endregion

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

        if (buttonTask.ButtonUpIf(ButtonTask.Name.Jump) && Gravity() > END_JUMP_POWER_MAX)
            gravity = END_JUMP_POWER_MAX;
    }

    public bool IsGround()
    {
        return isGround;
    }

    #endregion

    #endregion
}
