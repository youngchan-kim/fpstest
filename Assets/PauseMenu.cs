using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPause = false;
    [SerializeField]
    public GameObject Pause_Menu;
    public GameObject Option_Menu;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPause) Pause();
            else Resume();
        }
    }

    public void Resume()
    {
        Pause_Menu.SetActive(GameIsPause);
        Time.timeScale = 1f;
        GameIsPause = true;
        Debug.Log("게임 재개");
    }
    public void Pause()
    {
        Setting_Pause_Menu();
        Time.timeScale = 0f;
        GameIsPause = false;
        Debug.Log("일시 정지");
    }
    public void Setting_Option_Menu()
    {
        Pause_Menu.SetActive(false);
        Option_Menu.SetActive(true);
    }
    public void Setting_Pause_Menu()
    {
        Pause_Menu.SetActive(true);
        Option_Menu.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
