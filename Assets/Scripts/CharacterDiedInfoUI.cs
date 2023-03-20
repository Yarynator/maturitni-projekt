using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDiedInfoUI : MonoBehaviour
{
    
    [SerializeField] private Button backgroundClick;
    [SerializeField] private GameObject background;
    [SerializeField] private Button okButton;

    private void Awake(){

        backgroundClick.onClick.AddListener(() => {
            Hide();
        });
        okButton.onClick.AddListener(() => {
            Hide();
        });

        Player.OnAnyPlayerDies += Player_OnAnyPlayerDies;

        Hide();
    }

    private void Show(){
        backgroundClick.gameObject.SetActive(true);
        background.SetActive(true);
        okButton.gameObject.SetActive(true);
    }

    private void Hide(){
        backgroundClick.gameObject.SetActive(false);
        background.SetActive(false);
        okButton.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    private void Player_OnAnyPlayerDies(object sender, System.EventArgs e) {
        Debug.Log("hehe");
        if(PlayerManager.Instance.GetPlayerList().Count > 0) {
            Show();
            Time.timeScale = 0f;
        }
    }

}
