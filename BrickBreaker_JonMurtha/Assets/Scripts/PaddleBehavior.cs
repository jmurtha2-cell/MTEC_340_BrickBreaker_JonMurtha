using UnityEngine;

public class PaddleBehavior : MonoBehaviour
{
    public float Speed = 5.0f;
    public KeyCode leftDirection = KeyCode.LeftArrow;
    public KeyCode rightDirection = KeyCode.RightArrow;

    // Update is called once per frame
    void Update()
    {
        float movement = 0.0f;

        if (Input.GetKey(leftDirection)){
            movement -= Speed;
        }
        if (Input.GetKey(rightDirection)){
            movement += Speed;
        }

        movement *= Time.deltaTime;

        transform.Translate(movement, 0.0f, 0.0f);
    }
}
