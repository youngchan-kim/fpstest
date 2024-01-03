using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float horizontalInput;
    public float verticalInput;

    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump = true;

    [Header("Keybords")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;


    Vector3 moveDirection;

    Rigidbody rb;

    Player_Action anim;
    //총쏘는 애니매이션 테스트용
    
    bool aiming;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        anim = GetComponent<Player_Action>();
        orientation = transform.GetChild(0).GetChild(1).GetChild(2);
    }

    // Update is called once per frame
    private void Update()
    {
        //여기 있을 코드가 아니지만 테스트를 위함
        //조준 모션
        if (Input.GetMouseButtonDown(1))
        {
            aiming = anim.Aiming();
            
        }
        //공격 모션
        if (aiming)
        {
            if (Input.GetMouseButtonDown(0)) anim.Attacking();
            else if(Input.GetMouseButtonUp(0)) anim.Attacking();

        }
        //재장전 모션
        if (Input.GetKeyDown(KeyCode.R))
        {
            anim.Reloading();
            StartCoroutine(ReloadTimer());
        }
        //아이템 루팅 모션
        if (Input.GetKeyDown(KeyCode.F))
        {
            anim.PickUpMotion();
            StartCoroutine(PickupTimer());
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            anim.Drink_A_Heal();
            StartCoroutine(DrinkTimer());
        }

        //ground check
        grounded = Physics.Raycast(transform.position, Vector3.down/*, out RaycastHit a*/,playerHeight * 0.5f + 0.2f, whatIsGround);

        
        MyInput();

        //드래그 핸들
        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
           // Debug.Log("땅이 아닙니다.");
            rb.drag = 0;
        }

    }
    
    private void FixedUpdate()
    {
        MovePlayer();
        SpeedControl();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput =   Input.GetAxisRaw("Vertical");
        anim.MoveAnim(verticalInput, horizontalInput);
        //when to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded) 
        {
            readyToJump = false;

            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        if (Input.GetKeyDown(KeyCode.C) && grounded)
        {
            anim.Crouch();
        }
    }
    private void MovePlayer()
    {

        //움직이는 방향을 계산
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
       

        //땅위에 있을때
        if(grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        //공중에 있을때
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f*airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        //케릭터의 벡터의 방향과 좌표값을가진다.
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //limit velocity if needed
        //케릭터의 벡터의 평균이 moveSpeed보다 클때 적용
        if(flatVel.magnitude > moveSpeed)
        {
            //최고 속도는 케릭터의 정규화백터에 무브스피트를 곱해준다. 
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            //케릭터의 속도에 제한된 속도값을 넣어준다.
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        //reset y velocity
        //항상 정확히 같은 높이로 점프하기 위함
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        //ForceMode.Impulse는 힘을 한번만 적용해준다.
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        Debug.Log("점프를 누를수 있습니다." + jumpCooldown+"초 지났습니다.");
        readyToJump = true;  
    }
    IEnumerator JumpingTimer()
    {
        yield return new WaitForSeconds(0.5f);
        anim.IsJump();
        StopCoroutine(JumpingTimer());
    }
    IEnumerator DrinkTimer()
    {
        yield return new WaitForSeconds(4f);
        anim.Drink_A_Heal();
        StopCoroutine(DrinkTimer());
    }
    IEnumerator PickupTimer()
    {
        yield return new WaitForSeconds(2f);
        anim.PickUpMotion();
        StopCoroutine(PickupTimer());
    }
    IEnumerator ReloadTimer()
    {
        yield return new WaitForSeconds(3f);
        anim.Reloading();
        StopCoroutine(ReloadTimer());
    }
}
