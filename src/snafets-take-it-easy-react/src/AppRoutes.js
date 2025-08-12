import React from "react";
import { Routes, Route } from "react-router-dom";
import { NotFoundPage } from "./pages/NotFoundPage";
import { GamePage } from "./features/game/GamePage";
import { GamesPage } from "./features/game/GamesPage";
import { LobbyPage } from "./features/lobby/LobbyPage";
import { LobbiesPage } from "./features/lobby/LobbiesPage";


export const AppRoutes = () => {
  return (
    <Routes>
      <Route path="/" element={<LobbiesPage />} />
      <Route path="/lobby" element={<LobbiesPage />} />
      <Route path="/lobby/:lobbyId" element={<LobbyPage />} />
      <Route path="/game" element={<GamesPage />} />
      <Route path="/game/:gameId" element={<GamePage />} />
      <Route path="*" element={<NotFoundPage />} />
    </Routes>
  );
};