import classes from "./Message.module.css"

export default function Message( {children} ) {
    return (
        <div className={classes.container}>
            <div className={classes.text}>
                {children}
            </div>
        </div>
    )
}