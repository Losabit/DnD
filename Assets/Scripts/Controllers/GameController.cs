using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Controllers
{
    public class GameController : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

    public enum ControllerType
    {
        None = 0,
        Fight = 1,
        Editor = 2
    }
}