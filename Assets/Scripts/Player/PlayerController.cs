using System.Collections;
using UnityEngine;
// Ctrl K+Ctrl D
public class PlayerController : MonoBehaviour
{
    private CharacterController controller; // 控制player
    private Vector3 direction; // 控制方向
    public float forwardSpeed; // 控制速度
    public float maxSpeed;

    private int desiredLane = 1; // 0左 1中 2右
    public float laneDistance = 4; // 两条路之间的距离（左右移动的距离）

    public float jumpForce;
    public float Gravity = -20;

    public Animator animator;
    private bool isSliding = false;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //controller.Move(direction * Time.deltaTime); // 移动随帧数而变化
        if (!PlayerManager.isGameStarted)
            return;
        if (forwardSpeed < maxSpeed)
        {
            forwardSpeed += 0.1f * Time.deltaTime; // 随时间加速
        }

        animator.SetBool("isGameStarted", true);


        direction.z = forwardSpeed;


        if (controller.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || SwipeManager.swipeUp)
            {
                Jump();
                animator.SetBool("isGrounded", false);
            }
        }
        else
        {
            direction.y += Gravity * Time.deltaTime;
            animator.SetBool("isGrounded", true);
        }

        if ((Input.GetKeyDown(KeyCode.DownArrow) || SwipeManager.swipeDown) && !isSliding)
        {
            StartCoroutine(Slide());

        }

        // 从输入判断应该在哪条路（左、中、右）
        if (Input.GetKeyDown(KeyCode.RightArrow) || SwipeManager.swipeRight)
        {
            desiredLane++;
            if (desiredLane == 3)
                desiredLane = 2;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || SwipeManager.swipeLeft)
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
        if (transform.position != targetPosition)
        {

            Vector3 diff = targetPosition - transform.position;
            Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;
            if (moveDir.sqrMagnitude < diff.sqrMagnitude)
                controller.Move(moveDir);
            else
                controller.Move(diff);
        }



        //移动player
        controller.Move(direction * Time.deltaTime);

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
            FindObjectOfType<AudioManager>().PlaySound("GameOver");
        }
    }

    private IEnumerator Slide()
    {
        isSliding = true;
        animator.SetBool("isSliding", true);
        controller.center = new Vector3(0, -0.5f, 0);
        controller.height = 1;
        yield return new WaitForSeconds(1.3f);

        controller.center = new Vector3(0, 0, 0);
        controller.height = 2;
        animator.SetBool("isSliding", false);
        isSliding = false;
    }

}
