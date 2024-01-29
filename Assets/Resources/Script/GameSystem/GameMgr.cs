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
    public GameObject enemy;
    [Header("UI")]
    [SerializeField]
    public GameObject GUI_Mgr;
    [Header("Item_Prefab")]
    [SerializeField]
    public GameObject Item_Mgr;

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
        enemy.GetComponent<Enemy>().Initialize();
        GUI_Mgr.GetComponent<GUI_Mgr>().Initialize();
        Item_Mgr.GetComponent<Item_Mgr>().Initialize();

    }
    public GameObject GetCollierPlayer()
    {
        return player;
    }


}

