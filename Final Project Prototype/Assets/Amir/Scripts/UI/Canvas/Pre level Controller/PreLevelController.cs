using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreLevelController : MonoBehaviour
{
    [SerializeField] GameObject roundpanal;
    [SerializeField] Sprite sprite;
    [SerializeField] Vector3 posImg;
    [SerializeField] Vector3 scImg;
    // Start is called before the first frame update
    void Start()
    {
        int[] rounds = GameManager.Instance.Rounds;
        for (int i = 0; i < rounds.Length; i++)
        {
            var m_icon = new GameObject().AddComponent<RectTransform>();
            m_icon.transform.SetParent(roundpanal.transform);
            m_icon.transform.localPosition = posImg;
            m_icon.transform.localScale = scImg;
            var m_iconImage = m_icon.gameObject.AddComponent<Image>();
            m_iconImage.sprite = sprite;
            switch (rounds[i])
            {
                case 0: m_iconImage.color = Color.white; break;
                case 1: m_iconImage.color = new Color(0.8679245f, 0.5722274f, 0.0286579f); break;
                case 2: m_iconImage.color = Color.red; break;
                default:
                    break;
            }
            posImg.x += 64;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
