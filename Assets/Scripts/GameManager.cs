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
    
    // 만들어지는 도로 인덱스
    private int _roadIndex = 0;
    
    //상태
    public enum State {Start, Play, End}
    public State GameState { get; private set; }
    
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
    
    private void Start()
    {
        InnitializeRoadPool();
        
        GameState = State.Start;
        
        StartGame();
    }
    
    private void Update()
    {
        switch (GameState)
        {
            case State.Start:
                break;
            case State.Play:
                foreach (var activeRoad in _activeRoads)
                {
                    activeRoad.transform.Translate(Vector3.back * Time.deltaTime);
                }
        
                //Gas 정보 출력
                if (_carController != null)
                    gasText.text = _carController.Gas.ToString();
                break;
            case State.End:
                break;
        }
        
    }

    public void EndGame()
    {
        //게임 상태 변경
        GameState = State.End;
        
        // 자동차 제거
        Destroy(_carController.gameObject);
        
        //도로 제거
        foreach (var road in _activeRoads)
        {
            road.SetActive(false);
        }
        
        //TODO:게임 오버 패널 표시
        
    }
    
    private void StartGame()
    {
        //도로 생성
        SpawnRoad(Vector3.zero);
        
        //자동차 생성
        _carController = Instantiate(carPrefab, new Vector3(0, 0, -3f), Quaternion.identity)
            .GetComponent<CarController>();
        
        GameState = State.Play;
        
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
        GameObject road;
        if ((_roadPool.Count > 0))
        {
            road = _roadPool.Dequeue();
            road.SetActive(true);
            road.transform.position = position;
            
            
        }
        else
        {
            road = Instantiate(carPrefab, position, Quaternion.identity);
        }

        if (_roadIndex > 0 && _roadIndex % 2 == 0)
        {
            road.GetComponent<RoadController>().SpawnGas();
        }
        
        _activeRoads.Add(road);
        _roadIndex++;

    }

    public void DestoryRoad(GameObject road)
    {
        road.SetActive(false);
        _activeRoads.Remove(road);
        _roadPool.Enqueue(road);
    }
    
    #endregion
}
