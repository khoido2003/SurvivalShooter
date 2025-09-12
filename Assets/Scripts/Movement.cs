using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        Vector3 move = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
            move += Vector3.forward;

        if (Input.GetKey(KeyCode.S))
            move += Vector3.back;

        if (Input.GetKey(KeyCode.A))
            move += Vector3.left;

        if (Input.GetKey(KeyCode.D))
            move += Vector3.right;

        // normalize so diagonal movement isn't faster
        move = move.normalized;

        // move the object
        transform.Translate(move * speed * Time.deltaTime, Space.World);
    }
}
