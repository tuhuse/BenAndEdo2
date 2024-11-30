using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ダメージを受けたときにパネルを動かす
/// </summary>
public class DamageUI : MonoBehaviour
{
    [SerializeField] private Image _damagePanel;
    private Animator _damageAnimator;
    // Start is called before the first frame update
    void Start()
    {
        _damageAnimator = GetComponent<Animator>();
    }

    public void StartDamageUI()
    {
        _damagePanel.enabled = true;
        _damageAnimator.Play(0);
    }
}
