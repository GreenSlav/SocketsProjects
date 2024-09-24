import classes from "./MessageInput.module.css";

export default function MessageInput( {value, onChange, onEnter }) {
    const handleKeyDown = (e) => {
        if (e.key === 'Enter') {
            e.preventDefault(); // Предотвращаем переход на новую строку
            onEnter(); // Вызываем функцию, переданную через props
        }
    };

    return (
        <div className={classes.container}>
            <textarea
                className={classes.input}
                placeholder="Enter Message..."
                value={value}
                onChange={onChange}
                rows="4"
                onKeyDown={handleKeyDown}// Устанавливаем количество видимых строк
            />
        </div>
    )
}