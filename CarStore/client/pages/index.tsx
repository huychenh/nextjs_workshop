import type { NextPage } from 'next';
import Layout from '../layouts/Layout';
import CarList from '../components/CarList';
import { useState } from 'react';
import ProductService from '../services/ProductService';
import SearchBox from '../components/SearchBox';
import BrandSelector from '../components/Brand';
import BrandService from '../services/BrandService';

const Home: NextPage = (props: any) => {
  const { brands } = props;
  const [cars, setCars] = useState(props.cars || []);

  const handleSearch = (text: string) => {
    ProductService.getProducts(text)
      .then(response => {
        setCars(response.data);
      });
  }

  return (
    <Layout>
      <SearchBox onSearch={handleSearch} />
      <BrandSelector brands={brands} />
      <CarList cars={cars} />
    </Layout>
  )
}

export async function getStaticProps() {
  const { data: cars } = await ProductService.getProducts();
  const { data: brands } = await BrandService.getBrands();
  return {
    props: {
      cars,
      brands
    },
  }
}

export default Home