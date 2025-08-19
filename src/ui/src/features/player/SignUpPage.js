import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../../infra/AuthProvider";
import { signUp } from "../../data-access/player";
import { sha256Hex } from "./sha256";
import { useRealtime } from "../../infra/RealtimeProvider";
import {
  cardStyle,
  cardNameStyle,
  cardIdStyle,
  head1,
  head2,
  verticalContainer,
  verticalContainerItem,
} from "../../infra/css";

export const SignUpPage = () => {
  const { login } = useAuth();
  const { restartConnection } = useRealtime();
  const [name, setName] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    const passwordHash = await sha256Hex(name + ":" + password);
    await signUp(name, passwordHash);
    await login(name, passwordHash);
    await restartConnection();
    setName("");
    setPassword("");
    navigate("/");
  };

  return (
    <div
      style={{
        ...verticalContainer,
        maxWidth: 400,
        margin: "auto",
        background: "#fff",
        borderRadius: 12,
        boxShadow: "0 4px 16px rgba(0,0,0,0.08)",
        padding: "1.5rem 1rem",
      }}
    >
      <h1
        style={{
          ...verticalContainerItem,
          ...head1,
        }}
      >
        Sign Up
      </h1>
      <form
        onSubmit={handleSubmit}
        style={{
          ...verticalContainerItem,
          ...verticalContainer,
          display: "flex",
          flexDirection: "column",
          gap: "1rem",
        }}
      >
        <div style={{ ...verticalContainerItem, ...verticalContainer }}>
          <label
            htmlFor="name"
            style={{
              ...verticalContainerItem,
              display: "block",
              marginBottom: "0.25rem",
              fontWeight: 500,
              color: "#374151",
              fontSize: "0.85rem",
            }}
          >
            Name
          </label>
          <input
            id="name"
            type="text"
            value={name}
            onChange={(e) => setName(e.target.value)}
            placeholder="Enter your name"
            style={{
              ...verticalContainerItem,
              padding: "0.75rem 0.875rem",
              fontSize: "1rem",
              border: "1px solid #d1d5db",
              borderRadius: 8,
              background: "#fff",
              color: "#222",
              boxSizing: "border-box",
              minHeight: "44px",
            }}
            required
          />
        </div>
        <div style={{ ...verticalContainerItem, ...verticalContainer }}>
          <label
            htmlFor="password"
            style={{
              ...verticalContainerItem,
              display: "block",
              marginBottom: "0.25rem",
              fontWeight: 500,
              color: "#374151",
              fontSize: "0.85rem",
            }}
          >
            Password
          </label>
          <input
            id="password"
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            placeholder="Enter your password"
            style={{
              ...verticalContainerItem,
              padding: "0.75rem 0.875rem",
              fontSize: "1rem",
              border: "1px solid #d1d5db",
              borderRadius: 8,
              background: "#fff",
              color: "#222",
              boxSizing: "border-box",
              minHeight: "44px",
            }}
            required
          />
        </div>
        <button
          type="submit"
          className="btn btn-primary"
          style={{
            ...verticalContainerItem,
            marginTop: "0.5rem",
            fontSize: "1rem",
            fontWeight: 600,
            padding: "0.75rem 1rem",
          }}
        >
          Sign Up
        </button>
      </form>
    </div>
  );
};
