using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//Appeler dans FightController
// FightController hérité Controller 
class FightView : MonoBehaviour
{
    public List<string> personnagesName;
    private bool turnViewInstanced = false;

    private void Update()
    {
        if(personnagesName != null && !turnViewInstanced)
        {

            turnViewInstanced = true;
        }
    }
}

