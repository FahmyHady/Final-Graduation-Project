using GamepadInput;
using UnityEngine;

public class hplayerMove : MonoBehaviour
{
    #region Fields
    [SerializeField] public float speed = 5;
    private Vector3 direction;
    private GamepadState gamepad;
    private int id;
    private Vector2 move;
    private Vector3 movement;
    [SerializeField] private Rigidbody mybody = null;
    [SerializeField] private Transform rotatableTransform;
    #endregion Fields

    #region Methods
    private void FixedUpdate()
    {
        gamepad = GamePad.GetState(((GamePad.Index)id));
        move = gamepad.LeftStickAxis;
        if (move != Vector2.zero)
        {
            movement = new Vector3(move.x, 0.0f, move.y);
            movement = transform.TransformDirection(movement);
            mybody.AddForce(movement * speed);
            //mybody.AddForceAtPosition(movement.normalized, (movement.normalized + this.transform.position));
            //mybody.MovePosition(transform.position + movement.normalized * speed * Time.deltaTime);
            rotatableTransform.rotation = Quaternion.Slerp(rotatableTransform.rotation, Quaternion.LookRotation(movement), 0.15f);
        }
    }

    private void Start()
    { id = this.GetComponent<PlayerStateInfo>().PlayerController; }
    #endregion Methods
}