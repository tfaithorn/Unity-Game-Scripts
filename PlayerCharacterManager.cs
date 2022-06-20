using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class PlayerCharacterManager
{
    /// <summary>
    /// A list of all available players (used by player section dropdown)
    /// </summary>
    public static List<PlayerCharacter> playerCharacters;

    /// <summary>
    /// The current player character
    /// </summary>
    public static PlayerCharacter playerCharacter;

    public static void LoadPlayerCharacters()
    {
        var playerCharacterRepository = new PlayerCharacterRepository();
        playerCharacters = playerCharacterRepository.GetByCriteria();
        playerCharacter = GetMostRecentlyPlayed();
    }

    public static PlayerCharacter GetMostRecentlyPlayed()
    {
        return playerCharacters.OrderByDescending(x => x.lastPlayed).FirstOrDefault();
    }
}
