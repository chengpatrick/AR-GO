using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;

using Debug = UnityEngine.Debug;

public class PlayerManager : MonoBehaviourPun
{
    public enum EventCodes : byte
    {
        PlayerAdded,
    }

    public static PlayerManager instance;

    //Show in inspector
    public List<PlayerSingle> players = new List<PlayerSingle>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            return;
        }
        else
        {
            NewPlayerSend(PhotonNetwork.LocalPlayer.ActorNumber);
            Debug.Log("Player ID: " + PhotonNetwork.LocalPlayer.ActorNumber);
        }
    }

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    public void AddPlayer(PlayerSingle player)
    {
        players.Add(player);
        Debug.Log("Player Added");
    }

    public void NewPlayerSend(int playerId)
    {
        object[] data = new object[] { playerId };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent((byte)EventCodes.PlayerAdded, data, raiseEventOptions, SendOptions.SendReliable);
    }

    public void OnEvent(EventData photonEvent)
    {
        Debug.Log("Event Received");
        if (photonEvent.Code == (byte)EventCodes.PlayerAdded)
        {
            object[] data = (object[])photonEvent.CustomData;
            int playerId = (int)data[0];

            NewPlayerReceive(playerId);
            Debug.Log("Received Player ID: " + playerId);
        }
    }

    public void NewPlayerReceive(int playerId)
    {
        PlayerSingle player = new PlayerSingle(playerId);
        AddPlayer(player);

        for (int i = 0; i < players.Count; i++)
        {
            Debug.Log("Player ID in list: " + players[i].playerId);
        }
    }
}
