import styles from "./Brand.module.css";

import { Container } from '@mui/material';
import Link from 'next/link';
import { SwiperSlide } from "swiper/react";

import BrandSwiper from './BrandSwiper';


const BrandIconRootFolder = "/HomePage/Brand";

const BrandIcons: { [id: string]: { title: string, logoPath: string } } = {};
BrandIcons['audi'] = { title: "Audi", logoPath: `${BrandIconRootFolder}/audi-logo.png` };
BrandIcons['bentley'] = { title: "Bentley", logoPath: `${BrandIconRootFolder}/bentley-logo.png` };
BrandIcons['bmw'] = { title: "BMW", logoPath: `${BrandIconRootFolder}/bmw-logo.png` };
BrandIcons['ferrari'] = { title: "Ferrari", logoPath: `${BrandIconRootFolder}/ferrari-logo.png` };
BrandIcons['ford'] = { title: "Ford", logoPath: `${BrandIconRootFolder}/ford-logo.png` };
BrandIcons['honda'] = { title: "Honda", logoPath: `${BrandIconRootFolder}/honda-logo.png` };
BrandIcons['mazda'] = { title: "Mazda", logoPath: `${BrandIconRootFolder}/mazda-logo.png` };
BrandIcons['toyota'] = { title: "Toyota", logoPath: `${BrandIconRootFolder}/toyota-logo.png` };
BrandIcons['chevrolet'] = { title: "Chevrolet", logoPath: `${BrandIconRootFolder}/chevrolet.png` };
BrandIcons['default-brand'] = { title: "default-brand", logoPath: `${BrandIconRootFolder}/brand-logo.png` };

export { BrandIcons };

export default function BrandSelector({ brands }: any) {
    const swiperSlides = brands
        .sort((a: any, b: any) => a.name.localeCompare(b.name))
        .map((x: any) => {
            const brand = x.name.toLowerCase();
            const { logoPath, title } = BrandIcons[brand] ?? {
                logoPath: BrandIcons['default-brand'].logoPath,
                title: brand
            };
            return (
                <BrandIcon key={brand} src={logoPath}
                    brand={brand}
                    brandTitle={title}
                />
            );
        })
        .reduce((prev: any, current: any, index: any) => {
            if (prev.length === 0) prev[0] = [];
            prev[prev.length - 1].push(current);
            if (index !== 0 && index % 7 === 0) prev.push([]);
            return prev;
        }, [])
        .reduce((prev: any, current: any, index: any) => {
            prev.push(<SwiperSlide key={index}>{current}</SwiperSlide>);
            return prev;
        }, []);
    return (
        <>
            <h2>Looking for a specific brand</h2>
            <Container className={styles.brandContainer}>
                <BrandSwiper>
                    {swiperSlides}
                </BrandSwiper>
            </Container>
        </>
    )
}

const BrandIcon = ({ src, brand, brandTitle }: any) => {
    return (
        <div className={styles.brandLogo} title={brandTitle}>
            <Link href={`/brand/${brand.toLowerCase()}`}>
                <a>
                    <img src={src} className={styles.brandImage} alt={brandTitle} />
                </a>
            </Link>
        </div >
    )
}