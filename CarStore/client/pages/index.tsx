import type { NextPage } from 'next';
import Layout from '../layouts/Layout';
import CarList from '../components/CarList';
import { useState } from 'react';
import ProductService from '../services/ProductService';
import SearchBox from '../components/SearchBox';

const Home: NextPage = (props: any) => {
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
      <CarList cars={cars} />
    </Layout>
  )
}

export async function getStaticProps() {
  const { data } = await ProductService.getProducts();
  return {
    props: {
      cars: data,
    },
  }
}

export default Home
