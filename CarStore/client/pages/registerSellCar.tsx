import type { NextPage } from 'next';
import Layout from '../layouts/Layout';
import { useCallback, useEffect, useMemo, useState } from 'react';
import ProductService from '../services/ProductService';
import SellCar from '../components/SellCar';
import { Stack, Button } from "@mui/material";
import styles from "../components/SellCar/SellCar.module.css";
import ToastMessage from "../components/ToastMessage";
import { useSession, signIn } from "next-auth/react"
import Router from 'next/router'
import FileUploader from '../components/UI/FileUploader/FileUploader';
import FileStorageService from '../services/FileStorageService';
import { BlobServiceClient, ContainerClient } from '@azure/storage-blob';

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
  ownerId: any,
  images: string[],
};

const RegisterSellCar: NextPage = (props: any) => {
  const { data: session, status }: any = useSession();
  const accessToken = session?.accessToken;
  const sub = session?.user.sub;

  if (status == SessionStatus.UNAUTHENTICATED) {
    signIn("identity-server4");
  }

  const [step, setStep] = useState(0);
  const [toast, setToast] = useState({
    open: false,
    severity: "error",
    message: ""
  });
  const [carInfo, setCarInfo] = useState<SellData>({
    name: "abc",
    price: 0,
    brand: "Alfa Romeo",
    model: "",
    transmission: "Unknown",
    madeIn: "",
    seatingCapacity: 4,
    kmDriven: 0,
    year: 2020,
    fuelType: "Petrol",
    category: "M1",
    color: "Red",
    description: "test",
    hasInstallment: false,
    ownerId: sub,
    images: [],
  });
  const [files, setFiles] = useState<any[]>([]);

  const lastStep = Object.keys(carInfo).length - 2;

  const updateCarInfo = (key: string, value: any) => {
    setCarInfo({
      ...carInfo,
      [key]: value
    });
  };

  const handleSubmit = async () => {
    const imageNames = await uploadImages();
    carInfo.images = imageNames as string[];
    console.log('images', imageNames)
    await addProducts(carInfo);
  }

  const uploadImages = async () => {
    if (!files?.length) {
      return [];
    }
    return await FileStorageService.uploadImages(accessToken, files);
  }

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
          <h5>Review</h5>
          <div className={styles.confirm}>
            <div className={styles.infoItem}>
              <div>Car Name:</div>
              <div>{carInfo.name}</div>
            </div>
            <div className={styles.infoItem}>
              <div>Price:</div>
              <div>{carInfo.price}</div>
            </div>
            <div className={styles.infoItem}>
              <div>Brand:</div>
              <div>{carInfo.brand}</div>
            </div>
            <div className={styles.infoItem}>
              <div>Model:</div>
              <div>{carInfo.model}</div>
            </div>
            <div className={styles.infoItem}>
              <div>Transmission:</div>
              <div>{carInfo.transmission}</div>
            </div>
            <div className={styles.infoItem}>
              <div>Made In:</div>
              <div>{carInfo.madeIn}</div>
            </div>
            <div className={styles.infoItem}>
              <div>SeatingCapacity:</div>
              <div>{carInfo.seatingCapacity}</div>
            </div>
            <div className={styles.infoItem}>
              <div>Km Driven:</div>
              <div>{carInfo.kmDriven}</div>
            </div>
            <div className={styles.infoItem}>
              <div>Year:</div>
              <div>{carInfo.year}</div>
            </div>
            <div className={styles.infoItem}>
              <div>Fuel Type:</div>
              <div>{carInfo.fuelType}</div>
            </div>
            <div className={styles.infoItem}>
              <div>Category:</div>
              <div>{carInfo.category}</div>
            </div>
            <div className={styles.infoItem}>
              <div>Color:</div>
              <div>{carInfo.color}</div>
            </div>
            <div className={styles.infoItem}>
              <div>Description:</div>
              <div>{carInfo.description}</div>
            </div>
            <div className={styles.infoItem}>
              <div>Has Installment:</div>
              <div>{carInfo.hasInstallment ? "Yes" : "No"}</div>
            </div>
          </div>
          <h5>Upload Images</h5>
          <FileUploader
            onChange={setFiles}
          />
          < Stack direction="row" spacing={2} justifyContent="center">
            <Button
              variant="outlined"
              onClick={() => { setStep(step - 1) }}
            >
              Back
            </Button>
            <Button
              variant="outlined"
              onClick={handleSubmit}
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

export default RegisterSellCar
