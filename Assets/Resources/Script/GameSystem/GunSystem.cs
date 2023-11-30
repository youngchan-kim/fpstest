
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class GunSystem : MonoBehaviour
{
    //최대 사거리값 필요
    public Gun_Scriptable Gun_property;

    //GunPickUp pickup = null;

    //공격이 시작되는 지점
    //총의 muzzle오브젝트
    public Transform GunMuzzle;

    public AimShaker MuzzleAimShaker;
    //공격 할 지점
    //총의 muzzle오브젝트로부터 직선 방향 최대값 필요
    public Transform attackPoint;

    //총이 맞는 곳의 이펙트
    [SerializeField]
    public GameObject bulletHoleGraphic;


    //GameMgr의 GUIMgr로 관리
    //남은 탄 표시
    [SerializeField]
    public TextMeshProUGUI text;

    //사격 이펙트
    //muzzle오브젝트 하위 오브젝트에 있음
    public ParticleSystem muzzleFlashparticle;

    //남은 탄창, 쏠수 있는 탄
    int bulletsLeft, bulletsShot;

    //bools
    bool shootclick, readyToShoot, reloading;
    bool shoot = true;

    //공격한 곳
    //??
    public RaycastHit rayHit;

    //총구가 가리키는 포지션
    //이미 muzzle로 구하면됨
    Vector3 target_position;
    //촐구가 바라보는 방향
    //muzzle의 방향이 바라보는 방향임
    Vector3 direction;

    //public ItemObject bullet;
    float total_reloadTime;
    int total_magazineSize;
    float total_timeBetweenshootclick;

    //Player player;
    //스크립터블에 들어가야함
    int len = 100;
    public Transform GetMuzzleTr()
    {
        return GunMuzzle;
    }
    public Transform GetAttackPointtr()
    {
        return attackPoint;
    }

    private void Awake()
    {
        //player = GameMgr.Instance.player.GetComponent<Player>();
        //pickup = GetComponent<GunPickUp>();
        //탄창사이즈 만큼 남은 탄을 채워준다.
        //bulletsLeft = 0;
        bulletsLeft = 3000;
        //쏠 수 있는 상태
        readyToShoot = true;
    }
    private void Start()
    {
        //Debug.Log(transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.GetChild(1).name);
        attackPoint = transform.parent.parent.parent.parent.parent.parent.parent
            .parent.parent.parent.parent.GetChild(1);
        GunMuzzle = transform.GetChild(0).GetChild(0);
        MuzzleAimShaker = GunMuzzle.GetComponent<AimShaker>();
      
        muzzleFlashparticle = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<ParticleSystem>();
        //Debug.Log(transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).name);

    }
    //public int CheckGunParts()
    //{
    //    return Equipped_parts_Scopes;
    //}

    private void Update()
    {
        //Debug.Log(GameMgr.Instance.player.GetComponent<Player>().GetEquipped());
        //Debug.Log(GameMgr.Instance.player.GetComponent<Player>().GetGunslotFull());
        /*if (GameMgr.Instance.player.GetComponent<Player>().GetGunSlotEmpty())
        {
            //필요가 없는 코드
            //transform.LookAt(attackPoint.transform.position);
            Debug.DrawLine(firePosition.transform.position, attackPoint.transform.position, Color.red);
            direction = transform.forward;
            firePosition.transform.forward = direction;

            MyInput();
            //SetText
            text.SetText(bulletsLeft + "/" + player.GetInven_Find_Item(bullet));
        }*/

        transform.LookAt(attackPoint.transform.position);
        Debug.DrawLine(GunMuzzle.position, GunMuzzle.position + GunMuzzle.forward * len, Color.red);

        //MyInput();
    }

    //총은 단발 연사관련 입력이 들어왔을때 실행되어야한다.
    public void AllowButtonHold()
    {
        if (Gun_property.allowButtonHold)
            Gun_property.allowButtonHold = false;
        else Gun_property.allowButtonHold = true;
    }
    public bool HoldButtonUse()
    {
        return Gun_property.allowButtonHold;
    }

    public void ClickToShoot(bool click)
    {
        shootclick = click;
    }
    public bool FiringIsPossible()
    {
        if (shoot && shootclick)
            return true;
        return false;
    }
    public bool ReadyToShoot()
    {
        if (readyToShoot && !reloading)
            return true;
        return false;
    }
    public bool BulletIsEmpty()
    {
        if (bulletsLeft > 0)
            return true;
        return false;
    }

    //단발인지 연사인지에 따라 조건으로 실해되어야함
    //발사가 입력되었을때 실행되어야함
    public void Firing()
    {
        //Debug.Log("호출");
        StartCoroutine(attackPoint.GetComponent<AimShaker>().AimShake(Gun_property.spread));
        if (!muzzleFlashparticle.isPlaying)
            muzzleFlashparticle.Play();
        shoot = false;
        //Shoot
        //쏠준비됨 혹은 슈팅중이고 재장전중이지 않으며 장전된 탄의 수가 0보다 클때
        bulletsShot = Gun_property.bulletsPerTap;
        Shoot();
    }


    private void MyInput()
    {
        //연사와 단발을 B키를 통해 조작할 수 있다.
        /*if (Input.GetKeyDown(KeyCode.B))
            if (Gun_property.allowButtonHold)
                Gun_property.allowButtonHold = false;
            else Gun_property.allowButtonHold = true;*/
        //if (!Input.GetKeyUp(KeyCode.Mouse0))
        //버튼 눌림 체크가 false이면 shootclick은 눌렸을 때 ture;
        //연사 가능 일때
        if (Gun_property.allowButtonHold)
        {
            shootclick = Input.GetKey(KeyCode.Mouse0);
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (FiringIsPossible() && ReadyToShoot() && BulletIsEmpty())
                {
                    // Firing();
                }
            }
        }
        //연사 불가는 일때
        else
        {
            shootclick = Input.GetKeyDown(KeyCode.Mouse0);
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (FiringIsPossible() && ReadyToShoot() && BulletIsEmpty())
                {
                    // Firing();
                }
            }

        }

        //R키가 눌렸고 남은 탄이 탄창보다 작고 리로드 중이 아닐때 호출한다.
        if (Input.GetKeyDown(KeyCode.R) && ReloadIsPossible()) Reload();
    }

    public bool ReloadIsPossible()
    {
        if (bulletsLeft < Gun_property.magazineSize && !reloading)
            return true;
        return false;
    }

    private void Shoot()
    {
        readyToShoot = false;

        //RayCast
        //1인칭 기준으로 만들기 때문
        //접촉한 단일 개체의 정보를 얻어오기 위함
        //특정 위치에서 일정한 방향으로 광선을 발사
        if (Physics.Raycast(GunMuzzle.position, GunMuzzle.forward, out rayHit, Gun_property.range, Gun_property.what_can_Shoot))
        {
            //일단 쏘고 맞은 대상이 데미지 시스템이 있는지 확인 할것
            //Debug.Log(rayHit.collider.name);
            if (rayHit.collider.GetComponent<DamegeSystem>())
            {

                //타겟의 DamegeSystem으로 데미지를 넣어줌
                rayHit.collider.GetComponent<DamegeSystem>().TakeDamage(Gun_property.damage);
                //Debug.Log("타겟에게 데미지를 입혔습니다.");
            }

            var t = Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.LookRotation(rayHit.normal));
        }

        bulletsLeft--;
        bulletsShot--;
        //총 쏘면서 바뀐 속성 리셋
        total_timeBetweenshootclick = Gun_property.timeBetweenshootclick + 0;
        //Debug.Log(total_timeBetweenshootclick);
        Invoke("ResetShot", total_timeBetweenshootclick);

        //총을 쏠때 연사시간
        if (bulletsShot > 0 && bulletsLeft > 0)
        {
            Invoke("Shoot", Gun_property.timeBetweenshootclick);
        }
        shoot = false;
    }


    private void ResetShot()
    {
        readyToShoot = true;
        shoot = true;
    }

    //장전시 호출
    public void Reload()
    {
        reloading = true;
        total_reloadTime = Gun_property.reloadTime + 0;
        Invoke("ReloadFinished", total_reloadTime);
    }


    private void ReloadFinished()
    {
        total_magazineSize = Gun_property.magazineSize + 0;
        //int haveBullet = player.GetInven_Find_Item(bullet);
        int addBullet = 0;
        addBullet = total_magazineSize - bulletsLeft;

        /*if (haveBullet <= addBullet)
        {
            bulletsLeft = bulletsLeft + haveBullet;
            //player.SetInven_Find_Item(bullet, 0);
        }
        else
        {
            bulletsLeft = addBullet;
            //player.SetInven_Find_Item(bullet, haveBullet- addBullet);
        }*/
        reloading = false;
        //player.InventRefresh();
    }

    /* public Sprite GetSprite()
     {
         return Gun_property.sprites;
     }
    */

}
