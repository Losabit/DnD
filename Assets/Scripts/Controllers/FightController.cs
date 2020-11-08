using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Assets.Scripts.Controllers
{
    public class FightController : MonoBehaviour
    {
        public int[][] map;
        public List<Personnage> personnages;
        public GameObject view;
        public List<FightPlayController> fightPlayController;

        private Personnage playerPersonnage;
        private TurnView turnView;

        private TimeKeeper controlTurn;
        private int turn = 0;

        void Start()
        {
            turnView = view.GetComponentInChildren<TurnView>();
            controlTurn = new TimeKeeper(10f, delegate { return ChangeTurn(); });

            if (turnView != null && personnages != null)
            {
                List<string> personnagesName = new List<string>();
                for (int i = 0; i < personnages.Count; i++)
                {
                    personnagesName.Add(personnages[i].model.Name);
                    if (personnages[i].controllerValue == 1)
                    {
                        ((PlayerController)fightPlayController[i]).view = view;
                    }
                }
                turnView.personnagesName = personnagesName;
                fightPlayController[turn].canPlay = true;
                fightPlayController[turn].map = map;
            }
        }

        void Update()
        {
            if (personnages != null && map != null)
            {
                StartCoroutine(controlTurn.ReturnOrIncrease(Time.deltaTime));
            }
        }

        private IEnumerator ChangeTurn()
        {
            fightPlayController[turn].canPlay = false;
            personnages[turn].currentActionPoints = personnages[turn].model.ActionPoints;
            if (++turn == personnages.Count)
            {
                turn = 0;
            }
            fightPlayController[turn].canPlay = true;
            fightPlayController[turn].map = map;
            turnView.turnSender.MyValue = turn;
            yield return null;
        }
    }
}