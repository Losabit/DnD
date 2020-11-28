using UnityEngine;

class MapExplorer : MonoBehaviour
{
    private int x = 0;
    private int z = 0;
    private bool rightClickPressed = false;
    private void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            transform.position = new Vector3(transform.position.x + x, transform.position.y + 1, transform.position.z + z); 
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            transform.position = new Vector3(transform.position.x + x, transform.position.y - 1, transform.position.z + z);
        }

        if (Input.GetMouseButtonDown(1))
        {
            rightClickPressed = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            rightClickPressed = false;
        }
        if (rightClickPressed)
        {
            transform.Rotate(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            z = -1;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            z = 1;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            x = 1;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            x = -1;
        }

        if(x != 0 || z != 0)
        {
            transform.position = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);
        }

        if (Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.S))
        {
            z = 0;
        }
        else if (Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.D))
        {
            x = 0;
        }
    }
}
