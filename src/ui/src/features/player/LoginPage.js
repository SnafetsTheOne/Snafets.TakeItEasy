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
    <div className="container" style={{ 
      width: "80%",
      maxWidth: "400px",
      minHeight: "calc(100vh - 150px)",
      display: "flex",
      flexDirection: "column",
      justifyContent: "center"
    }}>
      <div style={{ 
        background: "#fff", 
        borderRadius: 12, 
        boxShadow: "0 4px 16px rgba(0,0,0,0.08)", 
        padding: "1rem 1rem",
        width: "100%"
      }}>
        <h1 style={{ 
          fontSize: "1.75rem", 
          fontWeight: 600, 
          marginBottom: "1.5rem", 
          color: "#222", 
          textAlign: "center" 
        }}>
          Login
        </h1>
        <form
          onSubmit={handleSubmit}
          style={{ display: "flex", flexDirection: "column", gap: "1rem" }}
        >
          <div>
            <label htmlFor="name" style={{
              display: "block",
              marginBottom: "0.25rem",
              fontWeight: 500,
              color: "#374151",
              fontSize: "0.85rem"
            }}>
              Name
            </label>
            <input
              id="name"
              type="text"
              value={name}
              onChange={(e) => setName(e.target.value)}
              placeholder="Enter your name"
              style={{ 
                width: "100%",
                padding: "0.75rem 0.875rem", 
                fontSize: "1rem", 
                border: "1px solid #d1d5db", 
                borderRadius: 8, 
                background: "#fff", 
                color: "#222",
                boxSizing: "border-box",
                minHeight: "44px"
              }}
              required
            />
          </div>
          <div>
            <label htmlFor="password" style={{
              display: "block",
              marginBottom: "0.25rem",
              fontWeight: 500,
              color: "#374151",
              fontSize: "0.85rem"
            }}>
              Password
            </label>
            <input
              id="password"
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              placeholder="Enter your password"
              style={{ 
                width: "100%",
                padding: "0.75rem 0.875rem", 
                fontSize: "1rem", 
                border: "1px solid #d1d5db", 
                borderRadius: 8, 
                background: "#fff", 
                color: "#222",
                boxSizing: "border-box",
                minHeight: "44px"
              }}
              required
            />
          </div>
          <button
            type="submit"
            className="btn btn-primary"
            style={{ 
              marginTop: "0.5rem",
              width: "100%",
              fontSize: "1rem",
              fontWeight: 600,
              padding: "0.75rem 1rem"
            }}
          >
            Login
          </button>
        </form>
      </div>
    </div>
  );
};
