import { TextField, Autocomplete, MenuItem, Select, Stack, Button, ToggleButtonGroup, ToggleButton } from "@mui/material";
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
    { key: "madeIn", type: "DropDownList", data: countries.map(item => item.name), label: "The car made in:" },
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
    const [sellYear, setSellYear] = useState<Date | null>(null);

    if (question.key == "model") {
        const data = car.find(x => x.brand == sellData["brand"])?.models;
        if (data) {
            question.data = data;
        }
    }

    const handleChange = (evt: any) => {
        setFieldData(evt.target.value);
    }

    const datePickerChange = (newValue: any) => {
        setSellYear(newValue);

        const date = new Date(newValue);
        setFieldData(date.getFullYear());
    }

    const handleNext = () => {
        if (fieldData != 0 || fieldData != "") {
            setError({ helperText: '', error: false });
            updateData(question.key, fieldData);
            setStep(step + 1);
        } else {
            setError({ helperText: 'Field is required', error: true });
        }
    };
    const handlePrevious = () => {
        setStep(step - 1);
    };

    useEffect(() => {
        setFieldData(sellData[question.key]);
    }, [step])

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
                return <Select
                    error={error.error}
                    required
                    value={fieldData}
                    onChange={handleChange}
                    fullWidth
                >
                    {question.data?.map(item => (
                        <MenuItem key={item} value={item}>{item}</MenuItem>
                    ))}
                </Select>
            case "DatePicker":
                return <div className={styles.sellCarPicker}>
                    <LocalizationProvider dateAdapter={AdapterDateFns} >
                        <DatePicker
                            views={['year']}
                            label="Year only"
                            value={sellYear}
                            onChange={datePickerChange}
                            renderInput={(params) => <TextField helperText={error.helperText} error={error.error} required {...params} />}
                        />
                    </LocalizationProvider>
                </div>
            case "Autocomplete":
                return <Autocomplete
                    freeSolo
                    value={fieldData}
                    onChange={handleChange}
                    disableClearable
                    options={question.data}
                    getOptionLabel={(item) => item || ""}
                    renderInput={(params) => (
                        <TextField
                            required
                            helperText={error.helperText}
                            error={error.error}
                            name="homeTown"
                            {...params}
                            InputProps={{
                                ...params.InputProps,
                                type: 'search',
                            }}
                        />
                    )}
                />
            case "ToggleButtonGroup":
                return <ToggleButtonGroup
                    color="primary"
                    value={fieldData}
                    exclusive
                    onChange={handleChange}
                    fullWidth
                >
                    <ToggleButton value="true">YES</ToggleButton>
                    <ToggleButton value="false">NO</ToggleButton>
                </ToggleButtonGroup>
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
                    <Button
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





