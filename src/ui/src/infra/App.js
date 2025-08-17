import './App.css';
import { AppRoutes } from "./AppRoutes";
import Navbar from "./Navbar";

function App() {
  return (
    <div className="App">
      <Navbar />
      <div className="flex-grow">
        <AppRoutes />
      </div>
    </div>
  );
}

export default App;
