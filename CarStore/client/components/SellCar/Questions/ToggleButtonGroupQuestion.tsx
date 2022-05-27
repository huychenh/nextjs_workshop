import { ToggleButtonGroup, ToggleButton, FormHelperText } from "@mui/material";
import { useEffect } from 'react';

const ToggleButtonGroupQuestion = (props: any) => {
    const { step, fieldData, setFieldData, car, error } = props;

    useEffect(() => {
        const data = car ? "true" : "false";
        setFieldData(data);
    }, [step])

    function handleChange(_event: any, value: any) {       
        setFieldData(value);
    }

    return <>
        <ToggleButtonGroup
            color="primary"
            value={fieldData}
            exclusive
            onChange={handleChange}
            fullWidth
        >
            <ToggleButton value="true">YES</ToggleButton>
            <ToggleButton value="false">NO</ToggleButton>
        </ToggleButtonGroup>
        <FormHelperText error={error.error}>{error.helperText}</FormHelperText>
    </>
};

export default ToggleButtonGroupQuestion;