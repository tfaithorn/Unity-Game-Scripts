using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerDatabase
{
    private static Dictionary<long, Player> players;
    private static Player lastPlayedPlayer;
    private static List<Player> playerList;

    static PlayerDatabase()
    {
        players = new Dictionary<long, Player>();

        var playerRepository = new PlayerRepository();
        playerList = playerRepository.GetPlayers();

        foreach (var player in playerList)
        {
            if (lastPlayedPlayer == null || lastPlayedPlayer.lastPlayed > player.lastPlayed)
            {
                lastPlayedPlayer = player;
            }

            players.Add(player.id, player);
        }
    }

    public static Player GetPlayer(long playerId)
    {
        return players[playerId];
    }

    public static Player GetLastPlayed()
    {
        return lastPlayedPlayer;
    }

    public static List<Player> GetPlayerList()
    {
        return playerList;
    }
}
