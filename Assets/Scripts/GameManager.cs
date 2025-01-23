using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //프리팹
    [SerializeField] private GameObject carPrefab;
    [SerializeField] private GameObject roadPrefab;
    //UI 관련
    [SerializeField] private MoveButton leftButton;
    [SerializeField] private MoveButton rightButton;
    [SerializeField] private TMP_Text gasText;
    
    //자동차 
    private CarController _carController;
    
    //도로 오브젝트 풀
    private Queue<GameObject> _roadPool = new Queue<GameObject>();
    private int _roadPoolSize = 3;
    // 도로 이동
    private List<GameObject> _activeRoads = new List<GameObject>();
    private void Start()
    {
        InnitializeRoadPool();
        
        StartGame();
    }
    //싱글톤
    public static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }
            return _instance;
        }   
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    private void Update()
    {
        foreach (var activeRoad in _activeRoads)
        {
            activeRoad.transform.Translate(Vector3.back * Time.deltaTime);
        }
        
        //Gas 정보 출력
        if (_carController != null)
            gasText.text = _carController.Gas.ToString();
    }

    private void StartGame()
    {
        //도로 생성
        SpawnRoad(Vector3.zero);
        
        //자동차 생성
        _carController = Instantiate(carPrefab, new Vector3(0, 0, -3f), Quaternion.identity)
            .GetComponent<CarController>();
        
        //Left, Right movebutton에 자동차 컨트롤 기능 적용
        leftButton.OnMoveButtonDown += () =>
        {
            _carController.Move(-1f);
        };
        rightButton.OnMoveButtonDown += () =>
        {
            _carController.Move(1f);
        };
    }

    #region 도로 생성 및 관리
    //도로 오브젝트 풀 초기화
    private void InnitializeRoadPool()
    {
        for (int i = 0; i < _roadPoolSize; i++)
        {
            GameObject road = Instantiate(roadPrefab);
            road.SetActive(false);
            _roadPool.Enqueue(road);
        }
    }
    //도로 오브젝트 풀에서 불러와 배치하는 함수
    public  void SpawnRoad(Vector3 position)
    {
        if ((_roadPool.Count > 0))
        {
            GameObject road = _roadPool.Dequeue();
            road.SetActive(true);
            road.transform.position = position;
            
            _activeRoads.Add(road);
        }
        else
        {
            GameObject road = Instantiate(carPrefab, position, Quaternion.identity);
            _activeRoads.Add(road);
        }
    }

    public void DestoryRoad(GameObject road)
    {
        road.SetActive(false);
        _activeRoads.Remove(road);
        _roadPool.Enqueue(road);
    }
    
    #endregion
}
