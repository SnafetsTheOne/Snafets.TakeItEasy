using System;
using System.Collections.Generic;
using TakeItEasy.Domain;

namespace TakeItEasy.Application
{
    /// <summary>
    /// Application service for managing Take It Easy games.
    /// </summary>
    public class GameService
    {
        private readonly Dictionary<Guid, TakeItEasyGame> _games = new();

        /// <summary>
        /// Creates a new game for the given players and adds it to the service.
        /// </summary>
        public TakeItEasyGame CreateGame(List<Player> players)
        {
            var game = new TakeItEasyGame(players);
            _games[game.Id] = game;
            return game;
        }

        /// <summary>
        /// Gets a game by its unique ID.
        /// </summary>
        public TakeItEasyGame? GetGame(Guid id)
        {
            _games.TryGetValue(id, out var game);
            return game;
        }
    }
}
