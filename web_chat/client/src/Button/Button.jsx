import classes from "./Button.module.css";

export default function Button( {children, onClick} ) {
    return (
        <div className={classes.container}>
            <button className={classes.button} onClick={onClick}>{children}</button>
        </div>
    )
}