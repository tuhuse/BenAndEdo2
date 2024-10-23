using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] private GameObject _camera; // �J�����I�u�W�F�N�g
    [SerializeField] private Transform _legRespawnPosition; // �r�̃��X�|�[���ʒu
    [SerializeField] private Transform _headRespawnPosition; // ���̃��X�|�[���ʒu
    [SerializeField] private Transform _bodyRespawnPosition; // �̂̃��X�|�[���ʒu
    [SerializeField] private GameObject _leg; // �r�I�u�W�F�N�g
    [SerializeField] private GameObject _head; // ���I�u�W�F�N�g
    [SerializeField] private GameObject _body; // �̃I�u�W�F�N�g

    [SerializeField] private CameraManager _cameraManager;
    // �Փˎ��̏���
    private void OnCollisionEnter(Collision collision)
    {
        string player = "Player"; // �v���C���[�����ʂ���^�O

        // �Փ˂����I�u�W�F�N�g���v���C���[�̏ꍇ
        if (collision.gameObject.CompareTag(player))
        {
            // �R���[�`�����J�n���ėZ���������s��
            StartCoroutine(Fusion());
            // �����������Ă��Ȃ���Ԃɐݒ�
            _head.GetComponent<HeadController>()._isHeadAlive = false;

        }
    }

    // �Z���������s���R���[�`��
    private IEnumerator Fusion()
    {
        float waittime = 1f; // �e�X�e�b�v�ł̑ҋ@����
        yield return new WaitForSeconds(2f);

        // �̂�"�O��Ă���"��Ԃɐݒ�
        _body.GetComponent<BodyController>()._isUnBody = true;

        // �r�����X�|�[���ʒu�Ɉړ�
        _leg.transform.position = _legRespawnPosition.position;


        // �r������������ɃJ�������r�̈ʒu�Ɉړ�
        _cameraManager.SwitchBody(1);
        _camera.transform.rotation = Quaternion.Euler(20, 0, 0);

        // 1�b�ԑҋ@
        yield return new WaitForSeconds(waittime);

        // �̂����X�|�[���ʒu�Ɉړ�
        _body.transform.position = _bodyRespawnPosition.position;

        // �Ă�1�b�ԑҋ@
        yield return new WaitForSeconds(waittime);

        // �������X�|�[���ʒu�Ɉړ�
        _head.transform.position = _headRespawnPosition.position;

        // �r�����X�|�[�������鏈�������s
        _leg.GetComponent<LegController>().RespawnWait();
    }

}
