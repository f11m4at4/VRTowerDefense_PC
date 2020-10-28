using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//랜덤한 시간에 한번씩 드론을 생성하고싶다
public class DroneManager : MonoBehaviour
{
    //랜덤 시간의 범위
    public float minTime = 1;
    public float maxTime = 5;
    //생성시간
    float createTime;
    //경과시간
    float currentTime;
    //드론 생성할 위치
    public Transform[] spawnPoints;
    //드론 공장
    public GameObject droneFactory;

    void Start()
    {
        // 생성시간을 랜덤범위에서 설정
        createTime = Random.Range(minTime, maxTime);
    }

    // Update is called once per frame
    void Update()
    {
        //1.시간이 흘러야 한다.
        currentTime += Time.deltaTime;
        //2.만약 경과 시간이 생성 시간을 초과 하였다면
        if (currentTime > createTime)
        {
            //3.드론 생성
            GameObject drone = Instantiate(droneFactory);
            //4.드론 위치 설정
            // 랜덤으로 spawnPoints 중 하나를 뽑는다.
            int index = Random.Range(0, spawnPoints.Length);
            // 드론의 위치를 랜덤으로 뽑힌 spawnPoint 의 위치로 할당
            drone.transform.position = spawnPoints[index].position;
            //5.경과시간 초기화
            currentTime = 0;
            //6.생성시간 재 할당
            createTime = Random.Range(minTime, maxTime);
        }
    }
}
