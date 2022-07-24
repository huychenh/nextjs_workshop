import { Box, Button, Grid, TextField } from "@mui/material";
import { signIn, useSession } from "next-auth/react";
import { useRouter } from "next/router";
import { ChangeEvent, useEffect, useState } from "react";
import ToastMessage from "../../components/ToastMessage";
import Layout from "../../layouts/Layout";
import { Models } from "../../models/product";
import OrderService from "../../services/OrderService";
import ProductService from "../../services/ProductService";

export const enum SessionStatus {
  LOADING = "loading",
  AUTHENTICATED = "authenticated",
  UNAUTHENTICATED = "unauthenticated",
}

export class ErrorMessage {
  public fullName: string | undefined;
  public email: string | undefined;
  public phone: string | undefined;
  public address: string | undefined;
}

const Order = () => {
  const { data: session, status }: any = useSession();
  if (status == SessionStatus.UNAUTHENTICATED) {
    signIn("identity-server4");
  }

  const formatter = new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'USD',
  });

  const router = useRouter()
  const [toast, setToast] = useState({
    open: false,
    severity: "error",
    message: ""
  });
  const [detail, setDetail] = useState(new Models.Product())
  const [form, setForm] = useState({
    fullName: '',
    email: '',
    phone: '',
    address: '',
    note: ''
  })
  const [errors, setErrors] = useState({
    fullName: '',
    email: '',
    phone: '',
    address: ''
  })
  const [disabledButton, setDisabledButton] = useState(false)

  useEffect(() => {
    if (router.query?.id) {
      ProductService.getProductDetail(router.query.id).then(res => {
        setDetail(res.data)
      })
    }
  }, [router.query?.id]);

  const handleChangeInput = (e: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    setForm({ ...form, [e.target.id]: e.target.value })
  }

  const getFormErrors = () => {
    let formErrors = {
      fullName: '',
      email: '',
      phone: '',
      address: ''
    }
    if (form.fullName == '') {
      formErrors.fullName = 'Please fill the required field'
    }
    if (form.email == '') {
      formErrors.email = 'Please fill the required field'
    } else {
      const emailRegex = /^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$/
      if (!form.email.match(emailRegex)) {
        formErrors.email = "Email is invalid"
      }
    }
    if (form.phone == '') {
      formErrors.phone = 'Please fill the required field'
    } else {
      const phoneRegex = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/
      if (!form.phone.match(phoneRegex)) {
        formErrors.phone = "Phone is invalid"
      }
    }
    if (form.address == '') {
      formErrors.address = 'Please fill the required field'
    }

    return formErrors
  }

  const hasErrors = (errors: any) => {
    for (var key in errors) {
      if (errors[key])
        return true;
    }

    return false;
  }

  const submitOrder = async () => {
    const errors = getFormErrors()
    if (hasErrors(errors)) {
      setErrors(errors)
    }
    else {
      setDisabledButton(true)
      const request = {
        productId: detail.id,
        price: detail.price,
        productName: detail.name,
        ownerId: detail.ownerId,
        fullName: form.fullName,
        address: form.address,
        phone: form.phone,
        buyerEmail: form.email,
        note: form.note,
        pictureUrl: null, //Todo: add picture url
      }
      const res = await OrderService.createOrder(session.accessToken, request)
      if (!res.isError) {
        setToast({
          open: true,
          severity: "success",
          message: "Registration success!"
        });
        router.push('/')
      }
      else {
        setDisabledButton(false)
        setToast({
          open: true,
          severity: "error",
          message: "Something went wrong! Please try again"
        });
      }
    }
  }

  return (
    <Layout>
      <ToastMessage toast={toast} setToast={setToast} />
      <Grid container spacing={2} paddingBottom={5}>
        <Grid item xs={8}>
          <h2>Contact Info</h2>
          <Box sx={{ display: 'flex', flexWrap: 'wrap' }}
          >
            <div>
              <TextField
                sx={{ m: 1, width: '25ch' }}
                required
                id="fullName"
                label="Full Name"
                onChange={(e) => handleChangeInput(e)}
                error={errors.fullName !== ''}
                helperText={errors.fullName}
                value={form.fullName}
              />
              <TextField
                sx={{ m: 1, width: '25ch' }}
                required
                id="email"
                label="Email"
                type="email"
                onChange={(e) => handleChangeInput(e)}
                value={form.email}
                error={errors.email !== ''}
                helperText={errors.email}
              />
              <TextField
                sx={{ m: 1, width: '25ch' }}
                required
                id="phone"
                label="Phone"
                onChange={(e) => handleChangeInput(e)}
                value={form.phone}
                error={errors.phone !== ''}
                helperText={errors.phone}
              />
              <TextField
                sx={{ m: 1, width: '79ch' }}
                required
                id="address"
                label="Address"
                onChange={(e) => handleChangeInput(e)}
                value={form.address}
                error={errors.address !== ''}
                helperText={errors.address}
              />
              <h3>Additional Note</h3>
              <TextField
                sx={{ m: 1, width: '79ch' }}
                id="note"
                label="Note"
                multiline
                rows={4}
                onChange={(e) => handleChangeInput(e)}
                value={form.note}
              />
            </div>
          </Box>
        </Grid>
        <Grid item xs={4}>
          <h2>Your Order</h2>
          <img src="/HomePage/Car/ford.jpg" />
          <h3>{detail.name}</h3>
          <p>{formatter.format(detail.price!)}</p>
          <div>
            <Button variant="contained" color="success" disabled={disabledButton} onClick={submitOrder}>SUBMIT</Button>
          </div>
        </Grid>
      </Grid>
    </Layout>
  )
}

export default Order;