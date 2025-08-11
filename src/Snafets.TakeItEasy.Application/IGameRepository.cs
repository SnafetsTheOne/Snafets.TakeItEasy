using System;
using System.Collections.Generic;
using TakeItEasy.Domain;

namespace TakeItEasy.Application
{
    public interface IGameRepository
    {
        void SaveGame(TakeItEasyGame game);
        TakeItEasyGame? LoadGame(Guid id);
        IEnumerable<TakeItEasyGame> GetAllGames();
        void DeleteGame(Guid id);
    }
}
