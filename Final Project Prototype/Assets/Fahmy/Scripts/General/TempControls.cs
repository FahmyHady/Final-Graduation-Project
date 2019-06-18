using UnityEngine;

[RequireComponent(typeof(BaseCharacter))]
public class TempControls : MonoBehaviour
{
    #region Fields
    private Vector3 forward, right; // Keeps track of our relative forward and right vectors
    private BaseCharacter mychar;
    private Throw myThrow;
    #endregion Fields

    #region Methods
    private void FixedUpdate()
    {
        if (!mychar.isStunned)
        {
            if (mychar.canMove)
            {
                if (Input.anyKey) Move(); // only execute if a key is being pressed
                if (Input.GetKeyDown(KeyCode.Mouse0)) { myThrow.ThrowSomething(mychar.DetectSurroundingPlayers(4)[0].transform, Cloud.myLocation); }
                if (Input.GetKeyDown(KeyCode.Mouse1)) { mychar.SkillTwo(); }
            }
        }
    }

    private void Move()
    {
        mychar.rb.AddForce(Input.GetAxis("Horizontal") * mychar.speed, 0, Input.GetAxis("Vertical") * mychar.speed); // setup a direction Vector based on keyboard input. GetAxis returns a value between -1.0 and 1.0. If the A key is pressed, GetAxis(HorizontalKey) will return -1.0. If D is pressed, it will return 1.0
    }

    private void Start()
    {
        mychar = GetComponent<BaseCharacter>();
        myThrow = GetComponent<Throw>();
        forward = Camera.main.transform.forward; // Set forward to equal the camera's forward vector
        forward.y = 0; // make sure y is 0
        forward = Vector3.Normalize(forward); // make sure the length of vector is set to a max of 1.0
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward; // set the right-facing vector to be facing right relative to the camera's forward vector
    }
    #endregion Methods
}