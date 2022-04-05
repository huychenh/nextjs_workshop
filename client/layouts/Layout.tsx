import { ClassNames } from '@emotion/react'
import {
  AppBar,
  Button,
  Container,
  Menu,
  MenuItem,
  Toolbar,
  Typography,
} from '@mui/material'
import Head from 'next/head'
import Link from 'next/link'
import { useState } from 'react'
import useStyles from './LayoutStyles'

const Layout = ({ children }: any) => {
  const classes = useStyles()
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null)

  const handleClick = (event: React.MouseEvent<HTMLButtonElement>) => {
    setAnchorEl(event.currentTarget)
  }

  const handleClose = () => {
    setAnchorEl(null)
  }

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
                <Button color="inherit">Login</Button>
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
  )
}

export default Layout
