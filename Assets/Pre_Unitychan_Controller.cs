using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pre_Unitychan_Controller : MonoBehaviour
{
    Rigidbody rigid;
    Animator animator;
    public float rotateForce = 200;    //回転量
    public float runForce = 600;       //前進量
    public float maxRunSpeed = 2;      //前進速度の制限
    public float jumpforce = 250;      //ジャンプ量

    void Start()
    {
        // 必要なコンポーネントを自動取得
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (rigid == null) return;  //物理運動が入っていない場合は終了

        //横方向の入力で方向転換する
        transform.Rotate(0, Input.GetAxis("Horizontal") * rotateForce * Time.deltaTime, 0);

        //ジャンプ
        if (Input.GetButtonDown("Jump"))
        {
            rigid.AddForce(transform.up * jumpforce);
            animator.SetTrigger("Jump");
        }

        //上方向の入力で進む（加速制限あり）
        bool isRun = Input.GetAxis("Vertical") > 0.01f; //入力があるかどうか
        Vector3 vel_xz = new Vector3(rigid.velocity.x, 0, rigid.velocity.z);
        if (isRun && vel_xz.magnitude < maxRunSpeed)
        {
            rigid.AddForce(transform.forward * Input.GetAxis("Vertical") * runForce * Time.deltaTime);
        }

        //走っているかどうかのアニメーション設定
        animator.SetBool("Run", isRun);

        //アニメーション速度を調整（アニメーション名で判別）
        string anim_name = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        if (anim_name.Contains("Run") && isRun)
        {
            animator.speed = rigid.velocity.magnitude / maxRunSpeed;
        }
        else
        {
            animator.speed = 1.0f;
        }
    }
}