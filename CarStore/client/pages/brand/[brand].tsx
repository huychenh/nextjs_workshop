import { useEffect, useState } from "react";

import styles from "./brand-page.module.css";

import CarList from "../../components/CarList";
import SearchBox from "../../components/SearchBox";
import Layout from "../../layouts/Layout";
import ProductService from "../../services/ProductService";
import { BrandIcons } from "../../components/Brand";
import { useRouter } from "next/router";

export default function BrandPage() {
    const router = useRouter()
    const { brand } = router.query;
    const [cars, setCars] = useState([]);
    const handleSearch = (text: string) => {
        ProductService.getProducts(text, brand as string)
            .then(response => {
                setCars(response.data);
            });
    }

    useEffect(() => {
        handleSearch("");
    }, [])

    const { title, logoPath } = brand ? BrandIcons[brand as string] : { title: '', logoPath: '' };

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