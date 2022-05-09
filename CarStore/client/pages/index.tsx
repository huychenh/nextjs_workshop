import type { NextPage } from 'next';
import Layout from '../layouts/Layout';
import CarList from './HomePage/CarList/index';

const Home: NextPage = () => {
  return (
    <Layout>
      <CarList />
    </Layout>
  )
}

export default Home
