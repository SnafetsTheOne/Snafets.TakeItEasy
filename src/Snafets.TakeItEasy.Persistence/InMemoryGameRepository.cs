using System;
using System.Collections.Generic;
using TakeItEasy.Application;
using TakeItEasy.Domain;

namespace Snafets.TakeItEasy.Persistence
{
    public class InMemoryGameRepository : IGameRepository
    {
        private readonly Dictionary<Guid, TakeItEasyGame> _store = new();

        public void SaveGame(TakeItEasyGame game) => _store[game.Id] = game;
        public TakeItEasyGame? LoadGame(Guid id) => _store.TryGetValue(id, out var game) ? game : null;
        public IEnumerable<TakeItEasyGame> GetAllGames() => _store.Values;
        public void DeleteGame(Guid id) => _store.Remove(id);
    }
}
