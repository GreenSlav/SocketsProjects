import { useRef, useEffect } from 'react';
import classes from "./ChatScroll.module.css";

export default function ChatScroll({ children }) {
    const scrollRef = useRef(null); // Создаем реф для элемента с прокруткой

    useEffect(() => {
        // Прокручиваем вниз при каждом изменении детей (сообщений)
        if (scrollRef.current) {
            scrollRef.current.scrollTop = scrollRef.current.scrollHeight; // Прокрутка вниз
        }
    }, [children]); // Запускаем эффект при каждом обновлении children

    return (
        <div className={classes.container}>
            <div className={classes.overlay}></div>
            <div className={classes.scroll} ref={scrollRef}>
                {children}
            </div>
        </div>
    );
}
