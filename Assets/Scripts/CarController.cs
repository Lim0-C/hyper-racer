using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private int gas = 100;
    [SerializeField] private float moveSpeed = 1f;
    
    public int Gas { get => gas;}

    void Start()
    {
        StartCoroutine(GasCoroutine());
    }

    IEnumerator GasCoroutine()
    {
        while (true)
        {
            gas -= 10;
            yield return new WaitForSeconds(1);
            if (gas <= 0) break;
        }
        //게임 종료
        GameManager.Instance.EndGame();
    }
    
    public void Move(float direction)
    {
        transform.Translate(Vector3.right * (direction * moveSpeed *Time.deltaTime));
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -2f, 2f), transform.position.y, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gas"))
        {
            gas += 30;
            
            //가스 아이템 숨기기
            other.gameObject.SetActive(false);
        }
    }
}
