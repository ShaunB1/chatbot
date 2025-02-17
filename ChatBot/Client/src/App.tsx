import './App.css'
import SidePanel from "./components/SidePanel/SidePanel.tsx";
import {Route, Routes} from "react-router-dom";
import ChatPage from "./pages/ChatPage/ChatPage.tsx";
import HomePage from "./pages/HomePage/HomePage.tsx";
import NavBar from "./components/NavBar/NavBar.tsx";
import AppCSS from "./App.module.css";

function App() {
  return (
      <>
          <NavBar />
          <SidePanel />
          <div className={AppCSS.routesContainer}>
              <Routes>
                  <Route path="/" element={<HomePage />} />
                  <Route path="/chat" element={<ChatPage />} />
              </Routes>
          </div>
      </>
  )
}

export default App
