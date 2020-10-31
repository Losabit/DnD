using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.Controllers
{
    public class FightController : MonoBehaviour
    {
        public int[][] map;
        public List<Personnage> personnages;
        private TimeKeeper controlTurn;
        private int turn = 0;

        void Start()
        {
            controlTurn = new TimeKeeper(10f, delegate { return ChangeTurn(); });
        }

        void Update()
        {
            if(personnages != null && map != null)
            {
                StartCoroutine(controlTurn.ReturnOrIncrease(Time.deltaTime));
                Debug.Log(personnages[turn].model.Name);
            }
        }

        private IEnumerator ChangeTurn()
        {
            if(++turn == personnages.Count)
            {
                turn = 0;
            }
            yield return null;
        }
    }
}