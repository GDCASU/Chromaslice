using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetManager : NetworkManager
{

    public List<Player> playerList;

    public List<Player> localPlayers;

    public void Start()
    {
        playerList = new List<Player>();
        localPlayers = new List<Player>();
    }

    public void StartLocalGame()
    {
        maxConnections = 0;
        for (int i = 0; i < 4; i++)
            localPlayers.Add(new Player() { name = "Player " + i, controllerId = i });
        StartHost();
    }

    void OnReceivePlayerInfo(NetworkMessage netMsg)
    {
        Debug.Log("Receive");
        PlayerInfoMessage msg = netMsg.ReadMessage<PlayerInfoMessage>();
        playerList.AddRange(msg.players);
    }

    public override void OnStartServer()
    {
        NetworkServer.RegisterHandler(ExtMsgType.PlayerInfo, OnReceivePlayerInfo);
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        OnServerAddPlayer(conn, playerControllerId, null);
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
    {
        NetworkServer.AddPlayerForConnection(conn, GameManager.singleton.SpawnPlayer(), playerControllerId);
    }

    public override void OnStopServer()
    {
        localPlayers = new List<Player>();
        playerList = new List<Player>();
    }

    public static NetManager GetInstance()
    {
        return singleton as NetManager;
    }

    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        if (!ClientScene.ready) ClientScene.Ready(conn);
        for (short i = 0; i < localPlayers.Count; i++)
        {
            if (ClientScene.localPlayers.Count >= i+1 && ClientScene.localPlayers[i].IsValid && ClientScene.localPlayers[i].gameObject)
                Destroy(ClientScene.localPlayers[i].gameObject);
            ClientScene.AddPlayer(conn, i);
        }
    }

    public override void OnServerReady(NetworkConnection conn)
    {
        NetworkServer.SendToClient(conn.connectionId, ExtMsgType.Ping, new PingMessage() { connectionId = conn.connectionId });
        base.OnServerReady(conn);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        client.RegisterHandler(ExtMsgType.Ping, OnPing);
        base.OnClientConnect(conn);
    }

    public void OnPing(NetworkMessage netMsg)
    {
        PingMessage msg = netMsg.ReadMessage<PingMessage>();
        foreach (Player p in localPlayers)
            p.connectionId = msg.connectionId;
        client.Send(ExtMsgType.PlayerInfo, new PlayerInfoMessage(localPlayers));
    }

    class PlayerInfoMessage : MessageBase
    {
        public List<Player> players;

        public PlayerInfoMessage() { }

        public PlayerInfoMessage(List<Player> players)
        {
            this.players = players;
        }

        public override void Serialize(NetworkWriter writer)
        {
            writer.Write(players.Count);
            foreach (Player p in players)
            {
                writer.Write(p.name);
                writer.Write(p.connectionId);
                writer.Write(p.controllerId);
            }
        }

        public override void Deserialize(NetworkReader reader)
        {
            players = new List<Player>();
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                players.Add(new Player() { name = reader.ReadString(), connectionId = reader.ReadInt32(), controllerId = reader.ReadInt32() });
            }
        }
    }

    class PingMessage : MessageBase
    {
        public int connectionId;

        public override void Serialize(NetworkWriter writer)
        {
            writer.Write(connectionId);
        }

        public override void Deserialize(NetworkReader reader)
        {
            connectionId = reader.ReadInt32();
        }
    }

    public class ExtMsgType
    {
        public static short PlayerInfo = MsgType.Highest + 1;
        public static short Ping = MsgType.Highest + 2;
    }
}
