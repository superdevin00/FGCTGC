using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialPage : MonoBehaviour
{
    public GameObject[] pages;
    private int currentPage;
    public GameObject leftArrow;
    public GameObject rightArrow;
    
    // Start is called before the first frame update
    void Start()
    {
        currentPage = 0;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePageArrows();
        UpdatePage();
    }
    
    private void UpdatePage()
    {
        for (int i = 0; i < pages.Length; i++)
        {
            if (currentPage == i)
            {
                pages[i].SetActive(true);
            }
            else
            {
                pages[i].SetActive(false);
            }
        }
    }

    private void UpdatePageArrows()
    {
        if (currentPage <= 0)
        {
            leftArrow.SetActive(false);
        }
        else
        {
            leftArrow.SetActive(true);
        }

        if (currentPage >= pages.Length - 1)
        {
            rightArrow.SetActive(false);
        }
        else
        {
            rightArrow.SetActive(true);
        }
    }

    public void NextPage()
    {
        currentPage++;
    }
    public void PrevPage()
    {
        currentPage--;
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
