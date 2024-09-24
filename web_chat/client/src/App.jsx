import "./App.css"
import ChatScroll from "./ChatScroll/ChatScroll.jsx";
import Message from "./Message/Message.jsx";
import MessageInput from "./MessageInput/MessageInput.jsx";
import Button from "./Button/Button.jsx";

import { useState, useEffect, useRef } from 'react';


export default function App() {
    const [messages, setMessages] = useState([]); // Список сообщений
    const [input, setInput] = useState("");       // Текст из инпута
    const ws = useRef(null);

    // Подключение к серверу при загрузке компонента
    useEffect(() => {
        ws.current = new WebSocket("ws://localhost:27221/chat"); // Укажи свой порт

        // Получение сообщений с сервера
        ws.current.onmessage = (event) => {
            setMessages((prev) => [...prev, event.data]); // Добавляем новое сообщение в список
        };

        // Закрытие сокета при демонтировании компонента
        return () => {
            ws.current.close();
        };
    }, []);

    // Функция для отправки сообщения
    const sendMessage = () => {
        if (ws.current && input) {
            ws.current.send(input);  // Отправляем сообщение на сервер
            setInput("");            // Очищаем поле ввода
        }
    };

    return (
        <div className="container">
            <div className="chat">
                <ChatScroll>
                    {messages.map((msg, index) => (
                        <Message key={index}>{msg}</Message>
                    ))}
                </ChatScroll>
                <MessageInput value={input} onChange={(e) => setInput(e.target.value)} onEnter={sendMessage}></MessageInput>
                <Button onClick={sendMessage}>Send</Button>
            </div>
        </div>
    )
}