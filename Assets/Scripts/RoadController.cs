using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoadController : MonoBehaviour
{
    [SerializeField] private GameObject[] gasObjects;

    private void Start()
    {
        foreach (var gasObject in gasObjects)
        {
            gasObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        foreach (var gasObject in gasObjects)
        {
            gasObject.SetActive(false);
        }
    }

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

    //랜덤으로 가스를 표시하는 함수
    public void SpawnGas()
    {
        int index = Random.Range(0, gasObjects.Length);
        gasObjects[index].SetActive(true);
    }
}
