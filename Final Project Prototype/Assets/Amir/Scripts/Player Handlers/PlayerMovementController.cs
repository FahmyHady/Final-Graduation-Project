using GamepadInput;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    #region Fields
    private Vector3 direction;
    private GamepadState gamepad;
    private PlayerStateInfo info;
    private Vector2 move;
    private Vector3 movement;
    private Rigidbody mybody;
    private BaseCharacter myChar;
    Camera currentCam;
    Quaternion rotationRelativeToCam;
    #endregion Fields

    #region Methods

    private void Start()
    {
        info = this.GetComponent<PlayerStateInfo>();
        myChar = this.GetComponent<BaseCharacter>();
        currentCam = Camera.main;
    }

    private void FixedUpdate()
    {
        gamepad = info.Controller;
        move.x = gamepad.LeftStickAxis.x;
        move.y = gamepad.LeftStickAxis.y;

        if (move != Vector2.zero)
        {
            movement.x = move.x;
            movement.y = 0.0f;
            movement.z = move.y;
            movement *= myChar.speed * Time.deltaTime;
            movement = Quaternion.Euler(0, currentCam.transform.eulerAngles.y, 0) * movement;
            myChar.rb.AddForce(movement * myChar.speed * Time.deltaTime);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(movement), 0.15f);
        }
        else if (myChar.rb.velocity.magnitude != 0) { myChar.rb.velocity = Vector3.zero; }

        if (myChar.rb.velocity.magnitude > myChar.MaxSpeed)
        {
            myChar.rb.velocity = myChar.rb.velocity.normalized * myChar.MaxSpeed;
        }
    }

    #endregion Methods
}