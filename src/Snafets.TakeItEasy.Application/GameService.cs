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
        private readonly IGameRepository _repository;

        public GameService(IGameRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Creates a new game for the given players and adds it to the repository.
        /// </summary>
        public TakeItEasyGame CreateGame(List<Player> players)
        {
            var game = new TakeItEasyGame(players);
            _repository.SaveGame(game);
            return game;
        }

        /// <summary>
        /// Gets a game by its unique ID.
        /// </summary>
        public TakeItEasyGame? GetGame(Guid id)
        {
            return _repository.LoadGame(id);
        }
    }
}
