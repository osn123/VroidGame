using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_MovingPlatform : MonoBehaviour
{
    [SerializeField]
    private Transform activePlatform;            //今踏んでいる床を取得

    CharacterController controller;             //CharacterController

    Vector3 moveDirection;                      //移動量（この移動量は完全に移動する床のためのもの）
    Vector3 activeGlobalPlatformPoint;          //移動する床のグローバル位置
    Vector3 activeLocalPlatformPoint;           //移動する床のローカル位置
    Quaternion activeGlobalPlatformRotation;    //移動する床のグローバル回転
    Quaternion activeLocalPlatformRotation;     //移動する床のローカル回転

    void Start()
    {
        controller = GetComponent<CharacterController>();  //CharacterControllerを取得
    }

    void Update()
    {
        if (activePlatform != null) //もし移動する床があった場合
        {
            Vector3 newGlobalPlatformPoint = activePlatform.TransformPoint(activeLocalPlatformPoint);   //新しいグローバル位置
            moveDirection = newGlobalPlatformPoint - activeGlobalPlatformPoint;                         //ベクトルの引き算、前の位置から今の移動床に指すベクトルを求める
            if (moveDirection.magnitude > 0.001f)   //もし変化量があった場合
            {
                controller.Move(moveDirection);     //その分を移動する
            }
            if (activePlatform) //移動した後に、床の情報はまだ取得できる場合
            {
                // 回転にも対応
                Quaternion newGlobalPlatformRotation = activePlatform.rotation * activeLocalPlatformRotation;           //新しいグローバル回転
                Quaternion rotationDiff = newGlobalPlatformRotation * Quaternion.Inverse(activeGlobalPlatformRotation);
                
                rotationDiff = Quaternion.FromToRotation(rotationDiff * Vector3.up, Vector3.up) * rotationDiff;         //回転差分を求める
                transform.rotation = rotationDiff * transform.rotation;                                                 //床の回転を合成して上書き
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);                                     //キャラクターの回転を水平に保つ
                                                                                                                      
                UpdateMovingPlatform();
            }         
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //ちゃんと上を踏んでいるのを確認
        if (hit.moveDirection.y < -0.9 && hit.normal.y > 0.41)
        {
            if (activePlatform != hit.collider.transform)
            {
                activePlatform = hit.collider.transform;
                UpdateMovingPlatform();
            }
        }
        else
        {
            activePlatform = null;
        }
    }

    void UpdateMovingPlatform()
    {
        //自分の位置情報、
        activeGlobalPlatformPoint = transform.position;                                                     //グローバル
        activeLocalPlatformPoint = activePlatform.InverseTransformPoint(transform.position);                //ローカル
        //自分の回転情報
        activeGlobalPlatformRotation = transform.rotation;                                                  //グローバル
        activeLocalPlatformRotation = Quaternion.Inverse(activePlatform.rotation) * transform.rotation;     //ローカル
    }
}