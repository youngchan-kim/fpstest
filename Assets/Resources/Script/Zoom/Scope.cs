using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

public class Scope : MonoBehaviour
{
    enum Scopes
    {
        scope,
        holo,
        reddot
    };
    public Animator animator;

    //public GameObject scopeOverlay;
    public Camera mainCamera;
    public GameObject scope_parts;
    private CinemachineVirtualCamera Cam;

    public float scopedFOV;
    private float normalFOV;
    private bool isScoped;
    private void Start()
    {
        isScoped = false;
        Cam = GetComponent<CinemachineVirtualCamera>();
        //if (gameObject.transform.GetChild(0).GetChild(1).gameObject.activeSelf)
            //mainCamera = transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<CinemachinePipeline>();
        //Debug.Log(gameObject.transform.GetChild(0).GetChild(1).gameObject.activeSelf);
        //Debug.Log(gameObject.transform.GetChild(0).GetChild(1).name);
        //
    }
    void Update()
    {
        //마우스 오른 버튼이 눌리면 
        if (Input.GetMouseButton(1)) StartCoroutine(OnScoped());
        else OnUnScoped();

        //animator.SetBool("is Scoped", isScoped);
    }
    public void OnUnScoped()
    {
        isScoped = false;
        scope_parts.SetActive(false);
        //scopeOverlay.SetActive(false);

        //mainCamera.fieldOfView = normalFOV;
    }

    IEnumerator OnScoped()
    {
        isScoped = true;
        yield return new WaitForSeconds(.25f);
        scope_parts.SetActive(true);
        //normalFOV = mainCamera.fieldOfView;
        scopedFOV = 15f;
        /*if (scope_parts.GetComponentInChildren<Transform>().Find("scope"))
            scopeOverlay.SetActive(true); 
        else
            scopeOverlay.SetActive(false);*/


        //mainCamera.fieldOfView = scopedFOV;
    }
}
