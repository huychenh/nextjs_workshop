import { MenuItem, Select, Typography, FormHelperText } from "@mui/material";
import { useEffect } from 'react';
import car from '../car-models.json'

const DropDownListQuestion = (props: any) => {
    const { step, fieldData, setFieldData, carInfo, question, error } = props;

    if (question.key == "model") {
        const data = car.find(x => x.brand == carInfo["brand"])?.models;
        if (data) {
            question.data = data;
        }
    }

    useEffect(() => {
        setFieldData(carInfo[question.key]);
    }, [step])

    const handleChange = (evt: any) => {
        setFieldData(evt.target.value);
    }

    return <><Select
        error={error.error}
        required
        value={fieldData}
        onChange={handleChange}
        fullWidth
    >

        {question.data?.map((item: any) => (
            <MenuItem key={item} value={item}>
                <Typography>{item}</Typography>
            </MenuItem>
        ))}
    </Select>
        <FormHelperText error={error.error}>{error.helperText}</FormHelperText></>
};

export default DropDownListQuestion;