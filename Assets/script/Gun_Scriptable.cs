
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(fileName = "New Gun", menuName = "Gun System/Gun")]
public class Gun_Scriptable : ScriptableObject
{
    //총의 아이디
    public int Id;
    //GunSystem의 속성
    public int damage;
    //총의 제어권
    public float timeBetweenShooting;
    //확산
    public float spread;
    //범위
    public int range;
    //재장전 시간
    public float reloadTime;
    //연사속도
    public float timeBetweenShots;
    //탄창의 사이즈
    public int magazineSize;
    //한번 누를때 발사하는 총알의 수
    public int bulletsPerTap;

    //버튼이 눌렸는지 확인하기 위함
    public bool allowButtonHold;

    public int Equipped_parts_Scopes;

    //장착시 아이템 이미지
    [SerializeField]
    public Sprite sprites;
    
    //공격할 수 있는 것들
    public LayerMask what_can_Shoot;
}
