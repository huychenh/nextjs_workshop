import React from "react";
import DialogContent from "@mui/material/DialogContent";
import Typography from "@mui/material/Typography";
import CheckboxesTags from "../../UI/DropDownMultiChoice";
import MinimumDistanceSlider from "../../UI/Slider";
import { AccessTime, LocalOffer } from "@material-ui/icons";
import styles from "./FilterResult.module.css";
import RadioButtonsGroup from "../../UI/Radio";
import Modal from "..";
import { ModalProps, SearchModel, modelSearchDefault } from "../Model";
const FilterSearch = (props: ModalProps) => {
  const { flag, searchBox, title, handleOnOff, onSearchFilter } = props;
  const [value2, setValue2] = React.useState<number[]>([5000, 50000]);
  const [lstCategories, setListCategories] = React.useState("");
  const [defaultCategories, setDefaultCategories] = React.useState([]);
  const [valueLastedNew, setValueLastedNew] = React.useState("");
  const [valueLowestPrice, setValueLowestPrice] = React.useState("");
  const handleClear = () => {
    setValue2([5000, 50000]);
    setDefaultCategories([]);
    setListCategories("");
    setValueLastedNew("");
    setValueLowestPrice("");
    onSearchFilter?.({ ...modelSearchDefault });
  };
  const sliderProps = {
    value2,
    setValue2,
  };
  const dropdownProps = {
    lstCategories,
    setListCategories,
    defaultCategories,
    setDefaultCategories,
  };
  const radioLastedNewProps = {
    valueLastedNew,
    setValueLastedNew,
  };
  const radioLowestPriceProps = {
    valueLastedNew: valueLowestPrice,
    setValueLastedNew: setValueLowestPrice,
  };
  const txtSearch: string = searchBox || "";
  const searchFollowFilter = () => {
    const modelFilter: SearchModel = {
      SearchText: txtSearch,
      CategoryName: lstCategories,
      PriceFrom: value2[0],
      PriceTo: value2[1],
      LatestNews: valueLastedNew ? true : false,
      LowestPrice: valueLowestPrice ? true : false,
      pageIndex: 0,
    };
    onSearchFilter?.(modelFilter);
    handleOnOff();
  };
  const modalProps: ModalProps = {
    flag,
    title,
    handleOnOff,
    handleClear,
    handleSearch: searchFollowFilter,
  };
  return (
    <Modal {...modalProps}>
      <DialogContent style={{ height: 380 }} dividers>
        <Typography gutterBottom>
          <b>Category</b>
        </Typography>
        <Typography gutterBottom>
          <CheckboxesTags {...dropdownProps} />
        </Typography>
        <br />
        <Typography gutterBottom>
          <b>
            Price: From {value2[0]} To {value2[1]}
          </b>
        </Typography>
        <Typography gutterBottom>
          <MinimumDistanceSlider {...sliderProps} />
        </Typography>
        <br />
        <Typography gutterBottom>
          <b>Order by</b>
        </Typography>
        <Typography gutterBottom>
          <div className={styles.divCss}>
            <span className={styles.icon}>
              <AccessTime />
              &nbsp;&nbsp;Latest news
            </span>
            <span>
              <RadioButtonsGroup {...radioLastedNewProps} />
            </span>
          </div>
          <div className={styles.divCss}>
            <span className={styles.icon}>
              <LocalOffer />
              &nbsp;&nbsp;Lowest price
            </span>
            <span>
              <RadioButtonsGroup {...radioLowestPriceProps} />
            </span>
          </div>
        </Typography>
      </DialogContent>
    </Modal>
  );
};
export default FilterSearch;
