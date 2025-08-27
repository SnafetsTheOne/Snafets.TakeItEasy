import React from "react";
import { Routes, Route } from "react-router-dom";
import { NotFoundPage } from "./NotFoundPage";
import { ProtectedRoute } from "./ProtectedRoute";
import { GamePage } from "../pages/game/GamePage";
import { GamesPage } from "../pages/game/GamesPage";
import { LobbyPage } from "../pages/lobby/LobbyPage";
import { LobbiesPage } from "../pages/lobby/LobbiesPage";
import { LoginPage } from "../pages/player/LoginPage";
import { SignUpPage } from "../pages/player/SignUpPage";

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