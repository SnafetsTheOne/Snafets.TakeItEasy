import { createContext, useContext, useEffect, useMemo, useRef } from "react";
import { buildConnection } from "../data-access/realtime";

const Ctx = createContext(null);

export function RealtimeProvider({  children, }) {
  const connectionRef = useRef(null);
  const handlersRef = useRef(new Map());

  // Helper to rebind all handlers
  const rebindAll = () => {
    const conn = connectionRef.current;
    handlersRef.current.forEach((set, evt) => {
      conn.off(evt);
      set.forEach((h) => conn.on(evt, h));
    });
  };

  // Helper to start connection
  const startConnection = async () => {
    const conn = buildConnection();
    connectionRef.current = conn;
    conn.onreconnected(rebindAll);
    conn.onreconnecting(() => {});
    try {
      await conn.start();
      rebindAll();
    } catch (e) {
      console.error("SignalR start failed", e);
    }
  };

  // Expose restartConnection
  const restartConnection = async () => {
    if (connectionRef.current) {
      try {
        await connectionRef.current.stop();
      } catch {}
      connectionRef.current = null;
    }
    await startConnection();
  };

  useEffect(() => {
    startConnection();
    return () => {
      if (connectionRef.current) connectionRef.current.stop().catch(() => {});
      connectionRef.current = null;
      handlersRef.current.clear();
    };
  }, []);

  const value = useMemo(() => {
    return {
      connection: connectionRef.current,
      on: (event, handler) => {
        const conn = connectionRef.current;
        if (!handlersRef.current.has(event)) handlersRef.current.set(event, new Set());
        handlersRef.current.get(event).add(handler);
        if (conn) conn.on(event, handler);
        // Unsubscribe
        return () => {
          handlersRef.current.get(event)?.delete(handler);
          connectionRef.current?.off(event, handler);
        };
      },
      send: async (method, ...args) => {
        const conn = connectionRef.current;
        if (!conn) throw new Error("No SignalR connection");
        await conn.send(method, ...args);
      },
      restartConnection, // <-- Expose here
    };
  }, []);

  return <Ctx.Provider value={value}>{children}</Ctx.Provider>;
}

export function useRealtime() {
  const ctx = useContext(Ctx);
  if (!ctx) throw new Error("useRealtime must be used inside <RealtimeProvider>");
  return ctx;
}