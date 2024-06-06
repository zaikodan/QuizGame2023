using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveSystem : MonoBehaviour
{
    int playersAmount;
    [SerializeField] Dropdown playerSelector;
    [SerializeField] Text scoreboardText;

    List<string> players = new List<string>();

    const string playerPrefName = "Player", playerPrefID = "PlayerID", playerPrefScore = "PlayerScore";

    private void Start()
    {
        playersAmount = PlayerPrefs.GetInt("PlayersAmount");
        LoadPlayerList();
    }

    #region Loads

    private void UpdateDropdown()
    {
        playerSelector.ClearOptions();
        playerSelector.AddOptions(players);
    }

    private void LoadPlayerList()
    {
        players.Add("");

        for (int i = 0; i < playersAmount; i++)
        {
            players.Add(PlayerPrefs.GetString(playerPrefName + i));
        }

        UpdateDropdown();
    }

    public void GetScore(int value)
    {
        string playerName = playerSelector.options[value].text;
        int score = PlayerPrefs.GetInt(playerPrefScore + playerName);
        scoreboardText.text = "Pontuação recorde: " + score;
    }

    #endregion

    //nome = playerPrefName + ID
    //id = playerPrefID + playerName
    //score = playerPrefScore + playerName

    //nome = Player0
    //id = PlayerIDPedro
    //score = PlayerScorePedro

    bool CheckPlayerName(string playerName)
    {
        return PlayerPrefs.HasKey(playerPrefID + playerName);
    }

    #region Saves
    public void SavePlayer(string playerName, int playerScore)
    {
        if (CheckPlayerName(playerName))
        {
            OverridePlayerSave(playerName, playerScore);
        }
        else
        {
            SaveNewPlayer(playerName, playerScore);
        }
    }

    private void SaveNewPlayer(string playerName, int playerScore)
    {
        PlayerPrefs.SetString(playerPrefName + playersAmount, playerName);
        PlayerPrefs.SetInt(playerPrefID + playerName, playersAmount);
        PlayerPrefs.SetInt(playerPrefScore + playerName, playerScore);

        playersAmount++;
        PlayerPrefs.SetInt("PlayersAmount", playersAmount);

        players.Add(playerName);
        UpdateDropdown();
    }

    private void OverridePlayerSave(string playerName, int playerScore)
    {
        PlayerPrefs.SetInt(playerPrefScore + playerName, playerScore);
    }

    #endregion
}
