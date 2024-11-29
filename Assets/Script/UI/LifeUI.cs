using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class LifeUI : MonoBehaviour
{
    [SerializeField] private Image[] _lifeSprite = default;
    [SerializeField] private Sprite _unLifeSprite = default;
    [SerializeField] private Sprite _lifeSpriteSourse = default;

    public void DamageLife(int playerHP)
    {
        switch (playerHP)
        {
            case 2:
                _lifeSprite[0].sprite = _unLifeSprite;
                break;
            case 1:
                _lifeSprite[1].sprite = _unLifeSprite;
                break;
            case 0:
                _lifeSprite[2].sprite = _unLifeSprite;
                break;

        }
    }
    public void HealLife(int playerHP)
    {
        switch (playerHP)
        {
            case 2:
                _lifeSprite[1].sprite = _lifeSpriteSourse;
                break;
            case 3:
                _lifeSprite[0].sprite = _lifeSpriteSourse;
                break;
            

        }
    }
}
