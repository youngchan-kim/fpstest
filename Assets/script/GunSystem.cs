
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GunSystem : MonoBehaviour
{
    //최대 사거리값 필요
    public Gun_Scriptable Gun_property;

    //GunPickUp pickup = null;

    //공격이 시작되는 지점
    //총의 muzzle오브젝트
    public Transform GunMuzzle;

    AimShaker MuzzleAimShaker;
    //공격 할 지점
    //총의 muzzle오브젝트로부터 직선 방향 최대값 필요
    Transform attackPoint;
    
    //총이 맞는 곳의 이펙트
    /*[SerializeField]
    public GameObject bulletHoleGraphic;*/


    //GameMgr의 GUIMgr로 관리
    //남은 탄 표시
    [SerializeField]
    public TextMeshProUGUI text;

    //사격 이펙트
    //muzzle오브젝트 하위 오브젝트에 있음
    ParticleSystem muzzleFlashparticle;

    //남은 탄창, 쏠수 있는 탄
    int bulletsLeft, bulletsShot;

    //bools
    bool shooting, readyToShoot, reloading;
    bool shoot;

    //공격한 곳
    //??
    public RaycastHit rayHit;

    //총구가 가리키는 포지션
    //이미 muzzle로 구하면됨
    Vector3 target_position;
    //촐구가 바라보는 방향
    //muzzle의 방향이 바라보는 방향임
    public Vector3 direction;

    //public ItemObject bullet;
    float total_reloadTime;
    int total_magazineSize;
    float total_timeBetweenShooting;

    Player player;
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
        player = GameMgr.Instance.player.GetComponent<Player>();
        //pickup = GetComponent<GunPickUp>();
        //탄창사이즈 만큼 남은 탄을 채워준다.
        //bulletsLeft = 0;
        bulletsLeft = 3000;
        //쏠 수 있는 상태
        readyToShoot = true;
    }
    private void Start()
    {
        GunMuzzle = transform.GetChild(0).GetChild(0);
        MuzzleAimShaker = GunMuzzle.GetComponent<AimShaker>();
        //Debug.Log(GunMuzzle);
        muzzleFlashparticle = transform.GetChild(0).GetChild(1).GetComponent<ParticleSystem>();
        attackPoint = GunMuzzle.GetChild(0);
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
        //총구가 바라보는 방향 선으로 확인
        Debug.DrawLine(GunMuzzle.position, GunMuzzle.position + GunMuzzle.forward*len, Color.red);
        attackPoint.position = GunMuzzle.position + GunMuzzle.forward * len;
        
        //Debug.Log("어택 포인트가 가리키는 좌표 "+attackPoint.transform.position);
        //GunMuzzle.LookAt(MuzzleAimShaker.GetLookpoint());
        //Debug.Log(GunMuzzle.position + GunMuzzle.forward * len);
        //이미 바라보는 곳임으로 LookAt으로 다시 바라볼 필요가 없음
        //GunMuzzle.LookAt(GunMuzzle.position + GunMuzzle.forward * len);

        //바라보는 방향은 GunMuzzle.forward
        //바라보는 곳의 좌표는 GunMuzzle.position + GunMuzzle.forward * len

        MyInput();

    }
    private void MyInput()
    {
        //연사와 단발을 B키를 통해 조작할 수 있다.
        if (Input.GetKeyDown(KeyCode.B))
            if (Gun_property.allowButtonHold)
                Gun_property.allowButtonHold = false;
            else Gun_property.allowButtonHold = true;
        //if (!Input.GetKeyUp(KeyCode.Mouse0))
        //버튼 눌림 체크가 false이면 shooting은 눌렸을 때 ture;
        //연사 가능 일때
        if (Gun_property.allowButtonHold)
        {
            
            shooting = Input.GetKey(KeyCode.Mouse0);
            //Debug.Log("연사"+shooting);
            if (Input.GetKey(KeyCode.Mouse0) && shoot)
            {
                //반동
                //StartCoroutine(GunMuzzle.GetComponent<AimShaker>().AimShake(Gun_property.spread));
                muzzleFlashparticle.Play();
                shoot = false;
            }
        }
        //연사 불가는 일때
        else
        {
            
            shooting = Input.GetKeyDown(KeyCode.Mouse0);
            //Debug.Log("단발" + shooting);
            if (Input.GetKeyDown(KeyCode.Mouse0) && shoot)
            {
                //StartCoroutine(GunMuzzle.GetComponent<AimShaker>().AimShake(Gun_property.spread));
                muzzleFlashparticle.Play();
                shoot = false;
            }

        }

        //R키가 눌렸고 남은 탄이 탄창보다 작고 리로드 중이 아닐때 호출한다.
        //if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < Gun_property.magazineSize && !reloading) Reload();

        //Shoot
        //쏠준비됨 혹은 슈팅중이고 재장전중이지 않으며 장전된 탄의 수가 0보다 클때
       
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = Gun_property.bulletsPerTap;
            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        //RayCast
        //1인칭 기준으로 만들기 때문
        //접촉한 단일 개체의 정보를 얻어오기 위함
        //특정 위치에서 일정한 방향으로 광선을 발사
        Vector3 firedirection = GunMuzzle.forward;
        if (Physics.Raycast(GunMuzzle.position, firedirection, out rayHit, Gun_property.range, Gun_property.what_can_Shoot))
        {
            //Debug.Log(rayHit.collider.name);
           /* if (rayHit.collider.GetComponent<Enemy>())
                rayHit.collider.GetComponent<DamegeSystem>().TakeDamage(Gun_property.damage);*/

            //var t = Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.LookRotation(rayHit.normal));

        }

        //Debug.DrawLine(firePosition.transform.position, rayHit.point, Color.blue);


        //Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
        //Instantiate(muzzleFlashparticle, attackPoint.transform.position, Quaternion.identity);

        bulletsLeft--;
        bulletsShot--;
        //총 쏘면서 바뀐 속성 리셋
        total_timeBetweenShooting = Gun_property.timeBetweenShooting + 0;
        Debug.Log(total_timeBetweenShooting);
        Invoke("ResetShot", total_timeBetweenShooting);

        //총을 쏠때 연사시간
        if (bulletsShot > 0 && bulletsLeft > 0)
        {
            Invoke("Shoot", Gun_property.timeBetweenShots);
        }
        shoot = false;
    }


    private void ResetShot()
    {
        readyToShoot = true;
        shoot = true;
    }


    /*private void Reload()
    {
        reloading = true;
        total_reloadTime = Gun_property.reloadTime + 0;
        Invoke("ReloadFinished", total_reloadTime);
    }*/


    /*private void ReloadFinished()
    {
        total_magazineSize = Gun_property.magazineSize + 0;
        int haveBullet = player.GetInven_Find_Item(bullet);
        int addBullet = 0;
        addBullet = total_magazineSize - bulletsLeft;

        if (haveBullet <= addBullet)
        {
            bulletsLeft = bulletsLeft + haveBullet;
            player.SetInven_Find_Item(bullet, 0);
        }
        else
        {
            bulletsLeft = addBullet;
            player.SetInven_Find_Item(bullet, haveBullet- addBullet);
        }
        reloading = false;
        player.InventRefresh();
    }*/

    /* public Sprite GetSprite()
     {
         return Gun_property.sprites;
     }*/

}
