using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class GameMgr : MonoBehaviour
{
    [Header("Player")]
    [SerializeField]
    public GameObject player;
    [Header("Enemy")]
    [SerializeField]
    public GameObject enumy;
    /*
    [Header("GUIMgr")]
    [SerializeField]
    public GUIMgr uiMgr;
    */
    static GameMgr instance = null;
    
    public static GameMgr Instance
    {
        get
        {
            if(!instance) 
            {
                instance = FindObjectOfType<GameMgr>();
                if (!instance) instance = new GameObject("GameManager").AddComponent<GameMgr>();

                instance.Initialize();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }
    private void Awake() { if (this != Instance) Destroy(gameObject); }
    private void Start()
    {
        Initialize();
    }
    void Initialize() 
    {
        StartGame();
    }
    void StartGame() 
    {
        player.GetComponent<Player>().Initialize();
        enumy.GetComponent<Enemy>().Initialize();
    }
    public GameObject GetCollierPlayer()
    {
        return player;
    }


}

