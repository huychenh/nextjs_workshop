import * as React from "react";
import Checkbox from "@mui/material/Checkbox";
import TextField from "@mui/material/TextField";
import Autocomplete from "@mui/material/Autocomplete";
import CheckBoxOutlineBlankIcon from "@mui/icons-material/CheckBoxOutlineBlank";
import CheckBoxIcon from "@mui/icons-material/CheckBox";

const icon = <CheckBoxOutlineBlankIcon fontSize="small" />;
const checkedIcon = <CheckBoxIcon fontSize="small" />;

export default function CheckboxesTags(props: any) {
  const { setListCategories, defaultCategories, setDefaultCategories } = props;
  const onCheckedList = (value: any) => {
    var result = value.join(", ");
    setListCategories(result);
    setDefaultCategories(value);
  };
  return (
    <Autocomplete
      multiple
      id="checkboxes-tags-demo"
      options={categories}
      onChange={(event, value) => onCheckedList(value)}
      defaultValue={defaultCategories}
      disableCloseOnSelect
      value={defaultCategories}
      getOptionLabel={(option) => option.title}
      renderOption={(props, option, { selected }) => (
        <li {...props}>
          <Checkbox
            icon={icon}
            checkedIcon={checkedIcon}
            style={{ marginRight: 8 }}
            checked={selected}
          />
          {option.title}
        </li>
      )}
      style={{ width: 500 }}
      renderInput={(params) => (
        <TextField
          {...params}
          placeholder="typing category car ..."
          onFocus={(e) => (e.target.placeholder = "")}
          // onBlur={(e) => (e.target.placeholder = "Name")}
        />
      )}
    />
  );
}

const categories = [
  {
    title: "Sedan",
    toString: function () {
      return this.title;
    },
  },
  {
    title: "Coupe",
    toString: function () {
      return this.title;
    },
  },
  {
    title: "Sport",
    toString: function () {
      return this.title;
    },
  },
  {
    title: "Station wagon",
    toString: function () {
      return this.title;
    },
  },
  {
    title: "Hatchback",
    toString: function () {
      return this.title;
    },
  },
  {
    title: "Convertible",
    toString: function () {
      return this.title;
    },
  },
  {
    title: "SUV",
    toString: function () {
      return this.title;
    },
  },
  {
    title: "Pickup Truck",
    toString: function () {
      return this.title;
    },
  },
  {
    title: "Minivan",
    toString: function () {
      return this.title;
    },
  },
];
