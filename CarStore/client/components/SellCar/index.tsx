import { Stack, Button } from "@mui/material";
import styles from "./SellCar.module.css";
import { useState } from 'react';
import brands from './car-brands.json'
import countries from './countries.json'

import AutocompleteQuestion from "./Questions/AutocompleteQuestion"
import DatePickerQuestion from "./Questions/DatePickerQuestion"
import DropDownListQuestion from "./Questions/DropDownListQuestion"
import TextFieldQuestion from "./Questions/TextFieldQuestion"
import ToggleButtonGroupQuestion from "./Questions/ToggleButtonGroupQuestion"

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

const ConvertType = (data: any, fieldData: any) => {
    if (typeof data === 'number') return Number(fieldData);
    if (typeof data === 'boolean') return (fieldData === 'true')
    return fieldData;
}

const SellCar = (props: any) => {
    const { carInfo, updateCarInfo, step, setStep } = props;
    const [error, setError] = useState({
        helperText: "",
        error: false
    });

    const question = questionData[step];
    const car = carInfo[question.key];
    const [fieldData, setFieldData] = useState<any>("");

    const handleNext = () => {
        const isContainValue = (question.data.length > 0) ? question.data.some(d => d == fieldData) : true;

        if (isContainValue && fieldData !== "") {
            const data = ConvertType(car, fieldData);
            updateCarInfo(question.key, data);
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

    const component = () => {
        switch (question.type) {
            case "TextField":
                return <TextFieldQuestion
                    step={step}
                    fieldData={fieldData}
                    setFieldData={setFieldData}
                    car={car}
                    error={error} />

            case "DropDownList":
                return <DropDownListQuestion
                    step={step}
                    fieldData={fieldData}
                    setFieldData={setFieldData}
                    carInfo={carInfo}
                    question={question}
                    error={error} />

            case "DatePicker":
                return <DatePickerQuestion
                    step={step}
                    setFieldData={setFieldData}
                    car={car}
                    error={error} />

            case "Autocomplete":
                return <AutocompleteQuestion
                    step={step}
                    fieldData={fieldData}
                    setFieldData={setFieldData}
                    car={car}
                    data={question.data}
                    error={error} />

            case "ToggleButtonGroup":
                return <ToggleButtonGroupQuestion
                    step={step}
                    fieldData={fieldData}
                    setFieldData={setFieldData}
                    car={car}
                    error={error} />
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
                        variant="contained"
                        onClick={handleNext}
                    >
                        Next
                    </Button>)}

            </Stack>
        </div>
    );
};

export default SellCar;