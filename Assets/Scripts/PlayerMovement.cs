using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator playerAnimator;

    public float MoveSpeedX;
    public float MoveSpeedZ;

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveDepth = Input.GetAxis("Vertical");
        playerAnimator.SetBool("IsWalking", false);

        if (!DialogueManager.GetInstance().GetDialogueState())
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector3.forward * MoveSpeedZ * Time.deltaTime);
                playerAnimator.SetBool("IsWalking", true);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(-Vector3.forward * MoveSpeedZ * Time.deltaTime);
                playerAnimator.SetBool("IsWalking", true);
            }

            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector3.left * MoveSpeedX * Time.deltaTime);
                playerAnimator.SetBool("IsWalking", true);
                gameObject.transform.localScale = new Vector3(1, 1, 1);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(-Vector3.left * MoveSpeedX * Time.deltaTime);
                playerAnimator.SetBool("IsWalking", true);
                gameObject.transform.localScale = new Vector3(-1, 1, 1);

            }
            // transform.Translate(new Vector3(moveHorizontal * MoveSpeedX, 0f, moveHorizontal * MoveSpeedZ));
        }
    }
}
