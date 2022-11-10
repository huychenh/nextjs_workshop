import React, { useState } from "react";
import styles from "./SearchBox.module.css";
import FilterSearch from "../Modal/FilterSearch";
import { ModalProps, SearchModel, modelSearchDefault } from "../Modal/Model";
const SearchBox = ({
  onSearch,
}: {
  onSearch: (model: SearchModel) => void;
}) => {
  const [text, setText] = useState("");
  const [open, setOpen] = React.useState(false);
  const handleClickOpen = () => {
    setOpen(!open);
  };

  const search = (e: any) => {
    e.preventDefault();
    if (typeof onSearch === "function") {
      onSearch({ ...modelSearchDefault, SearchText: text });
    }
  };
  const handleSearch = (model: SearchModel) => {
    onSearch(model);
  };
  const modalProps: ModalProps = {
    flag: open,
    searchBox: text,
    title: "Filter Result",
    handleOnOff: handleClickOpen,
    onSearchFilter: handleSearch,
  };
  return (
    <form onSubmit={search}>
      <div className={styles.searchContainer}>
        <div className={styles.textGroup}>
          <input
            type="text"
            title="Search"
            value={text}
            onChange={(e) => setText(e.target.value)}
            placeholder="Search by name, brand, model, or year"
            className={styles.searchInput}
          />
          <img src="/ic-search-home.webp" height={24} />
        </div>

        <div className={styles.filterGroup} onClick={handleClickOpen}>
          <input type="text" title="Filters" value="Filters" readOnly />
          <img src="/ic-filter.svg" height={24} />
        </div>
        <FilterSearch {...modalProps} />
        <div>
          <input type="submit" value="Search" className={styles.submitBtn} />
        </div>
      </div>
    </form>
  );
};

export default SearchBox;
