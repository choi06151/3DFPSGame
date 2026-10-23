using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    [Header("카메라 회전속도")]
    [SerializeField] private float _rotationSpeed = 200f;

    private float _accumulationX = 0f;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GameState != GameState.Playing)
            return;
        
        Cursor.visible = false;
        //1. 마우스 입력을 받는다
        float mouseX = Input.GetAxis("Mouse X");

        //2. 마우스 입력에 따라 회전할 방향을 구한다
        _accumulationX += mouseX * _rotationSpeed * Time.deltaTime;

        //3. 해당 방향으로 회전 시킨다


        transform.eulerAngles = new Vector3(0, _accumulationX, 0);

    }

}
