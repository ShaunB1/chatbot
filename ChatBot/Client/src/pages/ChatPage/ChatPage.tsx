import {Send} from "@mui/icons-material";
import {IconButton} from "@mui/material";
import {useState} from "react";
import ReactMarkdown from "react-markdown";
import remarkGfm from "remark-gfm";
import rehypeHighlight from "rehype-highlight";
import "highlight.js/styles/github-dark.css";

interface Message {
    role: string;
    message: string;
}

const ChatPage = () => {
    const url = "http://localhost:5292/api/ai/chat"
    const [messages, setMessages] = useState<Message[]>([
        { role: "user", message: "### Markdown example." },
        { role: "system", message: "### Markdown example" },
    ]);
    const [input, setInput] = useState<string>("");

    const handleEnterKey = async (event: React.KeyboardEvent<HTMLTextAreaElement>) => {
        if (event.key === "Enter") {
            if (event.shiftKey) {
                return;
            } else {
                event.preventDefault();
                await getAiResponse(input);
            }
        }
    }

    const getAiResponse = async (prompt: string) => {
        try {
            const promptObject = {
                role: "user",
                message: prompt,
            }

            setInput("");
            setMessages(prevMessages => [...prevMessages, promptObject]);

            const response = await fetch(url, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({ prompt: prompt })
            });

            const data = await response.text();
            const responseObject = {
                role: "system",
                message: data
            }

            setMessages(prevMessages => [...prevMessages, responseObject]);
        } catch (e) {
            console.error(e);
        }
    }

    return (
        <>
            <div
                style={{
                    display: "flex",
                    flexDirection: "column",
                    width: "85%",
                    gap: "10px",
                }}
            >
                <div
                    style={{
                        height: "73%",
                        background: "black",
                        color: "white",
                        padding: "20px",
                        display: "flex",
                        flexDirection: "column",
                        gap: "12px",
                        fontFamily: "Roboto",
                        fontSize: "16px",
                        overflow: "auto"
                    }}
                >
                    {messages.map((message, index) => (
                        message.role === "system" ? (
                            <div
                                key={index}
                                style={{
                                    width: "98.5%",
                                    padding: "10px",
                                    borderRadius: "5px",
                                }}
                            >
                                <div style={{ whiteSpace: "pre-wrap", fontFamily: "monospace", fontSize: "16px" }}>
                                    <ReactMarkdown
                                        remarkPlugins={[remarkGfm]}
                                        rehypePlugins={[rehypeHighlight]}
                                    >
                                        {message.message}
                                    </ReactMarkdown>
                                </div>
                            </div>
                        ) : (
                            <div
                                key={index}
                                style={{
                                    width: "100%",
                                    display: "flex",
                                    justifyContent: "flex-end",
                                }}
                            >
                                <div
                                    style={{
                                        width: "400px",
                                        background: "gray",
                                        padding: "10px",
                                        borderRadius: "10px",
                                    }}
                                >
                                    <div style={{ whiteSpace: "pre-wrap", fontFamily: "monospace", fontSize: "16px" }}>
                                        <ReactMarkdown
                                            remarkPlugins={[remarkGfm]}
                                            rehypePlugins={[rehypeHighlight]}
                                        >
                                            {message.message}
                                        </ReactMarkdown>
                                    </div>
                                </div>
                            </div>
                        )
                    ))}
                </div>
                <div
                    style={{
                        width: "100%",
                        background: "white",
                        display: "flex",
                        alignItems: "center",
                        gap: "10px",
                        borderRadius: "10px",
                        padding: "20px 20px",
                    }}
                >
                    <textarea
                        wrap="soft"
                        onChange={(e) => setInput(e.target.value)}
                        onKeyDown={handleEnterKey}
                        value={input}
                        style={{
                            all: "unset",
                            width: "100%",
                            height: "100px",
                            overflowY: "auto",
                            fontFamily: "Roboto",
                            fontSize: "16px",
                        }}
                    />
                    <IconButton onClick={() => getAiResponse(input)}>
                        <Send fontSize="small" />
                    </IconButton>
                </div>
            </div>
        </>
    );
}

export default ChatPage;