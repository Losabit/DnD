using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SortsPrinter : MonoBehaviour
{
    public GameObject buttonSort;
    public Personnage personnage;
    public Vector3 personnagePosition;
    public int[][] map;
    public int sortChoosen = -1;
    public Dictionary<int, List<Transform>> sortsLaunched = new Dictionary<int, List<Transform>>();
    public Vector3 sortsLaunchedAt;

    private List<SortPrevisualisation> sortPrevisualisations = new List<SortPrevisualisation>();
    private bool instancied = false;
    private Vector3 oldPersonnagePosition;

    void Start()
    {

    }

    void Update()
    {
        if (personnage != null && !instancied)
        {
            Initialize();
            instancied = true;
        }

        if (instancied)
        {
            if (oldPersonnagePosition != personnagePosition)
            {
                Refresh();
                oldPersonnagePosition = personnagePosition;
            }

            if (sortChoosen == -1)
            {
                for (int i = 0; i < sortPrevisualisations.Count; i++)
                {
                    if (sortPrevisualisations[i].readyToLaunchSort == true && sortChoosen == -1)
                    {
                        sortChoosen = i;
                        i = -1;
                        continue;
                    }

                    if (i != sortChoosen && sortChoosen != -1)
                    {
                        sortPrevisualisations[i].canPrevisualize = false;
                    }
                }
            }
            else if(sortPrevisualisations[sortChoosen].sortLaunchedAt != null)
            {
                sortsLaunched.Add(sortChoosen, sortPrevisualisations[sortChoosen].hitedCases);
                sortsLaunchedAt = (Vector3)sortPrevisualisations[sortChoosen].sortLaunchedAt;
                sortPrevisualisations[sortChoosen].sortLaunchedAt = null;
            }
            else if (sortPrevisualisations[sortChoosen].readyToLaunchSort == false)
            {
                for (int i = 0; i < sortPrevisualisations.Count; i++)
                {
                    sortPrevisualisations[i].canPrevisualize = true;
                }
                sortChoosen = -1;
            }
        }
    }

    private void Initialize()
    {
        foreach (var sort in personnage.model.Sorts)
        {
            Sprite sprite = Resources.Load<Sprite>(sort.Icone);
            Transform button = Instantiate(buttonSort.transform);

            SortPrevisualisation sortPrevisualisation = button.gameObject.AddComponent<SortPrevisualisation>();
            sortPrevisualisation.personnage = personnage;
            sortPrevisualisation.personnagePosition = personnagePosition;
            sortPrevisualisation.sort = sort;
            sortPrevisualisations.Add(sortPrevisualisation);

            button.GetChild(0).GetComponent<Image>().sprite = sprite;
            button.SetParent(transform, false);
        }
    }

    private void Refresh()
    {
        foreach (SortPrevisualisation sortPrevisualisation in sortPrevisualisations)
        {
            sortPrevisualisation.personnagePosition = personnagePosition;
        }
    }
}
