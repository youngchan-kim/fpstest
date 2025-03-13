using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundOption : MonoBehaviour
{
    public GameObject Option_Menu;
    public Slider slider;
    public SoundManager smg;

    //OK버튼
    //음량을 저장
    public void OkBtnClick()
    {
        smg.musicsource.volume = slider.value;
    }

    //back버튼
    public void BackBtnClick()
    {
        this.gameObject.SetActive(false);
        Option_Menu.SetActive(true);
    }

}
