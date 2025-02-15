import {useNavigate} from "react-router-dom";
import SidePanelCSS from "./SidePanel.module.css";
import {KeyboardDoubleArrowLeft} from "@mui/icons-material";

const SidePanel = () => {
    const navigate = useNavigate();

    return (
        <>
            <div
                style={{
                    width: "200px",
                    overflow: "hidden",
                    position: "fixed",
                    top: "66px",
                    height: "100vh",
                    background: "white",
                }}
            >
                <div
                    style={{
                        height: "86%",
                        display: "flex",
                        flexDirection: "column",
                        alignItems: "center",
                        gap: "12px",
                        paddingTop: "12px",
                    }}
                >
                    <button className={SidePanelCSS.sidePanelButton} onClick={() => navigate("/")}>Home</button>
                    <button className={SidePanelCSS.sidePanelButton} onClick={() => navigate("/chat")}>Chat</button>
                </div>
                <div
                    style={{
                        display: "flex",
                        justifyContent: "flex-end",
                        position: "relative",
                        right: "15px",
                    }}
                >
                    <button className={SidePanelCSS.collapseButton} id="collapse-button">
                        <KeyboardDoubleArrowLeft sx={{ color: "gray" }} />
                    </button>
                </div>
            </div>
        </>
    );
}

export default SidePanel;