import HoneycombStandaloneCell from "../../components/Honeycomb/HoneycombStandaloneCell";
import HoneycombBoard from "../../components/Honeycomb/HoneycombBoard";

export function GameBoard({ title, board, currentTile, gameId, reloadGame, isUser }) {
  console.log(board)
  currentTile = board.canPlay ? currentTile : null;
  return (
    <div style={{ display: 'flex', flexDirection: 'column', flex: 1, boxSizing: 'border-box', alignItems: 'center', justifyContent: 'flex-start', }}>
      <div style={{display: 'flex',flexDirection: 'column',gap: 32,alignItems: 'center',justifyContent: 'flex-start',width: '100%',maxWidth: 900,margin: '0 auto',boxSizing: 'border-box', }}>
        {/* Game Board header */}
        <div style={{ width: '100%', display: 'flex', justifyContent: 'center' }}>
          <div style={{ minWidth: 180, height: 48, textAlign: 'center', background: '#fff', padding: '0.5rem 1.5rem', borderRadius: 10, boxShadow: '0 1px 6px rgba(0,0,0,0.08)', fontWeight: 600, fontSize: '1.5rem', color: '#222', display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center' }}>
            {title}
          </div>
        </div>

        <div style={{ display: 'flex', flexDirection: 'row', gap: 40, alignItems: 'flex-start', justifyContent: 'center', width: '100%' }}>
          {/* Score */}
          <div style={{ minWidth: 120, textAlign: 'center', background: '#ffffffff', padding: '1.2rem 1.5rem', borderRadius: 10, boxShadow: '0 1px 6px rgba(0,0,0,0.04)', fontWeight: 600, fontSize: '1.1rem', color: '#222', display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center', height: 120 }}>
            <span style={{ fontSize: '0.9rem', color: '#888', fontWeight: 400, marginBottom: 4 }}>Score</span>
            <span style={{ fontSize: '2rem', fontWeight: 700 }}>{board?.score != null ? board.score : '-'}</span>
          </div>

          {/* Next Cell Preview (no text) */}
          <div style={{ minWidth: 120, textAlign: 'center', background: '#ffffffff', padding: '1.2rem 1.5rem', borderRadius: 10, boxShadow: '0 1px 6px rgba(0,0,0,0.04)', display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center', height: 120 }}>
            {currentTile ? (
              <HoneycombStandaloneCell tile={currentTile} />
            ) : (
              <span style={{ color: '#bbb', fontSize: '1.2rem' }}>—</span>
            )}
          </div>
        </div>

        {/* Game Board below */}
        <div style={{ width: '50%', minWidth: 320, maxWidth: 480, display: 'flex', background: '#ffffffff', flexDirection: 'column', alignItems: 'center', justifyContent: 'center', borderRadius: 10, boxShadow: '0 1px 6px rgba(0,0,0,0.04)', padding: '1.2rem 1.5rem', minHeight: 400 }}>
          <HoneycombBoard 
            tiles={board.spaces} 
            playerId={board.playerId} 
            gameId={gameId} 
            canPlay={isUser ? board.canPlay : false}
            reloadGame={isUser ? reloadGame : () => {}}
          />
        </div>
      </div>
    </div>
  );
}
