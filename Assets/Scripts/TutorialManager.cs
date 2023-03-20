using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    
    public static TutorialManager Instance { get; private set; }

    [SerializeField] private Transform cameraFollow;


    [Serializable]
    public struct TutorialData
    {
        public List<GameObject> hiddenGameobjects;
        public string text;
    }


    [SerializeField] private List<TutorialData> tutorialDataList;
    [SerializeField] private List<GameObject> arrowList;

    private bool isTutorial;
    private int tutorialIndex;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("TutorialManager already exists!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        foreach(GameObject arr in arrowList)
        {
            arr.gameObject.SetActive(false);
        }

        isTutorial = SceneInfo.Instance.IsTutorial();

        if (!isTutorial) return;


        TutorialUI.Instance.OnNextTutorialStep += Instance_OnNextTutorialStep;

        RunTutorial();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)){
            if(tutorialIndex == 14)
            {
                if(PlayerManager.Instance.GetActualPlayer() == null)
                {
                    TutorialUI.Instance.ShowWithoutButton("Click on a player to see his inventory");
                } 
                else
                {
                    TutorialUI.Instance.ShowWithoutButton("All the items your character has in his inventory are listed here.");
                }
            }
        }
    }

    private void Instance_OnNextTutorialStep(object sender, EventArgs e)
    {
        RunTutorial();
    }

    private void RunTutorial()
    {
        tutorialIndex = SceneInfo.Instance.GetTutorialIndex();

        if (tutorialDataList.Count > tutorialIndex)
        {
            foreach (GameObject gameObject in tutorialDataList[tutorialIndex].hiddenGameobjects)
            {
                gameObject.SetActive(false);
            }

            TutorialUI.Instance.Show(tutorialDataList[tutorialIndex].text);
        }

        switch (tutorialIndex)
        {
            case 1:
                arrowList[0].gameObject.SetActive(true);
                arrowList[1].gameObject.SetActive(true);
                TutorialUI.Instance.HideClick();
                break;
            case 2:
                arrowList[0].gameObject.SetActive(false);
                arrowList[1].gameObject.SetActive(false);
                arrowList[2].gameObject.SetActive(true);
                break;
            case 3:
                arrowList[2].gameObject.SetActive(false);
                foreach (GameObject go in tutorialDataList[tutorialIndex - 1].hiddenGameobjects)
                {
                    if (go.TryGetComponent<ActionsUI>(out ActionsUI ui))
                    {
                        go.SetActive(true);
                    }
                }
                arrowList[3].gameObject.SetActive(true);

                PlayerManager.Instance.ShowActions();
                break;
            case 4:
                arrowList[3].gameObject.SetActive(false);
                foreach (GameObject go in tutorialDataList[tutorialIndex - 1].hiddenGameobjects)
                {
                    if (go.TryGetComponent<PlayerListUI>(out PlayerListUI ui))
                    {
                        go.SetActive(true);
                    }
                }
                arrowList[4].gameObject.SetActive(true);

                break;
            case 5:
                arrowList[4].gameObject.SetActive(false);
                break;
            case 6:
                arrowList[5].gameObject.SetActive(true);
                TutorialUI.Instance.HideClick();
                cameraFollow.position = new Vector3(14, 12);
                break;
            case 7:
                arrowList[5].gameObject.SetActive(false);
                arrowList[6].gameObject.SetActive(true);
                cameraFollow.transform.position = new Vector3(15, 15);
                TutorialUI.Instance.HideClick();
                break;
            case 8:
                TutorialUI.Instance.HideAll();
                break;
            case 9:
                Time.timeScale = 0f;

                List<string> battleOrder = new List<string>();

                bool isEnemy = false;
                int index = 0;
                foreach (Entity e in BattleManager.Instance.GetBattleOrderList())
                {
                    if(index == 0)
                    {
                        isEnemy = e.IsEnemy();
                    }
                    battleOrder.Add(e.ToString());
                    index++;
                }
                string ret = "Now we come to the battle. The order of battle is randomly generated. Because this is your first battle, the order of battle will be known. Battle order: ";
                for (int i = 0; i < battleOrder.Count; i++)
                {
                    if(i < battleOrder.Count - 1)
                        ret += battleOrder[i] + ", ";
                    else 
                        ret += battleOrder[i] + ". ";
                }
                if (isEnemy)
                    ret += "It means you will not start.";
                else
                    ret += "It means you will start.";

                TutorialUI.Instance.Show(ret);
                break;
            case 10:
                arrowList[0].gameObject.SetActive(true);
                foreach (GameObject go in tutorialDataList[tutorialIndex - 1].hiddenGameobjects)
                {
                    if (go.TryGetComponent<BattleInfoUI>(out BattleInfoUI ui))
                    {
                        go.SetActive(true);
                    }
                }

                break;
            case 11:
                arrowList[0].gameObject.SetActive(false);
                arrowList[1].gameObject.SetActive(true);
                tutorialDataList[tutorialIndex - 1].hiddenGameobjects[0].gameObject.SetActive(true);
                break;
            case 12:
                if (BattleManager.Instance.FirstInListIsEnemy())
                {
                    foreach (GameObject go in tutorialDataList[tutorialIndex - 3].hiddenGameobjects)
                    {
                    
                        go.SetActive(false);
                        
                    }
                }
                arrowList[1].gameObject.SetActive(false);
                Time.timeScale = 1f;
                TutorialUI.Instance.HideAll();
                break;
            case 13:
                Vector3 arrowPosition = GameObject.Find("ChestItem").transform.position;
                arrowPosition.x -= 1f;
                arrowList[2].transform.position = arrowPosition;
                arrowList[2].gameObject.SetActive(true);
                TutorialUI.Instance.HideClick();
                break;
            case 14:
                arrowList[2].gameObject.SetActive(false);
                arrowList[3].gameObject.SetActive(true);
                tutorialDataList[tutorialIndex - 1].hiddenGameobjects[0].gameObject.SetActive(true);
                TutorialUI.Instance.HideClick();
                break;
            case 15:
                arrowList[3].gameObject.SetActive(false);
                break;
            case 16:
                SceneInfo.Instance.SetIsTutorial(false);
                TutorialUI.Instance.HideAll();
                break;
        }
    }

}
