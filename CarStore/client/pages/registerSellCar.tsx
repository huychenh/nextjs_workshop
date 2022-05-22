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
    year: 0,
    fuelType: "Petrol",
    category: "",
    color: "Red",
    description: "",
    hasInstallment: false,
    ownerId: "ownerId"
  });

  const lastStep = Object.keys(sellData).length - 1;
  const updateData = (key: any, value: any) => {
    setSellData({
      ...sellData,
      [key]: value
    });
  };

  async function addProducts(sellData: SellData) {
    try {
      await ProductService.addProducts(sellData);
      setToast({
        open: true,
        severity: "success",
        message: "Register sell car success!"
      });
      Router.push('/')
    }
    catch
    {
      setToast({
        open: true,
        severity: "error",
        message: "Register sell car fail!"
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
            <p>seatingCapacity: {sellData.seatingCapacity}</p>
            <p>kmDriven: {sellData.kmDriven}</p>
            <p>year: {sellData.year}</p>
            <p>fuelType: {sellData.fuelType}</p>
            <p>category: {sellData.category}</p>
            <p>color: {sellData.madeIn}</p>
            <p>description: {sellData.description}</p>
            <p>hasInstallment: {sellData.hasInstallment}</p>
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
