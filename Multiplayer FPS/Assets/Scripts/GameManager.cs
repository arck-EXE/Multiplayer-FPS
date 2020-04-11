using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    //class to take respawntime value.
    public MatchSettings matchSettings;

    private void Awake()
    {
        //Check for the game manager.
        if(instance != null)
        {
            Debug.LogError("More than one GameManager in the scene");
        }
        else
        {
            //assigns this gameobject as gamemanager.
            instance = this;
        }
    }

    #region Player Tracking

    
    private const string PLAYER_ID_PREFIX = "Player ";


    //Creating Players Dictionary to store all the host and the client players
    private static Dictionary<string, Player> players = new Dictionary<string, Player>();


    //Registers all the players into the created dictionary as seperate gameobject
    public static void RegisterPlayer(string _netID, Player _player)
    {
        string _playerID = PLAYER_ID_PREFIX + _netID;
        players.Add(_playerID, _player);
        _player.transform.name = _playerID;
    }

    //Unregisters the player if died
    public static void UnRegisterPlayer(string _playerID)
    {
        players.Remove(_playerID);
    }


    //returns the playerId for furthur process.
    public static Player GetPlayer(string _playerID)
    {
        return players[_playerID];
    }

    /*
      only for display as GUI
    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(200, 200, 200, 500));
        GUILayout.BeginVertical();
        
        foreach(string _playerID in players.Keys)
        {
            GUILayout.Label(_playerID + " - " + players[_playerID].transform.name);
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();

    }
    */
    #endregion
}
