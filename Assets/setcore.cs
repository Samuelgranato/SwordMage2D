using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class setcore : MonoBehaviour
{
    private TMP_Text m_TextComponent;

    // Start is called before the first frame update
    void Start()
    {
        m_TextComponent = GetComponent<TMP_Text>();
        m_TextComponent.SetText(GameState.score.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
