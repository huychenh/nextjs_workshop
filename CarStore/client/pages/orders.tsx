import Link from 'next/link';
import Layout from '../layouts/Layout';
import Card from "@mui/material/Card";
import CardContent from "@mui/material/CardContent";
import Typography from "@mui/material/Typography";
import Grid from "@mui/material/Grid";
import { useEffect, useState } from 'react';
import { signIn, useSession } from "next-auth/react"
import OrderService from '../services/OrderService';
import { Models } from '../models/order';
import { formatCurrency } from '../lib/formatCurrency';

export const enum SessionStatus {
  LOADING = "loading",
  AUTHENTICATED = "authenticated",
  UNAUTHENTICATED = "unauthenticated",
}

export default function Orders() {

  const { data: session, status }: any = useSession();
  const accessToken = session?.accessToken;
  if (status == SessionStatus.UNAUTHENTICATED) {
    signIn("identity-server4");
  }

  const [orders, setOrders] = useState<Models.Order[]>([]);

  const getOrders = () => {
    OrderService.getOrdersByCustomerId(session.accessToken)
      .then((res) => {
        setOrders(res.data);
        console.log(res.data);
      })
      .catch((error) => console.log(error));
  }

  useEffect(() => {
    if (accessToken) {
      getOrders();
    }
  }, [accessToken]);

  return (
    <Layout>
      {
        orders.length > 0 ?
          orders.map((item) => {
            const imageUrl = item.pictureUrl ?? '/HomePage/Car/car.png';
            return (
              <Card key={item.id} sx={{ width: 800, mt: 4, }} style={{ marginInline: 'auto', paddingBottom: 0 }}>
                <CardContent>
                  <Grid container spacing={2}>
                    {/* image car */}
                    <Grid xs={4}>
                      <Link href="#">
                        <img
                          src={imageUrl}
                          alt="xe"
                          style={{ width: "100%" }}
                        />
                      </Link>
                    </Grid>
                    {/* content car */}
                    <Grid xs={8} style={{ padding: '0 2rem' }}>
                      <Link href="#">
                        <Typography
                          component="div"
                          className="car-name"
                          sx={{ mb: 2, fontWeight: "bold", fontSize: '18px' }}
                        >
                          {item.productName}
                        </Typography>
                      </Link>
                      <Typography
                        sx={{ mb: 1.2, fontSize: '16px' }}
                        color="text.secondary"
                        className="car-date"
                      >
                        <strong>Order date:</strong> {new Date(item.date!).toLocaleDateString()}
                      </Typography>
                      <Typography
                        sx={{ mb: 1.2, fontSize: '16px' }}
                        color="text.secondary"
                        className="car-id"
                      >
                        <strong>Order Id:</strong> {item.id}
                      </Typography>
                      <Typography
                        sx={{ mb: 1.2, fontSize: '16px' }}
                        color="text.secondary"
                        gutterBottom
                        className="car-price"
                      >
                        <strong>Price: </strong> {formatCurrency(item.price!)}
                      </Typography>
                    </Grid>
                  </Grid>
                </CardContent>
              </Card>
            )
          }) : (<h1>Chua co order</h1>)
      }
    </Layout>
  );
}