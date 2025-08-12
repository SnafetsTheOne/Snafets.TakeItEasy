import React from "react";
import { Navigate, Outlet } from "react-router-dom";
import { useAuth } from "./AuthProvider";

export const ProtectedRoute = () => {
  const { status, user } = useAuth();

  if (status === "loading" || status === "idle") {
    return <div className="p-6">Checking session…</div>;
  }

  if (status === "unauthenticated") {
    return <Navigate to="/login" replace />;
  }

  return <Outlet />;
};