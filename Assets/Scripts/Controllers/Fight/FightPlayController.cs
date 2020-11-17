using System;
using UnityEngine;

public class FightPlayController : MonoBehaviour
{
    public int[][] map = null;
    public Personnage personnage;
    public bool canPlay = false;

    protected Vector2 position;

    private Animation animator;

    protected void Initialize()
    {
        //Remove animator ?
        animator = gameObject.AddComponent<Animation>();

        AnimationClip mdl = Resources.Load<AnimationClip>("Personnages/Animations/Orc Walk");
        GameObject mesh = Resources.Load<GameObject>("Personnages/Animations/Orc Walk");
        Animation anim = mesh.GetComponent<Animation>();
       
        if (!mdl)
        {
            Debug.LogError("animation could not be found.");
        }
        else
        {
            animator.AddClip(mdl, "walk");
            animator.Play("walk");
        }
        
    }

    protected void PlayAnimations()
    {
        if (!animator.isPlaying)
        {
           // Debug.Log("not playing");
            animator.Play("walk");
        }
        else
        {
         //   Debug.Log("playing");
        }
    }

    protected void Move(int x, int z)
    {
        if (position == null)
            throw new Exception("Position not defined");


        transform.position = new Vector3(x, 1, z);
        map[(int)position.y][(int)position.x] = 0;
        map[z][x] = personnage.controllerValue;
        personnage.currentActionPoints -= GetDistance(new Vector2(x, z), position);
        position.x = x;
        position.y = z;
    }

    protected float GetDistance(Vector3 start, Vector3 end)
    {
        return Mathf.Abs(start.x - end.x) +
            Mathf.Abs(start.y - end.y) +
            Mathf.Abs(start.z - end.z);
    }

    protected int GetDistance(Vector2 vector, Vector2 vector2)
    {
        return (int)(Mathf.Abs(vector.x - vector2.x) +
            Mathf.Abs(vector.y - vector2.y));
    }
}

