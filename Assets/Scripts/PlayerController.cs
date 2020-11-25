using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller; // 控制player
    private Vector3 direction; // 控制方向
    public float forwardSpeed; // 控制速度

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
    }

    // 不受帧数影响，处理物理逻辑
    private void FixedUpdate()
    {
        controller.Move(direction * Time.fixedDeltaTime); 
    }
}
