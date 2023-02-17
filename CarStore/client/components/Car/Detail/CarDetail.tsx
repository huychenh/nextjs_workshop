import { Avatar, Button, Grid, List, ListItem, ListItemAvatar, ListItemSecondaryAction, ListItemText } from "@mui/material";
import React from "react";
import { Models } from "../../../models/product";
import styles from './CarDetail.module.css';
import { Swiper, SwiperSlide } from 'swiper/react';
import 'swiper/css';
import { Autoplay, Pagination, Navigation } from "swiper";
import { formatCurrency } from "../../../lib/formatCurrency";
import { Label, LocalGasStation, Palette, Public, Speed, Today } from "@mui/icons-material";

export class CarDetailProps {
  detail?: Models.Product
}

const CarDetail = ({ detail }: CarDetailProps) => {
  const renderInfoItem = (icon: any, label: string, info: any) => {
    return <List>
      <ListItem>
        <ListItemAvatar>
          <Avatar>
            {icon}
          </Avatar>
        </ListItemAvatar>
        <ListItemText
          primary={label}
        />
        <ListItemSecondaryAction>
          {info}
        </ListItemSecondaryAction>
      </ListItem>
    </List>
  }

  const images = detail && detail.images && detail.images.length ? detail.images : ['/HomePage/Car/car.png']

  return <>
    {
      detail &&
      <>
        <h1>Car Detail</h1>

        <Swiper
          spaceBetween={50}
          slidesPerView={1}
          autoplay={{
            delay: 4000,
            disableOnInteraction: false,
          }}
          pagination={{
            clickable: true,
          }}
          navigation={true}
          modules={[Autoplay, Pagination, Navigation]}
          className="mySwiper"
        >
          {images.map(url =>
            <SwiperSlide key={url}>
              <div style={{
                backgroundImage: `url(${url})`,
                backgroundSize: 'contain',
                backgroundRepeat: 'no-repeat',
                backgroundPosition: 'center',
                height: '40vh'
              }}></div>
            </SwiperSlide>
          )}
        </Swiper>

        <h3>{detail.name}</h3>
        <p className={styles.price}>{formatCurrency(detail.price!)}</p>
        <Button variant="contained" color="secondary" href={`/order/${detail.id}`}>ORDER</Button>
        <h2>Basic Info</h2>
        <Grid container spacing={4} className={styles.basicInfo}>
          <Grid item xs={12} md={6}>
            <div className={styles.infoItem}>
              {renderInfoItem(<Label />, "Brand", detail.brand)}
              {renderInfoItem(<Speed />, "Driven", `${detail.kmDriven} (km)`)}
              {renderInfoItem(<Public />, "Made In", detail.madeIn)}
            </div>
          </Grid>
          <Grid item xs={12} md={6}>
            <div className={styles.infoItem}>
              {renderInfoItem(<Today />, "Year", detail.year)}
              {renderInfoItem(<LocalGasStation />, "Fuel Type", detail.fuelType)}
              {renderInfoItem(<Palette />, "Color", detail.color)}
            </div>
          </Grid>
        </Grid>
        <h2>Description</h2>
        <p>{detail.description}</p>
      </>
    }
  </>
}

export default CarDetail;