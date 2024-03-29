import type { NextPage } from "next";
import Layout from "../layouts/Layout";
import CarList from "../components/CarList";
import { useState } from "react";
import ProductService from "../services/ProductService";
import SearchBox from "../components/SearchBox";
import BrandSelector from "../components/Brand";
import BrandService from "../services/BrandService";
import { Models } from "../models/product";
import { SearchModel, modelSearchDefault } from "../components/Modal/Model";
import { Pagination } from "@mui/material";

const Home: NextPage = (props: any) => {
  const { brands } = props;
  const [cars, setCars] = useState<Models.Product[]>(props.cars || []);
  const [page, setPage] = useState(1);
  const [totalPages, setTotalpages] = useState(props.totalPages || 0);
  const [searchModel, setSearchModel] = useState(modelSearchDefault);

  const getProducts = (searchModel: SearchModel, page: number) => {
    ProductService.getProducts({ ...searchModel, pageIndex: page }).then(
      (response) => {
        setCars(response.data.items);
        setTotalpages(response.data.totalPages);
      }
    );
  }

  const handleSearchTextChanged = (model: SearchModel) => {
    setSearchModel(model);
    const page = 1;
    setPage(page);
    getProducts(model, page);
  }

  const handlePageChanged = (e: any, value: number) => {
    setPage(value);
    getProducts(searchModel, value);
  }

  return (
    <Layout>
      <SearchBox onSearch={handleSearchTextChanged} />
      <BrandSelector brands={brands} />
      <Pagination
        count={totalPages}
        variant="outlined"
        color="primary"
        className="pagination"
        page={page}
        onChange={handlePageChanged}
      />
      <br />
      <CarList cars={cars} />
      <Pagination
        count={totalPages}
        variant="outlined"
        color="primary"
        className="pagination"
        page={page}
        onChange={handlePageChanged}
      />
      <br />
    </Layout>
  );
};

export async function getStaticProps() {
  const productRes = await ProductService.getProducts({ ...modelSearchDefault, pageIndex: 1 });
  console.log('productRes', productRes)
  const { data: brands } = await BrandService.getBrands();
  return {
    props: {
      cars: productRes.data.items,
      totalPages: productRes.data.totalPages,
      brands,
    },
    revalidate: 60,
  }
}

export default Home;
