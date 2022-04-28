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

export const enum SessionStatus {
  LOADING = "loading",
  AUTHENTICATED = "authenticated",
  UNAUTHENTICATED = "unauthenticated",
}

function Layout({ children }: any) {
  const classes = useStyles();

  const { data: session, status } = useSession();

  return (
    <>
      <Head>
        <title>Cars</title>
      </Head>
      <div className={classes.root}>
        <AppBar position="static" className={classes.navbar}>
          <Container>
            <Toolbar>
              <Typography variant="h5" className={classes.title}>
                Car Store
              </Typography>
              <div className={classes.menu}>
                <Link href="#" passHref>
                  <Button color="inherit">Sell your car</Button>
                </Link>
              </div>
              <div className={classes.loginButton}>
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
      <Container className={classes.main}>{children}</Container>
      <footer className={classes.footer}>
        <Typography>All rights reserved. NextJS Car</Typography>
      </footer>
    </>
  );
}

export default Layout;
