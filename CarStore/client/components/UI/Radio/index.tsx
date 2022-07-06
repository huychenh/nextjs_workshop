import * as React from "react";
import Radio from "@mui/material/Radio";
import RadioGroup from "@mui/material/RadioGroup";
import FormControlLabel from "@mui/material/FormControlLabel";
import FormControl from "@mui/material/FormControl";
export default function RadioButtonsGroup(props: any) {
  const { valueLastedNew, setValueLastedNew } = props;

  function handleClick(event: React.ChangeEvent<HTMLInputElement>) {
    if (event.target.value === valueLastedNew) {
      setValueLastedNew("");
    } else {
      setValueLastedNew(event.target.value);
    }
  }
  return (
    <FormControl>
      <RadioGroup value={valueLastedNew}>
        <FormControlLabel
          style={{ marginRight: 0 }}
          value="123"
          control={<Radio onClick={handleClick} />}
        />
      </RadioGroup>
    </FormControl>
  );
}
