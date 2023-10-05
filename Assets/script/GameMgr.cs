using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Mesh;

public sealed class GameMgr : MonoBehaviour
{
    public GameObject player;
    
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
    }
    public GameObject GetCollierPlayer()
    {
        return player;
    }


}

