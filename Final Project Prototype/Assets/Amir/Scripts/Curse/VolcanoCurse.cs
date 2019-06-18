using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcanoCurse : MonoBehaviour, ICursed
{
    [SerializeField] GameObject meteor;
    [SerializeField] float radius;
    [SerializeField] int weight;
    [Tooltip("if true, only one at time that can be active")] [ReadOnlyWhenPlaying]
    [SerializeField] bool isOnceOnly;
    [SerializeField] GameEvent @event;
    bool isStartCurse;
    public int Weight { get => weight; set => weight = value; }
    public bool IsOnceOnly { get => isOnceOnly; set => isOnceOnly = value; }
    public bool IsStartCurse { get => isStartCurse; set => isStartCurse = value; }

    public void StartCurse()
    {
        GameObject controller = Instantiate(meteor, transform.position, transform.rotation);
        controller.GetComponent<MeteorMovmentController>().Fire(RoundManager.Instance.GetRandomChild().transform.position+GetOffset());
    }
    Vector3 GetOffset() {
        return new Vector3((Random.value-0.5f) * radius, 0, (Random.value - 0.5f) * radius);
    }
    public void EndCurse() {
        isStartCurse = false;
        @event.Raise();
    }
}
