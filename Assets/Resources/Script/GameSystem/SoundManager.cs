using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public TMP_InputField volumeInputField;
    public Slider slider;
    public AudioSource musicsource;
    //실린더로 음량 조절
    //bug 실린더로 조절한 음량이 적용이 되지 않음
    //ESC를 누르면 조절은 됨
    public void SetMussicVolume(float volume)
    {
        musicsource.volume = volume;
        volumeInputField.text = Mathf.Clamp(volume, 0f, 1f).ToString();
    }
    
    public void ShowVolume()
    {
        volumeInputField.text = musicsource.volume.ToString();
    }
    public void SetVolumeFromInput()
    {
        float volume;
        //입력된 텍스트를 float으로 변환
        if (float.TryParse(volumeInputField.text, out volume))
        {
            //0에서 1사이 값으로 변환
            volume = Mathf.Clamp(volume, 0f, 1f);
            musicsource.volume = volume;// AudioSource의 볼륨 설정
            slider.value = volume;
        }
        else
        {
            Debug.LogError("(음량)유효하지 않은 숫자 입니다.");
        }
    }

}
