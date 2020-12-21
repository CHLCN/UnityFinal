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

    public float jumpForce;
    public float Gravity = -20;
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


        if (controller.isGrounded)
        {
            direction.y = -1;
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Jump();
            }
        }
        else
        {
            direction.y += Gravity * Time.deltaTime;
        }

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


        // 碰撞后停止向前移动
        //transform.position = Vector3.Lerp(transform.position, targetPosition, 80 * Time.deltaTime);
        //controller.center = controller.center;
        if (transform.position == targetPosition)
            return;
        Vector3 diff = targetPosition - transform.position;
        Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;
        if (moveDir.sqrMagnitude < diff.sqrMagnitude)
            controller.Move(moveDir);
        else
            controller.Move(diff);

    }

    // 不受帧数影响，处理物理逻辑
    private void FixedUpdate()
    {
        controller.Move(direction * Time.fixedDeltaTime);
    }

    private void Jump()
    {
        direction.y = jumpForce;
    }

    // 碰撞检测
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Obstacle")
        {
            PlayerManager.gameOver = true;
        }
    }
}
