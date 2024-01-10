using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save_N_Load : MonoBehaviour
{
    Player player;
    GunSystem gun;
    // Start is called before the first frame update
    void Start()
    {
        player = GameMgr.Instance.player.GetComponent<Player>();
    }
    void Save()
    {
        player.inventory.Save();
        player.Guninventory.Save();
    }
    void Load()
    {
        player.inventory.Load();
        player.Guninventory.Load();
    }
 /*   void Load_to_Gun()
    {
        gun = player.GetGunSystem();
        if (gun != null)
            if(==gun.Gun_property.Id)
    }*/
}
