import logo from './logo.svg';
import './App.css';
import { AppRoutes } from "./AppRoutes";

function App() {
  return (
    <div className="App">
      <div className="flex-grow">
        <AppRoutes />
      </div>
    </div>
  );
}

export default App;
