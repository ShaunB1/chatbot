import {Assistant} from "@mui/icons-material";
import "../SidePanel/Sidepanel.module.css";
import NavBarCSS from "./NavBar.module.css"

interface NavBarProps {
    model: string,
    setModel: (model: string) => void;
}

const NavBar: React.FC<NavBarProps> = ({ model, setModel }) => {


    return (
        <>
            <div className={NavBarCSS.container}>
                <div
                    style={{
                        display: "flex",
                        alignItems: "center",
                        gap: "10px",
                        width: "10%",
                    }}
                >
                    <Assistant fontSize="large" sx={{ color: "white" }} />
                    <p style={{ fontSize: "2rem", color: "white" }}>ChatBot</p>
                </div>
                <div
                    style={{
                        width: "90%",
                        display: "flex",
                        justifyContent: "center",
                    }}
                >
                    <select
                        id="models"
                        name="models"
                        className={NavBarCSS.selectModel}
                        value={model}
                        onChange={(e) => setModel(e.target.value)}
                        style={{
                            width: "300px",
                            height: "35px",
                            background: "black",
                            color: "#B4B4B4",
                            border: "none",
                            fontSize: "16px",
                            fontFamily: "Roboto, sans-serif",
                            fontWeight: "bold",
                        }}
                    >
                        <option value="gpt-4o-mini">OpenAI 4o Mini</option>
                        <option value="chatgpt-4o-latest">OpenAI 4o</option>
                        <option value="claude-3-5-sonnet-latest">Claude Sonnet 3.5</option>
                        <option value="deepseek_r1">DeepSeek R1</option>
                    </select>
                </div>
            </div>
        </>
    );
}

export default NavBar;