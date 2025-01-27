using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_MovingPlatform : MonoBehaviour
{
    [SerializeField]
    private Transform activePlatform;            //������ł��鏰���擾

    CharacterController controller;             //CharacterController

    Vector3 moveDirection;                      //�ړ��ʁi���̈ړ��ʂ͊��S�Ɉړ����鏰�̂��߂̂��́j
    Vector3 activeGlobalPlatformPoint;          //�ړ����鏰�̃O���[�o���ʒu
    Vector3 activeLocalPlatformPoint;           //�ړ����鏰�̃��[�J���ʒu
    Quaternion activeGlobalPlatformRotation;    //�ړ����鏰�̃O���[�o����]
    Quaternion activeLocalPlatformRotation;     //�ړ����鏰�̃��[�J����]

    void Start()
    {
        controller = GetComponent<CharacterController>();  //CharacterController���擾
    }

    void Update()
    {
        if (activePlatform != null) //�����ړ����鏰���������ꍇ
        {
            Vector3 newGlobalPlatformPoint = activePlatform.TransformPoint(activeLocalPlatformPoint);   //�V�����O���[�o���ʒu
            moveDirection = newGlobalPlatformPoint - activeGlobalPlatformPoint;                         //�x�N�g���̈����Z�A�O�̈ʒu���獡�̈ړ����Ɏw���x�N�g�������߂�
            if (moveDirection.magnitude > 0.001f)   //�����ω��ʂ��������ꍇ
            {
                controller.Move(moveDirection);     //���̕����ړ�����
            }
            if (activePlatform) //�ړ�������ɁA���̏��͂܂��擾�ł���ꍇ
            {
                // ��]�ɂ��Ή�
                Quaternion newGlobalPlatformRotation = activePlatform.rotation * activeLocalPlatformRotation;           //�V�����O���[�o����]
                Quaternion rotationDiff = newGlobalPlatformRotation * Quaternion.Inverse(activeGlobalPlatformRotation);
                
                rotationDiff = Quaternion.FromToRotation(rotationDiff * Vector3.up, Vector3.up) * rotationDiff;         //��]���������߂�
                transform.rotation = rotationDiff * transform.rotation;                                                 //���̉�]���������ď㏑��
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);                                     //�L�����N�^�[�̉�]�𐅕��ɕۂ�
                                                                                                                      
                UpdateMovingPlatform();
            }         
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //�����Ə�𓥂�ł���̂��m�F
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
        //�����̈ʒu���A
        activeGlobalPlatformPoint = transform.position;                                                     //�O���[�o��
        activeLocalPlatformPoint = activePlatform.InverseTransformPoint(transform.position);                //���[�J��
        //�����̉�]���
        activeGlobalPlatformRotation = transform.rotation;                                                  //�O���[�o��
        activeLocalPlatformRotation = Quaternion.Inverse(activePlatform.rotation) * transform.rotation;     //���[�J��
    }
}