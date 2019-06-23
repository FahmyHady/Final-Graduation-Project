using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOutlineHandler : MonoBehaviour
{
    [SerializeField] PlayerStateInfo info;
    [SerializeField] Material outlinePlayerMat;
    [SerializeField] Material outlineObsMat;
    public void SetOutlineColor() {
        outlinePlayerMat.SetColor("_OutlineColor", info.Player.Outline);
        if (outlineObsMat != null || outlineObsMat != default(Material))
            outlineObsMat?.SetColor("_OutlineColor", info.Player.Outline);
    }
}
