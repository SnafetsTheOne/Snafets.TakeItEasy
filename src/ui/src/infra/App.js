import './App.css';
import { AppRoutes } from "./AppRoutes";
import Navbar from "./Navbar";

function App() {
  return (
    <div className="App" style={{ height: '100vh', padding: '1rem' }}>
      <Navbar />
      <div className="flex-grow">
        <AppRoutes />
      </div>
    </div>
  );
}

export default App;
