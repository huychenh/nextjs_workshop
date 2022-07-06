import type { NextPage } from 'next';
import Layout from '../layouts/Layout';
import CarList from '../components/CarList';
import { useEffect, useState } from 'react';
import ProductService from '../services/ProductService';
import SearchBox from '../components/SearchBox';
import BrandSelector from '../components/Brand';
import BrandService from '../services/BrandService';
import Pagination from "@material-ui/lab/Pagination";
import { Models } from '../models/product';

const Home: NextPage = () => {
  const [brands, setBrands] = useState([]);
  const [cars, setCars] = useState<Models.Product[]>([]);
  const [page, setPage] = useState(1);
  const [searchText, setSearchText] = useState('');

  useEffect(() => {
    ProductService.getProducts().then(({ data: cars }) => {
      setCars(cars);
    });
    BrandService.getBrands().then(({ data: brands }) => {
      setBrands(brands);
    });
  }, []);

  useEffect(() => {
    ProductService.getProducts(searchText, undefined, page)
      .then(response => {
        setCars(response.data);
      });
  }, [searchText, page]);

  const handleSearchTextChanged = (text: string) => {
    setSearchText(text);
    setPage(1);
  }

  return (
    <Layout>
      <SearchBox onSearch={handleSearchTextChanged} />
      <BrandSelector brands={brands} />
      <Pagination
        count={cars && cars[0] ? cars[0].totalPages : 1}
        variant='outlined'
        color='primary'
        className='pagination'
        page={page}
        onChange={(e: any, value: number) => setPage(value)}
      />
      <br />
      <CarList cars={cars} />
      <Pagination
        count={cars && cars[0] ? cars[0].totalPages : 1}
        variant='outlined'
        color='primary'
        className='pagination'
        page={page}
        onChange={(e: any, value: number) => setPage(value)}
      />
      <br />
    </Layout>
  )
}

export default Home