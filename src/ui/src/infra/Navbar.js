import { Link } from "react-router-dom";
import { useAuth } from "./AuthProvider";
import { useEffect, useState } from "react";
import { useRealtime } from "./RealtimeProvider";

const Navbar = () => {
	const { user, status, logout } = useAuth();
	const { on } = useRealtime();
	const [time, setTime] = useState(Date.now());
	const [showTimeInput, setShowTimeInput] = useState(false);

	useEffect(() => {
    	on("broadcast", (p) => setTime(p));
	}, [on]);

	return (
		<nav style={{
			width: "100%",
			background: "#fff",
			borderBottom: "1px solid #eee",
			padding: "0.75rem 0",
			marginBottom: "2rem",
			boxShadow: "0 2px 8px rgba(0,0,0,0.03)",
			position: "sticky",
			top: 0,
			zIndex: 100
		}}>
			<div style={{
				maxWidth: 900,
				margin: "0 auto",
				display: "flex",
				alignItems: "center",
				gap: "2rem"
			}}>
				<Link to="/lobbies" style={{ textDecoration: "none", color: "#222", fontWeight: 500, fontSize: "1.1rem" }}>Lobbies</Link>
				{status === "authenticated" ? (

					<Link to="/games" style={{ textDecoration: "none", color: "#222", fontWeight: 500, fontSize: "1.1rem" }}>Games</Link>
				) : null}
				<div
					style={{ marginLeft: "auto", display: "flex", gap: "1.2rem", alignItems: "center" }}
					onMouseEnter={() => setShowTimeInput(true)}
					onMouseLeave={() => setShowTimeInput(false)}
				>
					{status === "authenticated" ? (
						<>
							{showTimeInput && (
								<input type="text" value={time} readOnly style={{ width: '200px', textAlign: 'center', transition: 'opacity 0.2s' }} />
							)}
							<span style={{ fontWeight: 500, fontSize: "1.1rem" }}>{user.name}</span>
							<button onClick={logout} style={{ background: "none", border: "none", color: "#222", fontWeight: 500, fontSize: "1.1rem", cursor: "pointer" }}>Logout</button>
						</>
					) : (
						<>
							<Link to="/login" style={{ textDecoration: "none", color: "#222", fontWeight: 500, fontSize: "1.1rem" }}>Login</Link>
							<Link to="/signup" style={{ textDecoration: "none", color: "#222", fontWeight: 500, fontSize: "1.1rem" }}>Sign Up</Link>
						</>
					)}
				</div>
			</div>
		</nav>
	);
};

export default Navbar;
