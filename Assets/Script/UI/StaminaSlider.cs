using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaSlider : MonoBehaviour
{
    [SerializeField] private Slider _staminaSlider;
    private ValueManager _valueManager;
    // Start is called before the first frame update
    void Start()
    {
        _valueManager = ValueManager.Instance;
        _staminaSlider.maxValue = _valueManager.DashHP;
    }

    // Update is called once per frame
    void Update()
    {
        _staminaSlider.value = _valueManager.DashHP;
    }
}
