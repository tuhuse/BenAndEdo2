using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// プレイヤーに何をやってほしいか伝えるUI
/// </summary>
public class MissionUI : MonoBehaviour
{

    [SerializeField] private Text[] _missionText;
   
    public static MissionUI Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
        else
        {
            Destroy(gameObject); // 重複するインスタンスを破棄
        }
    }
    private void Start()
    {
        Mission();
    }
    private void Mission()
    {
        _missionText[0].enabled = true;
    }
    /// <summary>
    /// 鍵を見つけろ
    /// </summary>
    public void Mission1()
    {
        _missionText[0].enabled = false;
        _missionText[1].enabled = true;
    }
    /// <summary>
    /// 右クリックして赤いかぎを作れ
    /// </summary>
    public void Mission2()
    {
        _missionText[1].enabled = false;
        _missionText[2].enabled = true;
    }
    /// <summary>
    /// 金庫を開けろ
    /// </summary>
    public void Mission3()
    {
        _missionText[2].enabled = false;
        _missionText[3].enabled = true;
    }
   /// <summary>
   /// ボタンを押せ
   /// </summary>
    public void Mission4()
    {
        _missionText[3].enabled = false;
        _missionText[4].enabled = true;
    }
    /// <summary>
    /// 出口が開いたにげろ
    /// </summary>
    public void Mission5()
    {
        _missionText[4].enabled = false;
        _missionText[5].enabled = true;
    }
}
