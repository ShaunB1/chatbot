import {useNavigate} from "react-router-dom";
import SidePanelCSS from "./SidePanel.module.css";
import {KeyboardDoubleArrowLeft} from "@mui/icons-material";

const SidePanel = () => {
    const navigate = useNavigate();

    return (
        <>
            <div className={SidePanelCSS.sidepanelContainer}>
                <div className={SidePanelCSS.routesContainer}>
                    <button
                        className={SidePanelCSS.sidePanelButton}
                        onClick={() => navigate("/")}
                        aria-label="Navigate to Home"
                    >
                        Home
                    </button>
                    <button
                        className={SidePanelCSS.sidePanelButton}
                        onClick={() => navigate("/chat")}
                        aria-label="Navigate to Chat"
                    >
                        Chat
                    </button>
                </div>
                <div className={SidePanelCSS.footerContainer}>
                    <button
                        className={SidePanelCSS.collapseButton}
                        id="collapse-button"
                        aria-label="Collapse Sidepanel"
                    >
                        <KeyboardDoubleArrowLeft sx={{ color: "gray" }} />
                    </button>
                </div>
            </div>
        </>
    );
}

export default SidePanel;