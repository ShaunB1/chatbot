import './App.css'
import NavBar from "./components/NavBar/NavBar.tsx";
import SidePanel from "./components/SidePanel/SidePanel.tsx";
import {Route, Routes} from "react-router-dom";
import ChatPage from "./pages/ChatPage/ChatPage.tsx";
import HomePage from "./pages/HomePage/HomePage.tsx";

function App() {
  return (
      <>
          <div>
              <NavBar />
              <SidePanel />
              <div
                  style={{
                      position: "relative",
                      display: "flex",
                      width: "100%",
                      height: "100vh",
                      outline: "2px solid red",
                      left: "202px",
                      top: "69px",
                      padding: "12px",
                  }}
              >
                  <Routes>
                      <Route path="/" element={<HomePage />} />
                      <Route path="/chat" element={<ChatPage />} />
                  </Routes>
              </div>
          </div>
      </>
  )
}

export default App
