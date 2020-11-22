using UnityEngine;
using UnityEngine.UI;

class MapEditor : MonoBehaviour
{
    public GameObject componentView;
    public GameObject map;

    public void TabOnClick(GameObject toActivate)
    {
        ActivateOnlyChoosenTab(componentView, toActivate);
    } 

    private void ActivateOnlyChoosenTab(GameObject parent, GameObject toActivate)
    {
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            Transform child = parent.transform.GetChild(i);
            if (child.name == toActivate.transform.name)
            {
                toActivate.SetActive(true);
            }
            else if (child.gameObject.activeSelf)
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    public void CreateMapOnClick()
    {
        MapModding.SaveMap(map, Random.Range(1, 1000).ToString());
    }

    private int limit = 100;
    public void ChangeXDimension(Text newValue)
    {
        int value = int.Parse(newValue.text);
        if (value <= limit)
            map.GetComponent<MapController>().xSize = value;
    }

    public void ChangeZDimension(Text newValue)
    {
        int value = int.Parse(newValue.text);
        if (value <= limit)
            map.GetComponent<MapController>().zSize = value;
    }
}

