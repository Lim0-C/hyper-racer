using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class GameTestScript
{
    private CarController _carController;
    private GameObject _leftMoveButton;
    private GameObject _rightMoveButton;
    
    
    // A Test behaves as an ordinary method
    [Test]
    public void GameTestScriptSimplePasses()
    {
        // Use the Assert class to test conditions
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator GameTestScriptWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        //씬로드
        SceneManager.LoadScene("Scenes/Game2", LoadSceneMode.Single);
        yield return WaitForSceneLoad();
        
        //필수 오브젝트 확인
        var gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Assert.IsNotNull(gameManager, "GameManager Object is null");
        var startButton = GameObject.Find("Start Button");
        Assert.IsNotNull(startButton, "StartButton is null");
        
        //게임 실행
        startButton.GetComponent<Button>().onClick.Invoke();
        
        //플레이어 자동차 확인
        _carController = GameObject.Find("Car(Clone)").GetComponent<CarController>();
        Assert.IsNotNull(_carController, "Car Controller is null");
        
        //게임 제어 관련 버튼 확인
        _leftMoveButton = GameObject.Find("LeftButton");
        Assert.IsNotNull(_leftMoveButton, "LeftButton is null");
        _rightMoveButton = GameObject.Find("RightButton");
        Assert.IsNotNull(_rightMoveButton, "RightButton is null");
        
        //가스의 등장 위치 파악하기
        Vector3 leftPosition = new Vector3(-1.5f, 0.2f, -3f);
        Vector3 rightPosition = new Vector3(1.5f, 0.2f, -3f);
        Vector3 centerPosition = new Vector3(0f, 0.2f, -3f);

        float rayDistance = 10f;
        Vector3 rayDirection = Vector3.forward;
        
        //플레이 시간
        float elapsedTime = 0f;
        float targetTime = 10f;
        
        //반복
        while (gameManager.GameState == GameManager.State.Play)
        {
            RaycastHit hit;
            if (Physics.Raycast(leftPosition, rayDirection, out hit, rayDistance,
                    LayerMask.GetMask("Gas")))
            {
                Debug.Log("left");
                MoveCar(hit.point);
            }
            else if(Physics.Raycast(rightPosition, rayDirection, out hit, rayDistance,
                        LayerMask.GetMask("Gas")))
            {
                Debug.Log("right");
                MoveCar(hit.point);
            }
            else if (Physics.Raycast(centerPosition, rayDirection, out hit, rayDistance,
                          LayerMask.GetMask("Gas")))
            {
                Debug.Log("center");
                MoveCar(hit.point);
            }
            else
            {
                Debug.Log("none");
                MoveButtonUp(_leftMoveButton);
                MoveButtonUp(_rightMoveButton);
            }
            
            Debug.DrawRay(leftPosition, rayDirection * rayDistance, Color.red);
            Debug.DrawRay(rightPosition, rayDirection * rayDistance, Color.green);
            Debug.DrawRay(centerPosition, rayDirection * rayDistance, Color.blue);
            
            //시간 체크
            elapsedTime += Time.deltaTime;
            
            yield return null;
        }

        if (elapsedTime < targetTime)
        {
            Assert.Fail("Game Time is too short");
        }
        
    }

    private IEnumerator WaitForSceneLoad()
    {
        while (SceneManager.GetActiveScene().buildIndex >0)
        {
            yield return null;
        }
    }

    private void MoveButtonDown(GameObject moveButton)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(moveButton, pointerEventData, ExecuteEvents.pointerDownHandler);
    }
    private void MoveButtonUp(GameObject moveButton)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(moveButton, pointerEventData, ExecuteEvents.pointerUpHandler);
    }

    private void MoveCar(Vector3 targetposition)
    {
        if (Mathf.Abs(targetposition.x - _carController.transform.position.x) < 0.1f)
        {
            MoveButtonUp(_leftMoveButton);
            MoveButtonUp(_rightMoveButton);
            return;
        }
        
        if (targetposition.x < _carController.transform.position.x)
        {
            MoveButtonDown(_leftMoveButton);
            MoveButtonUp(_rightMoveButton);
        }
        else if (targetposition.x > _carController.transform.position.x)
        {
            MoveButtonDown(_rightMoveButton);
            MoveButtonUp(_leftMoveButton);
        }
        else
        {
            MoveButtonUp(_leftMoveButton);
            MoveButtonUp(_rightMoveButton);
        }
    }
}
