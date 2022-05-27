import { useSession } from "next-auth/react";
import { useRouter } from "next/router";
import { useEffect, useState } from "react";
import CarDetail from "../../components/Car/Detail/CarDetail";
import Layout from "../../layouts/Layout";
import { Models } from "../../models/product";
import ProductService from "../../services/ProductService";

const Detail = () => {
  const { data: session } = useSession()
  const router = useRouter()
  
  const [detail, setDetail] = useState(new Models.Product())

  useEffect(() => {
    if (router.query?.id) {
      ProductService.getProductDetail(router.query.id).then(res => {
        setDetail(res.data)
      })
    }
  }, [router.query?.id]);

  const renderProtectedPage = () => {
    return <>
      <h1>Protected Page</h1>
      <p>You can view this page because you are signed in.</p>
    </>
  }

  const renderContentPage = () => {
    return <CarDetail detail={detail}></CarDetail>
  }
  
  return (
    <Layout>
      {session ? renderContentPage() : renderProtectedPage()}
    </Layout>
  )
}

export default Detail;