import {useEffect, useState} from "react";
import ReactMarkdown from "react-markdown";
import remarkGfm from "remark-gfm";
import rehypeHighlight from "rehype-highlight";
import "highlight.js/styles/github-dark.css";
import ChatPageCSS from "./ChatPage.module.css";

interface Message {
    role: string;
    message: string;
}

interface Chat {
    id: string;
    message: string;
    role: string;
}

const ChatPage = () => {
    const url = "http://localhost:5292/api/ai/chat";
    const dbUrl = "http://localhost:5292/api/db/history";
    const [messages, setMessages] = useState<Message[]>([]);
    const [input, setInput] = useState<string>("");

    useEffect(() => {
        fetch(dbUrl)
            .then((response) => response.json())
            .then((data: Chat[]) => {
                const temp = [...messages];
                data.forEach((chat: Chat) => {
                    const msgObj = {
                        role: chat.role,
                        message: chat.message,
                    }
                    temp.push(msgObj);
                });

                setMessages(temp);
            })
            .catch((error) => console.error("Error fetching chat history: ", error))
    }, []);

    const handleEnterKey = async (event: React.KeyboardEvent<HTMLTextAreaElement>) => {
        try {
            if (event.key === "Enter") {
                if (event.shiftKey) {
                    return;
                } else {
                    event.preventDefault();
                    await getAiResponse(input);
                }
            }
        } catch (error) {
            console.error(error);
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
            <div className={ChatPageCSS.rootContainer}>
                <div className={ChatPageCSS.mainContainer}>
                    {messages.map((message, index) => (
                        message.role === "system" ? (
                            <div
                                key={index}
                            >
                                <div>
                                    <ReactMarkdown
                                        remarkPlugins={[remarkGfm]}
                                        rehypePlugins={[rehypeHighlight]}
                                    >
                                        {message.message}
                                    </ReactMarkdown>
                                </div>
                            </div>
                        ) : (
                            <div key={index} className={ChatPageCSS.userMessageContainer}>
                                <div className={ChatPageCSS.userContainer}>
                                    <div style={{ whiteSpace: "pre-wrap" }}>
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
                <div className={ChatPageCSS.promptBarContainer}>
                    <textarea
                        wrap="soft"
                        className={ChatPageCSS.promptBar}
                        placeholder="Message AI Model"
                        onChange={(e) => setInput(e.target.value)}
                        onKeyDown={handleEnterKey}
                        value={input}
                    />
                </div>
            </div>
        </>
    );
}

export default ChatPage;