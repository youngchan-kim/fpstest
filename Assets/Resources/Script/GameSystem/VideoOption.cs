using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VideoOption : MonoBehaviour
{
    [SerializeField]
    FullScreenMode screenMode;
    public TMP_Dropdown resolution_Dropdown;
    public Toggle fullscreenBtn;

    //public Dropdown resolution_Dropdowns;
    //public GameObject Dropdownsss;
    //리스트의 타입은 Resolution로 List에 Resolution의 정보를 입력해줌
    List<Resolution> resolutions = new List<Resolution>();

    //처음 시작시 Dropdwon의 값이 초기화 되어 있지 않기 때문에 0으로 설정해줌
    public int resolutionNum;

    public GameObject Option_Menu;
    // Start is called before the first frame update
    void Start()
    {
        InitUI();
    }

    // Update is called once per frame
    void InitUI()
    {
        //리스트에 화면의 해상도
        resolutions.AddRange(Screen.resolutions);

        for (int i =0; i < Screen.resolutions.Length; i++)
        {
            if(Screen.resolutions[i].refreshRate == 60)
                resolutions.Add(Screen.resolutions[i]);
        }
        /*해상도와 프레임을 확인하기 위해 사용
         foreach(Resolution item in resolutions)
         {
             Debug.Log(item.width + "x" + item.height + "y" + item.refreshRate+"프레임");
         }
        */
        //options은 리스트의 형태이기 때문에 클리어 후 사용한다.
        resolution_Dropdown.options.Clear();
        //resolutions로 받아온 Resolution의 정보를 Dropdown에 넣어야한다.
        //Dropdown은 options클래스로 되어 있으니 해당 클래스 리스트로 만든후 입력해준다.
        //
        
        int optionNum = 0;

        foreach (Resolution item in resolutions)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = item.width + "x" + item.height + " " + item.refreshRate + "hz";
            resolution_Dropdown.options.Add(option);
            if (item.width == Screen.width && item.height == Screen.height)
                resolution_Dropdown.value = optionNum;
            optionNum++;
        }
        resolution_Dropdown.RefreshShownValue();
        fullscreenBtn.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
    }
    public void DropboxOption(int x)
    {
        resolutionNum = x;
    }

    //토글버튼
    public void FullScreenBtn(bool isFull)
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        OkBtnClick();
    }

    //OK버튼
    public void OkBtnClick()
    {
        Screen.SetResolution(resolutions[resolutionNum].width,
            resolutions[resolutionNum].height, screenMode);

    }

    //back버튼
    public void BackBtnClick()
    {
        this.gameObject.SetActive(false);
        Option_Menu.SetActive(true);
    }
    //https://www.youtube.com/watch?v=_LK1YQ2ff2k
}
