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
    LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    //moveDirection 움직이는 방향?
    Vector3 moveDirection;

    Rigidbody rb;

    Player_Action anim;
    //총쏘는 애니매이션 테스트용

    bool aiming;

    public Transform playerCamera;

    private void Start()
    {
        whatIsGround = LayerMask.GetMask("Ground");
        //orientation가 항상 카메라가 바라보는 방향을 따라가게됨
        orientation.rotation = Quaternion.Euler(0, playerCamera.eulerAngles.y, 0);

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        anim = GetComponent<Player_Action>();
        //현재 문제 되는 코드
    }



    // Update is called once per frame
    private void Update()
    {

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


        if (Input.GetKeyDown(KeyCode.J))
        {
            anim.Drink_A_Heal();
            StartCoroutine(DrinkTimer());
        }

        //ground check
        // 바닥 감지를 CheckSphere()로 변경 → 더 정확하고 안정적!
        // 이동 로직 유지(땅에서는 즉각, 공중에서는 힘을 약하게 적용)
        // 불필요한 Raycast 제거 → 성능 최적화
        //
        //grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);


        grounded = Physics.CheckSphere(transform.position - Vector3.up * (playerHeight * 0.98f), 1.0f, whatIsGround);

        // 바닥에서는 드래그 적용
        rb.drag = grounded ? groundDrag : 0;

        MyInput();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        SpeedControl();

        // 최신 moveDirection 기준으로 선 그리기
        Debug.DrawLine(transform.position, transform.position + moveDirection * 10, Color.yellow);
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
        //추가된 코드 Vector3 camForward, Vector3 camRight,  moveDirection
        Vector3 camForward = new Vector3(playerCamera.forward.x, 0, playerCamera.forward.z).normalized;
        Vector3 camRight = new Vector3(playerCamera.right.x, 0, playerCamera.right.z).normalized;
        moveDirection = camForward * verticalInput + camRight * horizontalInput;
        //기존 코드
       /* //움직이는 방향을 계산
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        Debug.Log(moveDirection);
       */
        Debug.DrawLine(transform.position, transform.position + moveDirection *10, Color.yellow);


        if (grounded)
        {
            Debug.Log("땅");
            // 땅에서는 즉각적인 반응
            rb.velocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, moveDirection.z * moveSpeed);
        }
        else
        {
            // 공중에서는 이동이 제한되도록 힘을 적게 가함
            rb.AddForce(moveDirection.normalized * moveSpeed * airMultiplier, ForceMode.Force);
        }
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

    //아이템 루팅 모션
    public void Pickup_Item_Motion()
    {
        anim.PickUpMotion();
        StartCoroutine(PickupTimer());
    }

    IEnumerator JumpingTimer()
    {
        yield return new WaitForSeconds(0.5f);
        anim.IsJump();
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
        yield return new WaitForSeconds(1.5f);
        anim.Reloading();
        StopCoroutine(ReloadTimer());
    }
}
