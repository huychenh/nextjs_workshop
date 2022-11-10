import { TextField } from "@mui/material";
import { DatePicker, LocalizationProvider } from '@mui/x-date-pickers';
import { AdapterDateFns } from "@mui/x-date-pickers/AdapterDateFns";
import styles from "../SellCar.module.css";
import { useState, useEffect } from 'react';

const DatePickerQuestion = (props: any) => {
    const { step, setFieldData, car, error } = props;

    const [sellYear, setSellYear] = useState<Date | null>(new Date());

    useEffect(() => {
        const data = car;
        setFieldData(data);
        setSellYear(new Date(data, 0));
    }, [step])

    const handleChange = (newValue: any) => {
        setSellYear(newValue);
        const date = new Date(newValue);
        setFieldData(date.getFullYear());
    }

    return <div className={styles.sellCarPicker}>
        <LocalizationProvider dateAdapter={AdapterDateFns} >
            <DatePicker
                views={['year']}
                label="Year only"
                value={sellYear}
                onChange={handleChange}
                renderInput={(params) => <TextField fullWidth helperText={error.helperText} error={error.error} required {...params} />}
            />
        </LocalizationProvider>
    </div>
};

export default DatePickerQuestion;