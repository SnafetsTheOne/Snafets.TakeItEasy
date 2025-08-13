import { createContext, useContext, useEffect, useMemo, useRef } from "react";
import { buildConnection } from "../data-access/realtime";

const Ctx = createContext(null);

export function RealtimeProvider({  children, }) {
  const connectionRef = useRef(null);
  const handlersRef = useRef(new Map());

  useEffect(() => {
    const conn = buildConnection();
    connectionRef.current = conn;

    // Rebind all handlers after reconnects
    const rebindAll = () => {
      handlersRef.current.forEach((set, evt) => {
        // Clear any previous bindings for safety
        conn.off(evt);
        set.forEach((h) => conn.on(evt, h));
      });
    };

    conn.onreconnected(rebindAll);
    conn.onreconnecting(() => {
      // optional: emit an app-wide status event
    });

    conn
      .start()
      .then(rebindAll)
      .catch((e) => console.error("SignalR start failed", e));

    return () => {
      conn.stop().catch(() => {});
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
    };
  }, []);

  return <Ctx.Provider value={value}>{children}</Ctx.Provider>;
}

export function useRealtime() {
  const ctx = useContext(Ctx);
  if (!ctx) throw new Error("useRealtime must be used inside <RealtimeProvider>");
  return ctx;
}