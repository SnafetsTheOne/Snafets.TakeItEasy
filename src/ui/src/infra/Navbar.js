import { Link } from "react-router-dom";
import { useAuth } from "./AuthProvider";
import { useEffect, useState } from "react";
import { useRealtime } from "./RealtimeProvider";

const Navbar = () => {
	const { user, status, logout } = useAuth();
	const { on } = useRealtime();
	const [time, setTime] = useState(Date.now());
	const [showTimeInput, setShowTimeInput] = useState(false);
	const [isMobileMenuOpen, setIsMobileMenuOpen] = useState(false);

	useEffect(() => {
    	on("broadcast", (p) => setTime(p));
	}, [on]);

	const toggleMobileMenu = () => {
		setIsMobileMenuOpen(!isMobileMenuOpen);
	};

	const closeMobileMenu = () => {
		setIsMobileMenuOpen(false);
	};

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
				gap: "2rem",
				padding: "0 1rem"
			}}>
				{/* Mobile menu button */}
				<button 
					onClick={toggleMobileMenu}
					style={{
						display: "flex",
						flexDirection: "column",
						justifyContent: "space-around",
						width: "2rem",
						height: "2rem",
						background: "transparent",
						border: "none",
						cursor: "pointer",
						padding: 0,
						zIndex: 10
					}}
					className="desktop-hidden"
				>
					<span style={{
						width: "2rem",
						height: "0.25rem",
						background: "#222",
						borderRadius: "10px",
						transition: "all 0.3s linear",
						transform: isMobileMenuOpen ? "rotate(45deg)" : "rotate(0)",
						transformOrigin: "1px"
					}} />
					<span style={{
						width: "2rem",
						height: "0.25rem",
						background: "#222",
						borderRadius: "10px",
						transition: "all 0.3s linear",
						opacity: isMobileMenuOpen ? "0" : "1",
						transform: isMobileMenuOpen ? "translateX(20px)" : "translateX(0)"
					}} />
					<span style={{
						width: "2rem",
						height: "0.25rem",
						background: "#222",
						borderRadius: "10px",
						transition: "all 0.3s linear",
						transform: isMobileMenuOpen ? "rotate(-45deg)" : "rotate(0)",
						transformOrigin: "1px"
					}} />
				</button>

				{/* Desktop navigation */}
				<div className="mobile-hidden" style={{ display: "flex", alignItems: "center", gap: "2rem" }}>
					<Link to="/lobbies" style={{ textDecoration: "none", color: "#222", fontWeight: 500, fontSize: "1.1rem" }}>Lobbies</Link>
					{status === "authenticated" ? (
						<Link to="/games" style={{ textDecoration: "none", color: "#222", fontWeight: 500, fontSize: "1.1rem" }}>Games</Link>
					) : null}
				</div>

				{/* Desktop user section */}
				<div
					className="mobile-hidden"
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

				{/* Mobile menu overlay */}
				{isMobileMenuOpen && (
					<div style={{
						position: "fixed",
						top: 0,
						left: 0,
						right: 0,
						bottom: 0,
						background: "rgba(0,0,0,0.5)",
						zIndex: 5
					}} onClick={closeMobileMenu} />
				)}

				{/* Mobile menu */}
				<div style={{
					position: "fixed",
					top: 0,
					left: 0,
					width: "280px",
					height: "100vh",
					background: "#fff",
					transform: isMobileMenuOpen ? "translateX(0)" : "translateX(-100%)",
					transition: "transform 0.3s ease-in-out",
					zIndex: 6,
					padding: "5rem 2rem 2rem 2rem",
					boxShadow: "2px 0 8px rgba(0,0,0,0.1)"
				}}>
					<div style={{ display: "flex", flexDirection: "column", gap: "1.5rem" }}>
						<Link 
							to="/lobbies" 
							onClick={closeMobileMenu}
							style={{ 
								textDecoration: "none", 
								color: "#222", 
								fontWeight: 500, 
								fontSize: "1.2rem",
								padding: "0.75rem 0",
								borderBottom: "1px solid #eee"
							}}
						>
							Lobbies
						</Link>
						
						{status === "authenticated" ? (
							<Link 
								to="/games" 
								onClick={closeMobileMenu}
								style={{ 
									textDecoration: "none", 
									color: "#222", 
									fontWeight: 500, 
									fontSize: "1.2rem",
									padding: "0.75rem 0",
									borderBottom: "1px solid #eee"
								}}
							>
								Games
							</Link>
						) : null}

						<div style={{ marginTop: "2rem", paddingTop: "1rem", borderTop: "2px solid #eee" }}>
							{status === "authenticated" ? (
								<>
									<div style={{ marginBottom: "1rem" }}>
										<span style={{ fontWeight: 500, fontSize: "1.1rem", color: "#666" }}>Logged in as:</span>
										<div style={{ fontWeight: 600, fontSize: "1.2rem", color: "#222", marginTop: "0.5rem" }}>{user.name}</div>
									</div>
									<button 
										onClick={() => { logout(); closeMobileMenu(); }} 
										style={{ 
											background: "#ef4444", 
											border: "none", 
											color: "white", 
											fontWeight: 500, 
											fontSize: "1.1rem", 
											cursor: "pointer",
											padding: "0.75rem 1.5rem",
											borderRadius: "8px",
											width: "100%"
										}}
									>
										Logout
									</button>
								</>
							) : (
								<>
									<Link 
										to="/login" 
										onClick={closeMobileMenu}
										style={{ 
											textDecoration: "none", 
											color: "#222", 
											fontWeight: 500, 
											fontSize: "1.1rem",
											padding: "0.75rem 0",
											borderBottom: "1px solid #eee",
											display: "block"
										}}
									>
										Login
									</Link>
									<Link 
										to="/signup" 
										onClick={closeMobileMenu}
										style={{ 
											textDecoration: "none", 
											color: "#222", 
											fontWeight: 500, 
											fontSize: "1.1rem",
											padding: "0.75rem 0",
											borderBottom: "1px solid #eee",
											display: "block"
										}}
									>
										Sign Up
									</Link>
								</>
							)}
						</div>
					</div>
				</div>
			</div>
		</nav>
	);
};

export default Navbar;
