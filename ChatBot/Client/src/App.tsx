import './App.css'
import SidePanel from "./components/SidePanel/SidePanel.tsx";
import {Route, Routes} from "react-router-dom";
import ChatPage from "./pages/ChatPage/ChatPage.tsx";
import HomePage from "./pages/HomePage/HomePage.tsx";
import NavBar from "./components/NavBar/NavBar.tsx";
import AppCSS from "./App.module.css";
import {useState} from "react";

function App() {
    const [model, setModel] = useState<string>("gpt-4o-mini-2024-07-18");

    return (
        <>
            <NavBar model={model} setModel={setModel} />
            <SidePanel />
            <div className={AppCSS.routesContainer}>
                <Routes>
                    <Route path="/" element={<HomePage />} />
                    <Route path="/chat" element={<ChatPage model={model} />} />
                </Routes>
            </div>
        </>
    )
}

export default App
