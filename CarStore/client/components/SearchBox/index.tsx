import React, { useState } from "react";
import styles from "./SearchBox.module.css";

const SearchBox = ({ onSearch }: {onSearch: (text: string) => void}) => {
  const [text, setText] = useState("");

  const openFilterBox = () => {
    console.log("filters clicked!");
  }

  const search = (e: any) => {
    e.preventDefault();
    if (typeof onSearch === "function") {
      onSearch(text);
    }
  }

  return (
    <form onSubmit={search}>
      <div className={styles.searchContainer}>
        <div className={styles.textGroup}>
          <input
            type="text"
            title="Search"
            value={text}
            onChange={e => setText(e.target.value)}
            placeholder="Search by name, brand, model, or year"
            className={styles.searchInput}
          />
          <img src="/ic-search-home.webp" height={24} />
        </div>

        <div className={styles.filterGroup}>
          <input type="text" title="Filters" value="Filters" onClick={openFilterBox} readOnly />
          <img src="/ic-filter.svg" height={24} />
        </div>

        <div>
          <input type="submit" value="Search" className={styles.submitBtn} />
        </div>
      </div>
    </form>
  );
};

export default SearchBox;