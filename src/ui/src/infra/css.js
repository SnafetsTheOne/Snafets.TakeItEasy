const baseGap = "0.5rem";

export const verticalContainer = {
  display: "flex",
  alignItems: "center",
  flexDirection: "column",
  justifyContent: "space-between",
  minWidth: 0,
  gap: baseGap,
};

export const verticalContainerItem = {
  display: "flex",
  minHeight: 0,
  gap: baseGap,
  alignSelf: "stretch"
};

export const horizontalContainer = {
  display: "flex",
  alignItems: "flex-start",
  flexDirection: "row",
  justifyContent: "space-between",
  minWidth: 0,
  gap: baseGap,
};

export const horizontalContainerItem = {
  display: "flex",
  minHeight: 0,
  gap: baseGap,
};

export const head1 = {
  fontSize: "2.5rem",
  fontWeight: 600,
  color: "#222",
  textAlign: "center",
  alignSelf: "center"
};

export const head2 = {
  fontSize: "1.8rem",
  fontWeight: 400,
  color: "#222",
  textAlign: "center",
  alignSelf: "center"
};

export const cardStyle = {
  border: "none",
  background: "#e3f2fd",
  borderRadius: "12px",
  boxShadow: "0 2px 8px rgba(0,0,0,0.07)",
  padding: "1.5rem",
  transition: "box-shadow 0.2s",
};

export const cardNameStyle = {
  fontSize: "1.6rem",
  fontWeight: 700,
  color: "#222",
  letterSpacing: "0.02em",
  display: "block",
  wordBreak: "break-word",
};

export const cardIdStyle = {
  fontSize: "0.8rem",
  color: "#bbb",
  marginLeft: "1.2rem",
  fontWeight: 400,
  letterSpacing: "0.01em",
  display: "block",
  marginTop: "0.2rem",
};
