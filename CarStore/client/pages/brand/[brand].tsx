import { useEffect, useState } from "react";

import styles from "./brand-page.module.css";

import CarList from "../../components/CarList";
import SearchBox from "../../components/SearchBox";
import Layout from "../../layouts/Layout";
import BrandService from "../../services/BrandService";
import ProductService from "../../services/ProductService";
import { BrandIcons } from "../../components/Brand";
import { modelSearchDefault, SearchModel } from "../../components/Modal/Model";
export default function BrandPage({ brand }: any) {
  const [cars, setCars] = useState([]);
  const handleSearch = (model: SearchModel) => {
    debugger;
    ProductService.getProducts({
      ...model,
      brand,
    }).then((response) => {
      setCars(response.data);
    });
  };
  useEffect(() => {
    handleSearch(modelSearchDefault);
  }, []);

  const { title, logoPath } = BrandIcons[brand];

  return (
    <Layout title={`${title} Cars`}>
      <div className={styles.header}>
        <div className={styles.brandLogo}>
          <img src={logoPath} className={styles.brandImage} />
        </div>
        <h2 className={styles.headerTitle}>{title} Cars</h2>
        <div className={styles.clearBoth}></div>
      </div>
      <SearchBox onSearch={handleSearch} />
      <CarList cars={cars} />
    </Layout>
  );
}

export async function getStaticPaths() {
  const { data: brands } = await BrandService.getBrands();

  const paths = brands.map((brand: any) => ({
    params: { brand: brand.name.toLowerCase() },
  }));

  return { paths, fallback: false };
}

export async function getStaticProps({ params }: any) {
  return {
    props: {
      brand: params.brand,
    },
  };
}
