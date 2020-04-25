using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class State : MonoBehaviour
{

    public List<Transition> transitions;
    public string selfname;

    [SerializeField]
    public bool gothit;
    [SerializeField]
    public bool invoked;

    public virtual void Awake()
    {
        transitions = new List<Transition>();
        //TODO
        // definir as transições aqui
    }

    public virtual void OnEnable()
    {
        //TODO
        // Inicializações do estado;
    }

    public virtual void OnDisable()
    {
        //TODO
        //Finalização do estado
    }

    public virtual void Update()
    {
        //TODO
        // Implementacáo do comportamento
    }

    public void LateUpdate()
    {
        // Para cada transição que esse estado tiver
        // é feita a verificação de sua condição
        foreach (Transition t in transitions)
        {
            if (t.condition.Test())
            {
                CancelInvoke();
                if (t.target.selfname.Equals("StateAtacando") || t.target.selfname.Equals("StateAngry"))
                {
                    t.target.invoked = false;
                }
                t.target.enabled = true;
                if (t.target.selfname.Equals("StatePerseguindo") && this.selfname.Equals("StatePatrulha"))
                {
                    t.target.Invoke("InstantiateExclamation", 0f);
                }

                this.enabled = false;

                return;
            }
        }
    }

}