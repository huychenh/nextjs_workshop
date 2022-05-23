import { TextField, Autocomplete, MenuItem, Select, Stack, Button, ToggleButtonGroup, ToggleButton, Typography, FormHelperText } from "@mui/material";
import { DatePicker, LocalizationProvider } from '@mui/x-date-pickers';
import { AdapterDateFns } from "@mui/x-date-pickers/AdapterDateFns";
import styles from "./SellCar.module.css";
import { useState, useEffect } from 'react';
import brands from './car-brands.json'
import car from './car-models.json'
import countries from './countries.json'

const questionData = [
    { key: "name", type: "TextField", data: [], label: "Please enter name:" },
    { key: "price", type: "TextField", data: [], label: "Please enter price:" },
    { key: "brand", type: "DropDownList", data: brands, label: "Please choose brand:" },
    { key: "model", type: "DropDownList", data: [], label: "Please choose model:" },
    { key: "transmission", type: "DropDownList", data: ["Unknown", "Manual", "Automatic", "Automanual", "CVT"], label: "Please choose transmission:" },
    { key: "madeIn", type: "Autocomplete", data: countries.map(item => item.name), label: "The car made in:" },
    { key: "seatingCapacity", type: "DropDownList", data: [4, 7, 16, 29, 45], label: "Please choose seating capacity:" },
    { key: "kmDriven", type: "TextField", data: [], label: "Please enter kmDriven:" },
    { key: "year", type: "DatePicker", data: [], label: "Please choose manufacturing date:" },
    { key: "fuelType", type: "DropDownList", data: ["Petrol", "Diesel"], label: "Please choose fuelType:" },
    { key: "category", type: "DropDownList", data: ["M", "M1", "M2", "M3", "N", "N1", "N2", "N3"], label: "Please choose category:" },
    { key: "color", type: "DropDownList", data: ["Red", "Green", "Blue"], label: "Please choose color:" },
    { key: "description", type: "TextField", data: [], label: "Please enter description:" },
    { key: "hasInstallment", type: "ToggleButtonGroup", data: [], label: "Accept installment payment method:" }
]

const SellCar = (props: any) => {
    const { sellData, updateData, step, setStep } = props;
    const [error, setError] = useState({
        helperText: "",
        error: false
    });

    const question = questionData[step];
    const [fieldData, setFieldData] = useState<any>("");
    const [sellYear, setSellYear] = useState<Date | null>(new Date());

    if (question.key == "model") {
        const data = car.find(x => x.brand == sellData["brand"])?.models;
        if (data) {
            question.data = data;
        }
    }

    const handleNext = () => {
        const isContainValue = (question.data.length > 0) ? question.data.some(d => d == fieldData) : true;
        const isEmpty = (question.data.length > 0) ? question.data.some(d => d == fieldData) : true;

        if (isContainValue && fieldData !== "") {
            const data = () => {
                if (typeof sellData[question.key] === 'number') return Number(fieldData);
                if (fieldData === "true") return true;
                if (fieldData === "false") return false;
                return fieldData;
            }
            updateData(question.key, data());
            setError({ helperText: '', error: false });
            setStep(step + 1);
        } else {
            setError({ helperText: 'Field is required', error: true });
        }
    };
    const handlePrevious = () => {
        setError({ helperText: '', error: false });
        setStep(step - 1);
    };

    useEffect(() => {
        let data = sellData[question.key];
        if (typeof data == "boolean") {
            data = data ? "true" : "false";
        }
        setFieldData(data);
        setSellYear(new Date(data, 0));
    }, [step])

    const handleChange = (evt: any) => {
        setFieldData(evt.target.value);
    }

    function handleInputChange(_event: any, value: any) {
        setFieldData(value);
    }

    const handleDatePickerChange = (newValue: any) => {
        setSellYear(newValue);
        const date = new Date(newValue);
        setFieldData(date.getFullYear());
    }

    const component = () => {
        switch (question.type) {
            case "TextField":
                return <TextField
                    type={typeof sellData[question.key]}
                    helperText={error.helperText}
                    error={error.error}
                    required
                    value={fieldData}
                    onChange={handleChange}
                    fullWidth
                    className={styles.quantity}
                />
            case "DropDownList":
                return <><Select
                    error={error.error}
                    required
                    value={fieldData}
                    onChange={handleChange}
                    fullWidth
                >

                    {question.data?.map(item => (
                        <MenuItem key={item} value={item}>
                            <Typography>{item}</Typography>
                        </MenuItem>
                    ))}
                </Select>
                    <FormHelperText error={error.error}>{error.helperText}</FormHelperText></>
            case "DatePicker":
                return <div className={styles.sellCarPicker}>
                    <LocalizationProvider dateAdapter={AdapterDateFns} >
                        <DatePicker
                            views={['year']}
                            label="Year only"
                            value={sellYear}
                            onChange={handleDatePickerChange}
                            renderInput={(params) => <TextField helperText={error.helperText} error={error.error} required {...params} />}
                        />
                    </LocalizationProvider>
                </div>
            case "Autocomplete":
                return <Autocomplete
                    value={fieldData}
                    options={question.data}
                    getOptionLabel={(option) => option}
                    id="auto-complete"
                    autoComplete
                    includeInputInList
                    onChange={handleInputChange}
                    renderInput={(params) => (
                        <TextField helperText={error.helperText} error={error.error} {...params} variant="standard" />
                    )}
                />
            case "ToggleButtonGroup":
                return <>
                    <ToggleButtonGroup
                        color="primary"
                        value={fieldData}
                        exclusive
                        onChange={handleInputChange}
                        fullWidth
                    >
                        <ToggleButton value="true">YES</ToggleButton>
                        <ToggleButton value="false">NO</ToggleButton>
                    </ToggleButtonGroup>
                    <FormHelperText error={error.error}>{error.helperText}</FormHelperText>
                </>
            default:
                return null;
        }
    }
    return (
        <div className={styles.sellCarContainer}>
            <h3>{question.label}</h3>
            {component()}
            <Stack direction="row" spacing={2} className={styles.sellCarButton}>
                {step > 0 && (
                    <Button
                        variant="outlined"
                        onClick={handlePrevious}
                    >
                        Back
                    </Button>)}
                {(step >= 0) && (
                    <Button type="submit"
                        variant="outlined"
                        onClick={handleNext}
                    >
                        Next
                    </Button>)}

            </Stack>
        </div>
    );
};

export default SellCar;