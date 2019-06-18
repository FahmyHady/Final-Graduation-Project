using UnityEngine;

public class InfectionTrigger : MonoBehaviour
{
    #region Fields
    private hplayerMove PlayerMove = null;
    #endregion Fields

    #region Methods
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Child" && gameObject.tag == "Infected")
        {
            Debug.Log("Collision");
            collision.collider.GetComponentInChildren<Renderer>().material.color = new Color(0, 255, 0);
            gameObject.GetComponentInChildren<Renderer>().material.color = new Color(255, 255, 255);
            collision.collider.tag = "Infected";
            PlayerMove = collision.collider.GetComponent<hplayerMove>();
            PlayerMove.speed += 70;
            gameObject.tag = "Child";
            PlayerMove = gameObject.GetComponent<hplayerMove>();
            PlayerMove.speed -= 70;
            collision.collider.gameObject.GetComponent<InfectionTrigger>().enabled = true;
            gameObject.GetComponent<InfectionTrigger>().enabled = false;
        }
    }
    #endregion Methods
}