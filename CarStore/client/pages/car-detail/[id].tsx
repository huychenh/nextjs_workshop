import { useRouter } from "next/router";
import { useEffect, useState } from "react";
import CarDetail from "../../components/Car/Detail/CarDetail";
import Layout from "../../layouts/Layout";
import { Models } from "../../models/product";
import ProductService from "../../services/ProductService";

const Detail = () => {
  const router = useRouter()  
  const [detail, setDetail] = useState(new Models.Product())

  useEffect(() => {
    if (router.query?.id) {
      ProductService.getProductDetail(router.query.id).then(res => {
        setDetail(res.data)
      })
    }
  }, [router.query?.id]);
  
  return (
    <Layout>
      <CarDetail detail={detail}></CarDetail>
    </Layout>
  )
}

export default Detail;