using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Personnage))]
public class MouseAction : MonoBehaviour
{
    public Material material;
    private Personnage player;
    void Start()
    {
        player = GetComponent<Personnage>();
    }

    Ray ray;
    RaycastHit hit;
    //void Update()
    //{
    //    if (player == null)
    //        return;

    //    ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    if (Physics.Raycast(ray, out hit))
    //    {
    //        if (material != null && hit.collider.tag == "MouseOver")
    //        {
    //            if ((int)CaseDistance(player.transform.localPosition, hit.collider.transform.position) <= player.MovementPoints)
    //            {
    //                hit.collider.GetComponent<MeshRenderer>().material = material;
    //                if (Input.GetMouseButtonDown(0))
    //                {
    //                    player.transform.localPosition = hit.collider.transform.position;
    //                }
    //            }
    //        }
    //    }
    //}

    private float CaseDistance(Vector3 start, Vector3 end)
    {
        return Mathf.Abs(start.x - end.x) +
            Mathf.Abs(start.y - end.y) +
            Mathf.Abs(start.z - end.z);
    }
}
