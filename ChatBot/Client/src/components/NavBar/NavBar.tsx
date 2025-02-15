import {Assistant} from "@mui/icons-material";
import "../SidePanel/Sidepanel.module.css";

const NavBar = () => {
    return (
        <>
            <div
                style={{
                    height: "50px",
                    display: "flex",
                    alignItems: "center",
                    gap: "8px",
                    padding: "8px 24px",
                    background: "white",
                    position: "fixed",
                    width: "100%",
                }}
            >
                <Assistant fontSize="large" sx={{ color: "black" }} />
                <p style={{ fontSize: "2rem" }}>ChatBot</p>
            </div>
        </>
    );
}

export default NavBar;