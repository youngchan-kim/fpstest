using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어가 바라보는 방향을 카메라가 바라보게 하기위함
//플레이어의 앞면을 가리키는 플레이어.foward 변수를 통해서 케릭터가 바라보는 방향을 알 수 있다. 
public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;

    public float rotationSpeed;

    public Transform RightLookAt;
    public Transform LeftLookAt;

    public GameObject thirdPersonCam;
    public GameObject rightWaveCam;
    public GameObject leftWaveCam;

    protected CameraStyle curentStyle;
    public enum CameraStyle
    {
        Basic,
        RightWave,
        LeftWave,
        OnePerson,
        Topdown
    }

    public CameraStyle GetCameraStyle()
    {
        return curentStyle;
    }

    //
    int wavecontrol = 0;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        wavecontrol = WaveControl(wavecontrol);
        //카메라 스타일 바꾸기
        if (wavecontrol == 0)
        {
            
            if (curentStyle == CameraStyle.RightWave)
                thirdPersonCam.transform.localPosition.Set(rightWaveCam.transform.position.x + 10, rightWaveCam.transform.position.y, rightWaveCam.transform.position.z);
            else if (curentStyle == CameraStyle.LeftWave)
                thirdPersonCam.transform.position.Set(leftWaveCam.transform.position.x - 10, leftWaveCam.transform.position.y, leftWaveCam.transform.position.z);
            SwitchCameraStyle(CameraStyle.Basic); 
        }
        else if (wavecontrol == 1 && curentStyle == CameraStyle.Basic)
        {
            rightWaveCam.transform.position.Set(thirdPersonCam.transform.position.x - 10, thirdPersonCam.transform.position.y, thirdPersonCam.transform.position.z);
            SwitchCameraStyle(CameraStyle.RightWave);
        }
        else if (wavecontrol == -1 && curentStyle == CameraStyle.Basic)
        {
            leftWaveCam.transform.position.Set(thirdPersonCam.transform.position.x + 10, thirdPersonCam.transform.position.y, thirdPersonCam.transform.position.z);
            SwitchCameraStyle(CameraStyle.LeftWave);
        }
        
        
        //카메라에서 플레이어까지의 방향을 계산하여 전방이 어디인지 알아내기
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);

        //카메라가 바라보는 방향은viewDir.normalized값을쓴다.
        orientation.forward = viewDir.normalized;

        // rotate player object
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (curentStyle == CameraStyle.Basic)
        {
            if (inputDir != Vector3.zero)
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);

        }
        else if (curentStyle == CameraStyle.RightWave)
        {
            Vector3 dirToRightWaveLookAt = RightLookAt.position - new Vector3(transform.position.x, RightLookAt.position.y, transform.position.z);
            orientation.forward = dirToRightWaveLookAt.normalized;
            playerObj.forward = dirToRightWaveLookAt.normalized;          
        }
        else if (curentStyle == CameraStyle.LeftWave)
        {
            Vector3 dirToLeftWaveLookAt = LeftLookAt.position - new Vector3(transform.position.x, LeftLookAt.position.y, transform.position.z);
            orientation.forward = dirToLeftWaveLookAt.normalized;

            playerObj.forward = dirToLeftWaveLookAt.normalized;
           
        }
        
    }

    private int WaveControl(int wavecontrol)
    {
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (wavecontrol >= 0)
                wavecontrol -= 1;
            else if (wavecontrol == -1)
                wavecontrol += 1;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (wavecontrol <= 0)
                wavecontrol += 1;
            else if (wavecontrol == 1)
                wavecontrol -= 1;
        }
        return wavecontrol;
    }

    private void SwitchCameraStyle(CameraStyle newStyle)
    {
        rightWaveCam.SetActive(false);
        leftWaveCam.SetActive(false);
        thirdPersonCam.SetActive(false);

        if (newStyle == CameraStyle.Basic) { thirdPersonCam.SetActive(true); }
        else if (newStyle == CameraStyle.RightWave) { rightWaveCam.SetActive(true);  }
        else if (newStyle == CameraStyle.LeftWave) { leftWaveCam.SetActive(true);  }
        curentStyle = newStyle;
    }

}
