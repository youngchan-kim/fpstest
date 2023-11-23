using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChagedIcon : MonoBehaviour
{
    public Sprite[] sprites;
    Sprite image;
    private void Start()
    {
        image = sprites[0];
    }
    void SetChagedSprite(int num)
    {
        switch(num)
        {
            case 0:
                image = sprites[num];
                break;
            case 1:
                image = sprites[num];
                break;
            case 2:
                image = sprites[num];
                break;
        }
    }
}
