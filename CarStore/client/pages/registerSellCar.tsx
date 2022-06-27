import type { NextPage } from 'next';
import Layout from '../layouts/Layout';
import { useState } from 'react';
import ProductService from '../services/ProductService';
import SellCar from '../components/SellCar';
import { Stack, Button } from "@mui/material";
import styles from "../components/SellCar/SellCar.module.css";
import ToastMessage from "../components/ToastMessage";
import { useSession, getSession, signIn } from "next-auth/react"
import Router from 'next/router'

export const enum SessionStatus {
  LOADING = "loading",
  AUTHENTICATED = "authenticated",
  UNAUTHENTICATED = "unauthenticated",
}
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
  const { data: session, status }: any = useSession();
  if(status == SessionStatus.UNAUTHENTICATED)
  {
    signIn("identity-server4");
  }
 
  const [step, setStep] = useState(0);
  const [toast, setToast] = useState({
    open: false,
    severity: "error",
    message: ""
  });
  const [carInfo, setCarInfo] = useState<SellData>({
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
    category: "M1",
    color: "Red",
    description: "",
    hasInstallment: false,
    ownerId: "3fa85f64-5717-4562-b3fc-2c963f66afe8"
  });

  const lastStep = Object.keys(carInfo).length - 1;

  const updateCarInfo = (key: string, value: any) => {
    setCarInfo({
      ...carInfo,
      [key]: value
    });
  };

  async function addProducts(sellData: SellData) {
    try {
      const result = await ProductService.addProducts(session.accessToken, sellData);
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
      {step < lastStep && (<SellCar setStep={setStep} step={step} carInfo={carInfo} updateCarInfo={updateCarInfo} />)}
      {step == lastStep && (
        <div>
          <div className={styles.confirm}>
            <p>Car Name: {carInfo.name}</p>
            <p>Price: {carInfo.price}</p>
            <p>Brand: {carInfo.brand}</p>
            <p>Model: {carInfo.model}</p>
            <p>Transmission: {carInfo.transmission}</p>
            <p>MadeIn: {carInfo.madeIn}</p>
            <p>SeatingCapacity: {carInfo.seatingCapacity}</p>
            <p>kmDriven: {carInfo.kmDriven}</p>
            <p>Year: {carInfo.year}</p>
            <p>FuelType: {carInfo.fuelType}</p>
            <p>Category: {carInfo.category}</p>
            <p>Color: {carInfo.color}</p>
            <p>Description: {carInfo.description}</p>
            <p>HasInstallment: {carInfo.hasInstallment ? "Yes" : "No"}</p>
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
              onClick={async () => { await addProducts(carInfo); }}
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

// export async function getServerSideProps(context: any) {
//   return {
//     props: {
//       session: await getSession(context),
//     },
//   }
// }

export default RegisterSellCar
