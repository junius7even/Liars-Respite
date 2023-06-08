using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IDataPersistance
{
    public static bool IsInDialogue;

    public float MoveSpeedX = 5;
    public float MoveSpeedZ = 2;
    

    public void LoadData(GameData data)
    {
        this.transform.position = data.playerPosition;

    }

    public void SaveData(ref GameData data)
    {
        data.playerPosition = this.transform.position;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveDepth = Input.GetAxis("Vertical");
        
        if (!IsInDialogue)
        {
            if(Input.GetKey(KeyCode.W))
                transform.Translate(Vector3.forward * MoveSpeedX * Time.deltaTime);
        
            else if(Input.GetKey(KeyCode.S))
                transform.Translate(-Vector3.forward * MoveSpeedX * Time.deltaTime);
            if(Input.GetKey(KeyCode.A))
                transform.Translate(Vector3.left * MoveSpeedZ * Time.deltaTime);
            else if(Input.GetKey(KeyCode.D))
                transform.Translate(-Vector3.left * MoveSpeedZ * Time.deltaTime);
            // transform.Translate(new Vector3(moveHorizontal * MoveSpeedX, 0f, moveHorizontal * MoveSpeedZ));

        }
    }
}
