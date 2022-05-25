import { TextField, Autocomplete } from "@mui/material";
import { useEffect } from 'react';

const AutocompleteQuestion = (props: any) => {
    const { step, fieldData, setFieldData, car, data, error } = props;

    useEffect(() => {
        setFieldData(car);
    }, [step])

    function handleChange(_event: any, value: any) {
        setFieldData(value);
    }

    return <Autocomplete
        value={fieldData}
        options={data}
        getOptionLabel={(option) => option}
        id="auto-complete"
        autoComplete
        includeInputInList
        onChange={handleChange}
        renderInput={(params) => (
            <TextField helperText={error.helperText} error={error.error} {...params} variant="standard" />
        )}
    />
};

export default AutocompleteQuestion;