import type { NextPage } from "next";
import Layout from "../layouts/Layout";
import CarList from "../components/CarList";
import { useEffect, useState } from "react";
import ProductService from "../services/ProductService";
import SearchBox from "../components/SearchBox";
import BrandSelector from "../components/Brand";
import BrandService from "../services/BrandService";
import Pagination from "@material-ui/lab/Pagination";
import { Models } from "../models/product";
import { SearchModel, modelSearchDefault } from "../components/Modal/Model";

const Home: NextPage = () => {
  const [brands, setBrands] = useState([]);
  const [cars, setCars] = useState<Models.Product[]>([]);
  const [page, setPage] = useState(1);
  const [totalPages, setTotalpages] = useState(0);
  const [searchText, setSearchText] = useState(modelSearchDefault);

  useEffect(() => {
    BrandService.getBrands().then(({ data: brands }) => {
      setBrands(brands);
    });
  }, []);

  useEffect(() => {
    ProductService.getProducts({ ...searchText, pageIndex: page }).then(
      (response) => {
        setCars(response.data.items);
        setTotalpages(response.data.totalPages);
      }
    );
  }, [searchText, page]);

  const handleSearchTextChanged = (model: SearchModel) => {
    setSearchText(model);
    setPage(1);
  };

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
        onChange={(e: any, value: number) => setPage(value)}
      />
      <br />
      <CarList cars={cars} />
      <Pagination
        count={totalPages}
        variant="outlined"
        color="primary"
        className="pagination"
        page={page}
        onChange={(e: any, value: number) => setPage(value)}
      />
      <br />
    </Layout>
  );
};

export default Home;
