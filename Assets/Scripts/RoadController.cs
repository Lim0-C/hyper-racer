using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoadController : MonoBehaviour
{
    
    //플레이어 차량이 도로에 진입하면 다음 도로를 생성
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.SpawnRoad(transform.position + new Vector3(0,0,10));
        }
    }
    //플레이어 차량이 도로를 벗어나면 해당 도로를 제거
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.DestoryRoad(gameObject);
        }
    }
}
