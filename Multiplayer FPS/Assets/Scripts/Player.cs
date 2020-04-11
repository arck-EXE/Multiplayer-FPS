using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    //variable that gets synced to the server [SyncVar]
    [SyncVar]
    private bool _isDead = false;

    //using getter and setter to easily swap the bool values in the variable
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

    [SerializeField]
    private int maxHealth = 100;

    //current health gets synced with the server and the clients
    [SyncVar]
    private int currentHealth;

    //variables to disable the components from the behaviour
    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;

    //called in the player setup script and distinguishes the host player from the clients
    public void Setup()
    {

        wasEnabled = new bool[disableOnDeath.Length];

        for (int i = 0; i < wasEnabled.Length; i++)
        {
            wasEnabled[i] = disableOnDeath[i].enabled;
        }

        SetDefaults();
    }


    //called on the server then updates the clients
    [ClientRpc]
    public void RpcTakeDamage(int _amount)
    {

        if (isDead)
            return;

        currentHealth -= _amount;
        Debug.Log(transform.name + " now has " + currentHealth + "health");

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    //handles dying and respawning also disables the components and collider that can impact the game when a player dies
    private void Die()
    {
        isDead = true;

        //Disable Components
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }

        Collider _col = GetComponent<Collider>();
        if (_col != null)
            _col.enabled = false;

        Debug.Log(transform.name + " is Dead!");

        //Call Respawn Method 
        StartCoroutine(Respawn());
    }


    //coroutine to respawn players
    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);
        Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;

        Debug.Log(transform.name + "respawned");
    }

    //sets defaults like the enabled components and colliders...
    public void SetDefaults()
    {
        isDead = false;

        currentHealth = maxHealth;

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];

            Collider _col = GetComponent<Collider>();
            if(_col != null)
            {
                _col.enabled = true;
            }
        }
    }
}
