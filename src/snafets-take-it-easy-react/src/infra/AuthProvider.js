import React, { useState, useEffect, useCallback, useContext, createContext } from "react";

import { loginPlayer, logoutPlayer, me } from "../data-access/player";

const AuthContext = createContext(null);

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [status, setStatus] = useState("idle");

  const fetchMe = useCallback(async () => {
    setStatus("loading");
    try {
      const data = await me();
      setUser(data);
      setStatus("authenticated");
    } catch (error) {
      console.error("Failed to fetch user data", error);
      setUser(null);
      setStatus("unauthenticated");
    }
  }, []);

  useEffect(() => {
    // Hydrate on mount
    fetchMe();
  }, [fetchMe]);

  useEffect(() => {
    const onStorage = (e) => {
      if (e.key === "app:logout") {
        setUser(null);
        setStatus("unauthenticated");
      }
    };
    window.addEventListener("storage", onStorage);
    return () => window.removeEventListener("storage", onStorage);
  }, []);

  const login = async (name, password) => {
    setStatus("loading");
    await loginPlayer(name, password);
    await fetchMe(); 
  };

  const logout = async () => {
    await logoutPlayer();
    setUser(null);
    setStatus("unauthenticated");
    // Optional: cross-tab logout
    localStorage.setItem("app:logout", Date.now().toString());
  };

  return (
    <AuthContext.Provider value={{ user, status, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error("useAuth must be used within AuthProvider");
  return ctx;
};
