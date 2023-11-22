using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [SerializeField]public GameObject slider;

    public void SetMaxHealth(float health)
    {
        slider.GetComponent<Slider>().maxValue= health;
        slider.GetComponent<Slider>().value = health;
    }
    public void SetHealth(float health)
    {
        slider.GetComponent<Slider>().value = health;
    }
}
