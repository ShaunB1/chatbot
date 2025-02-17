import {Assistant} from "@mui/icons-material";
import "../SidePanel/Sidepanel.module.css";
import NavBarCSS from "./NavBar.module.css"

const NavBar = () => {
    return (
        <>
            <div className={NavBarCSS.container}>
                <Assistant fontSize="large" sx={{ color: "white" }} />
                <p style={{ fontSize: "2rem", color: "white" }}>ChatBot</p>
            </div>
        </>
    );
}

export default NavBar;