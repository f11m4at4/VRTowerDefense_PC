﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    // 데미지 표현할 UI
    public Transform damageUI;
    public Image damageImage;

    // 깜빡 거림 시간
    public float damageTime = 0.1f;

    // 타워의 최초 HP
    public int initialHP = 10;
    // 내부 hp 변수
    int _hp = 0;

    // _hp 의 get/set 프로퍼티
    public int HP
    {
        get
        {
            return _hp;
        }
        set
        {
            _hp = value;
            // 기존 진행 중인 코루틴 해제
            StopAllCoroutines();
            // 깜빡거림을 처리할 코루틴 호출
            StartCoroutine(DamageEvent());
            // hp 가 0 이하이면 제거
            if(_hp <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    // Tower 의 싱글톤객체
    public static Tower Instance;

    void Awake()
    {
        // 싱글톤 객체 값 할당
        if(Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        _hp = initialHP;
        // 카메라의 nearClipPlane 값을 기억해 둔다.
        float z = Camera.main.nearClipPlane;
        // damageUI 객체의 부모를 카메라로 설정
        damageUI.parent = Camera.main.transform;
        // damageUI 의 위치를 X, Y 는 0, Z 값은 카메라의 near 값으로 설정
        damageUI.localPosition = new Vector3(0, 0, z);
        // damageImage 는 안보이도록 초기 비활성화 해 놓는다.
        damageImage.enabled = false;
    }

    // 데미지 처리를 위한 코루틴 함수
    IEnumerator DamageEvent()
    {
        // damageImage 컴포넌트를 활성화
        damageImage.enabled = true;
        // damageTime 만큼 기다린다.
        yield return new WaitForSeconds(damageTime);
        // 다시 원래 대로 비활성화 시켜준다.
        damageImage.enabled = false;
    }
}
