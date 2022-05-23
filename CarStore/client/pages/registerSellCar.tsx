import type { NextPage } from 'next';
import Layout from '../layouts/Layout';
import { useState } from 'react';
import ProductService from '../services/ProductService';
import SellCar from '../components/SellCar';
import { Stack, Button } from "@mui/material";
import styles from "../components/SellCar/SellCar.module.css";
import ToastMessage from "../components/ToastMessage";
import { useSession, getSession } from "next-auth/react"
import Router from 'next/router'

interface SellData {
  name: string,
  price: number,
  brand: string,
  model: string,
  transmission: string,
  madeIn: string,
  seatingCapacity: number,
  kmDriven: number,
  year: number,
  fuelType: string,
  category: string,
  color: string,
  description: string,
  hasInstallment: boolean,
  ownerId: any
};

const RegisterSellCar: NextPage = (props: any) => {
  const { data: session } = useSession();

  if (!session) {
    return (
      <Layout>
        <h1>Protected Page</h1>
        <p>You can view this page because you are signed in.</p>
      </Layout>
    )
  }
  const [step, setStep] = useState(0);
  const [toast, setToast] = useState({
    open: false,
    severity: "error",
    message: ""
  });
  const [sellData, setSellData] = useState<SellData>({
    name: "",
    price: 0,
    brand: "Alfa Romeo",
    model: "",
    transmission: "Unknown",
    madeIn: "",
    seatingCapacity: 4,
    kmDriven: 0,
    year: 1900,
    fuelType: "Petrol",
    category: "",
    color: "Red",
    description: "",
    hasInstallment: false,
    ownerId: "3fa85f64-5717-4562-b3fc-2c963f66afe8"
  });

  const lastStep = Object.keys(sellData).length - 1;

  const updateData = (key: string, value: any) => {
    setSellData({
      ...sellData,
      [key]: value
    });
  };

  async function addProducts(sellData: SellData) {
    try {
      const result = await ProductService.addProducts(sellData);
      console.log(result)
      const { isError } = result;
      if (!isError) {
        setToast({
          open: true,
          severity: "success",
          message: "Registration success!"
        });
        Router.push('/')
      }
      else {
        throw (new Error("Something went wrong"));
      }
    }
    catch {
      setToast({
        open: true,
        severity: "error",
        message: "Registration failed!"
      })

    }
  }

  return (
    <Layout>
      {step < lastStep && (<SellCar setStep={setStep} step={step} sellData={sellData} updateData={updateData} />)}
      {step == lastStep && (
        <div>
          <div className={styles.confirm}>
            <p>Car Name: {sellData.name}</p>
            <p>Price: {sellData.price}</p>
            <p>Brand: {sellData.brand}</p>
            <p>Model: {sellData.model}</p>
            <p>Transmission: {sellData.transmission}</p>
            <p>MadeIn: {sellData.madeIn}</p>
            <p>SeatingCapacity: {sellData.seatingCapacity}</p>
            <p>kmDriven: {sellData.kmDriven}</p>
            <p>Year: {sellData.year}</p>
            <p>FuelType: {sellData.fuelType}</p>
            <p>Category: {sellData.category}</p>
            <p>Color: {sellData.color}</p>
            <p>Description: {sellData.description}</p>
            <p>HasInstallment: {sellData.hasInstallment ? "Yes" : "No"}</p>
          </div>
          < Stack direction="row" spacing={2} justifyContent="center">
            <Button
              variant="outlined"
              onClick={() => { setStep(step - 1) }}
            >
              Back
            </Button>
            <Button
              variant="outlined"
              onClick={async () => { await addProducts(sellData); }}
            >
              Submit
            </Button>
          </Stack>
        </div>
      )}
      <ToastMessage toast={toast} setToast={setToast} />
    </Layout >
  )
}

export async function getServerSideProps(context: any) {
  return {
    props: {
      session: await getSession(context),
    },
  }
}

export default RegisterSellCar
