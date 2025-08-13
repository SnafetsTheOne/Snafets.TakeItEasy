import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../../infra/AuthProvider";
import { sha256Hex } from "./sha256";

export const LoginPage = () => {
  const { login } = useAuth();
  const [name, setName] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    const passwordHash = await sha256Hex(name + ':' + password);
    await login(name, passwordHash);
    setName("");
    setPassword("");
    navigate("/");
  };

  return (
    <div
      style={{ maxWidth: 400, margin: "3rem auto", padding: "2rem 1.5rem", background: "#fff", borderRadius: 10, boxShadow: "0 2px 8px rgba(0,0,0,0.04)", }}
    >
      <h1
        style={{ fontSize: "1.7rem", fontWeight: 600, marginBottom: "2rem", color: "#222", textAlign: "center", }}
      >
        Login
      </h1>
      <form
        onSubmit={handleSubmit}
        style={{ display: "flex", flexDirection: "column", gap: "1.2rem" }}
      >
        <input
          type="text"
          value={name}
          onChange={(e) => setName(e.target.value)}
          placeholder="Name"
          style={{ padding: "0.7rem 1rem", fontSize: "1rem", border: "1px solid #ddd", borderRadius: 6, background: "#fafafa", color: "#222", }}
          required
        />
        <input
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          placeholder="Password"
          style={{ padding: "0.7rem 1rem", fontSize: "1rem", border: "1px solid #ddd", borderRadius: 6, background: "#fafafa", color: "#222", }}
          required
        />
        <button
          type="submit"
          style={{ padding: "0.7rem 1.5rem", fontSize: "1rem", border: "1px solid #ddd", borderRadius: 6, background: "#fff", color: "#222", fontWeight: 500, cursor: "pointer", }}
        >
          Login
        </button>
      </form>
    </div>
  );
};
