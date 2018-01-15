using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// Developer:   Kyle Aycock
// Date:        11/17/17
// Description: This class is responsible for managing the
//              networking components of the game. This and the
//              GameManager work very closely together and
//              communicate often. Currently supports these situations:
//                  4 local players
//                  2 online players (each with 2 local players)
//                  3 online players (one with 2 local, two with 1)
//                  4 online players (all with 1 local)

public class NetManager : NetworkManager
{
    public List<Player> playerList;

    public List<Player> localPlayers;


    public string lobbyScene;

    public void Start()
    {
        playerList = new List<Player>();
        localPlayers = new List<Player>();
    }

    public void StartLocalGame()
    {
        maxConnections = 0;
        for (short i = 0; i < 4; i++)
            localPlayers.Add(new Player() { name = "Player " + i, controllerId = i });
        StartHost();
    }

    void OnReceivePlayerInfo(NetworkMessage netMsg)
    {
        PlayerInfoMessage msg = netMsg.ReadMessage<PlayerInfoMessage>();

        if ((IsGameJoinable(msg.players.Count) || msg.players.Count == 4) && GetPlayer(msg.players[0].connectionId, msg.players[0].controllerId) == null)
        {
            playerList.AddRange(msg.players);
            for (int i = 0; i < playerList.Count; i++)
                playerList[i].playerId = i;
            NetworkServer.SendToAll(ExtMsgType.PlayerInfo, new PlayerInfoMessage(playerList));
        }
        else if(networkSceneName == lobbyScene)
        {
            Debug.Log("Disconnecting player");
            GetConnection(msg.players[0].connectionId).Disconnect();
        }
    }

    public override void OnStartServer()
    {
        NetworkServer.RegisterHandler(ExtMsgType.PlayerInfo, OnReceivePlayerInfo);
        base.OnStartServer();
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        OnServerAddPlayer(conn, playerControllerId, null);
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
    {
        NetworkServer.AddPlayerForConnection(conn, GameManager.singleton.SpawnPlayer(GetPlayer(conn.connectionId, playerControllerId)), playerControllerId);
    }

    public void SendScoreUpdate()
    {
        NetworkServer.SendToAll(ExtMsgType.Score, new ScoreMessage()
        {
            team1Score = GameManager.singleton.teams[0].GetComponent<Team>().points,
            team2Score = GameManager.singleton.teams[1].GetComponent<Team>().points
        });
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        playerList.RemoveAll(p => p.connectionId == conn.connectionId);
        base.OnServerDisconnect(conn);
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        conn.Send(ExtMsgType.PlayerInfo, new PlayerInfoMessage(playerList));
        base.OnServerConnect(conn);
    }

    public override void OnStopServer()
    {
        localPlayers = new List<Player>();
        playerList = new List<Player>();
        base.OnStopServer();
    }

    public override void OnStopClient()
    {
        localPlayers = new List<Player>();
        playerList = new List<Player>();
        base.OnStopClient();
    }

    public static NetManager GetInstance()
    {
        return singleton as NetManager;
    }

    public static Player GetPlayer(int connId, short controllerId)
    {
        return GetInstance().playerList.Find(p => p.connectionId == connId && p.controllerId == controllerId);
    }

    public static NetworkConnection GetConnection(Player ply)
    {
        foreach (NetworkConnection conn in NetworkServer.connections)
            if (conn.connectionId == ply.connectionId)
                return conn;
        return null;
    }

    public static NetworkConnection GetConnection(int connectionId)
    {
        foreach (NetworkConnection conn in NetworkServer.connections)
            if (conn.connectionId == connectionId)
                return conn;
        return null;
    }

    public bool IsGameJoinable(int playersJoining)
    {
        return (networkSceneName == lobbyScene && (playersJoining + playerList.Count <= 4));
    }

    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        if (!ClientScene.ready) { ClientScene.Ready(conn); }
        for (short i = 0; i < localPlayers.Count; i++)
        {
            if (ClientScene.localPlayers.Count >= i + 1 && ClientScene.localPlayers[i].IsValid && ClientScene.localPlayers[i].gameObject)
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
        client.RegisterHandler(ExtMsgType.PlayerInfo, OnReceivePlayerInfoClient);
        client.RegisterHandler(ExtMsgType.StartGame, GameManager.singleton.OnStartGame);
        client.RegisterHandler(ExtMsgType.Score, OnReceiveScore);
        base.OnClientConnect(conn);
    }

    public void OnReceiveScore(NetworkMessage netMsg)
    {
        ScoreMessage msg = netMsg.ReadMessage<ScoreMessage>();
        GameManager.singleton.team1Score = msg.team1Score;
        GameManager.singleton.team2Score = msg.team2Score;
    }

    public void OnReceivePlayerInfoClient(NetworkMessage netMsg)
    {
        PlayerInfoMessage msg = netMsg.ReadMessage<PlayerInfoMessage>();
        playerList = msg.players;
        if (GameObject.FindWithTag("MainCanvas") && GameObject.FindWithTag("MainCanvas").GetComponent<Lobby>())
            GameObject.FindWithTag("MainCanvas").GetComponent<Lobby>().UpdatePlayerDisplay();
    }

    public void OnPing(NetworkMessage netMsg)
    {
        PingMessage msg = netMsg.ReadMessage<PingMessage>();
        foreach (Player p in localPlayers)
            p.connectionId = msg.connectionId;
        Debug.Log("Received ping");
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
                players.Add(new Player() { name = reader.ReadString(), connectionId = reader.ReadInt32(), controllerId = reader.ReadInt16(), playerId = i });
            }
        }
    }

    public class PingMessage : MessageBase
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

    public class ScoreMessage : MessageBase
    {
        public int team1Score;
        public int team2Score;

        public override void Serialize(NetworkWriter writer)
        {
            writer.Write(team1Score);
            writer.Write(team2Score);
        }

        public override void Deserialize(NetworkReader reader)
        {
            team1Score = reader.ReadInt32();
            team2Score = reader.ReadInt32();
        }
    }

    public class ExtMsgType
    {
        public static short PlayerInfo = MsgType.Highest + 1;
        public static short Ping = MsgType.Highest + 2;
        public static short StartGame = MsgType.Highest + 3;
        public static short Score = MsgType.Highest + 4;
    }
}
