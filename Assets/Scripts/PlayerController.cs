using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Ctrl K+Ctrl D
public class PlayerController : MonoBehaviour
{
    private CharacterController controller; // 控制player
    private Vector3 direction; // 控制方向
    public float forwardSpeed; // 控制速度

    private int desiredLane = 1; // 0左 1中 2右
    public float laneDistance = 4; // 两条路之间的距离（左右移动的距离）

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //controller.Move(direction * Time.deltaTime); // 移动随帧数而变化
        direction.z = forwardSpeed;

        // 从输入判断应该在哪条路（左、中、右）
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            desiredLane++;
            if (desiredLane == 3)
                desiredLane = 2;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            desiredLane--;
            if (desiredLane == -1)
                desiredLane = 0;
        }

        // 判断未来趋势
        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;

        if (desiredLane == 0)
        {
            targetPosition += Vector3.left * laneDistance;
        }
        else if (desiredLane == 2)
        {
            targetPosition += Vector3.right * laneDistance;
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, 80 * Time.deltaTime);

    }

    // 不受帧数影响，处理物理逻辑
    private void FixedUpdate()
    {
        controller.Move(direction * Time.fixedDeltaTime);
    }
}
