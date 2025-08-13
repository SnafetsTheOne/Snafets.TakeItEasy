import React from "react";
import { Routes, Route } from "react-router-dom";
import { NotFoundPage } from "./NotFoundPage";
import { ProtectedRoute } from "./ProtectedRoute";
import { GamePage } from "../features/game/GamePage";
import { GamesPage } from "../features/game/GamesPage";
import { LobbyPage } from "../features/lobby/LobbyPage";
import { LobbiesPage } from "../features/lobby/LobbiesPage";
import { LoginPage } from "../features/player/LoginPage";
import { SignUpPage } from "../features/player/SignUpPage";

export const AppRoutes = () => {
  return (
    <Routes>
      <Route path="/" element={<LobbiesPage />} />
      <Route path="/lobbies" element={<LobbiesPage />} />
      <Route path="/lobby/:lobbyId" element={<LobbyPage />} />
      <Route element={<ProtectedRoute />}>
        <Route path="/games" element={<GamesPage />} />
      </Route>
      <Route path="/game/:gameId" element={<GamePage />} />
      <Route path="/login" element={<LoginPage />} />
      <Route path="/signup" element={<SignUpPage />} />
      <Route path="*" element={<NotFoundPage />} />
    </Routes>
  );
};