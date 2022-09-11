import { ClassNames } from "@emotion/react";
import {
  AppBar,
  Button,
  Container,
  Menu,
  MenuItem,
  Toolbar,
  Typography,
} from "@mui/material";
import Head from "next/head";
import Link from "next/link";
import { useEffect, useState } from "react";
import useStyles from "./LayoutStyles";
import { useSession, signIn, signOut } from "next-auth/react";
import { useRouter } from "next/router";
import ProductService from "../services/ProductService";
import styles from "./customStyles.module.css";
import "slick-carousel/slick/slick.css";
import "slick-carousel/slick/slick-theme.css";
export const enum SessionStatus {
  LOADING = "loading",
  AUTHENTICATED = "authenticated",
  UNAUTHENTICATED = "unauthenticated",
}

function Layout({ children, title }: any) {
  const { data: session, status } = useSession();

  // useEffect(() => {
  //   ProductService.getProducts().then(res => console.log(res))
  // })

  return (
    <>
      <Head>
        <title>{title ?? "Cars"}</title>
      </Head>
      <div className={styles.root}>
        <AppBar position="static" sx={useStyles.navbar}>
          <Container>
            <Toolbar>
              <Typography variant="h5" sx={useStyles.title}>
                <Link href="/"><a>Car Store</a></Link>
              </Typography>
              <div className={styles.menu}>
                <Link href="/registerSellCar" passHref>
                  <Button color="inherit">Sell your car</Button>
                </Link>
              </div>
              <div className={styles.loginButton}>
                {status === SessionStatus.AUTHENTICATED && (
                  <>{`${session?.user?.email}`}</>
                )}
                <Button
                  variant="outlined"
                  color="secondary"
                  onClick={() => {
                    if (status === SessionStatus.UNAUTHENTICATED) {
                      signIn("identity-server4");
                    } else if (status === SessionStatus.AUTHENTICATED) {
                      signOut();
                    }
                  }}
                >
                  {status === SessionStatus.LOADING && <>loading</>}
                  {status === SessionStatus.AUTHENTICATED && <>logout</>}
                  {status === SessionStatus.UNAUTHENTICATED && <>login</>}
                </Button>
              </div>
            </Toolbar>
          </Container>
        </AppBar>
      </div>
      <Container sx={useStyles.main}>{children}</Container>
      <footer>
        <Typography sx={useStyles.footer}>
          All rights reserved. NextJS Car
        </Typography>
      </footer>
    </>
  );
}

export default Layout;
