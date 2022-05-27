import { TextField } from "@mui/material";
import styles from "../SellCar.module.css";
import { useEffect } from 'react';

const TextFieldQuestion = (props: any) => {
    const { step, fieldData, setFieldData, car, error } = props;

    useEffect(() => {
        setFieldData(car);
    }, [step])

    const handleChange = (evt: any) => {
        setFieldData(evt.target.value);
    }

    return (
        <TextField
            type={typeof car}
            helperText={error.helperText}
            error={error.error}
            required
            value={fieldData}
            onChange={handleChange}
            fullWidth
            className={styles.quantity}
        />
    );
};

export default TextFieldQuestion;