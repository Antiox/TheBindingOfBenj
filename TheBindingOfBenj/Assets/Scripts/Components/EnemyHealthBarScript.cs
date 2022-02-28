using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarScript : MonoBehaviour
{
    [SerializeField] private Image _barImage;
    [SerializeField] private Image _damagedBarImage;

    private const float DAMAGED_HEALTH_TIMER_MAX = 0.6f;

    private float damagedHealthShrinkTimer;

    private void Update()
    {
        damagedHealthShrinkTimer -= Time.deltaTime;
        if (damagedHealthShrinkTimer < 0)
        {
            if (_barImage.fillAmount < _damagedBarImage.fillAmount)
            {
                _damagedBarImage.fillAmount -= 1.5f * Time.deltaTime;
            }
        }
    }

    public void SetHealth(float healthNormalized)
    {
        _barImage.fillAmount = healthNormalized;
        damagedHealthShrinkTimer = DAMAGED_HEALTH_TIMER_MAX;
    }
}
