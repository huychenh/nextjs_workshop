import { Avatar, Grid, List, ListItem, ListItemAvatar, ListItemSecondaryAction, ListItemText } from "@material-ui/core";
import { Label, LocalGasStation, Palette, Public, Speed, Today } from "@material-ui/icons";
import { Button } from "@mui/material";
import React from "react";
import { Models } from "../../../models/product";
import styles from './CarDetail.module.css';
import Slider from 'react-slick';

export class CarDetailProps {
  detail?: Models.Product
}

const CarDetail = ({ detail }: CarDetailProps) => {
  const formatter = new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'USD',
  });

  // Todo: find a better image slider. May be the slider in the brand list
  const sliderSettings = {
    dots: true,
    infinite: true,
    speed: 500,
    slidesToShow: 1,
    slidesToScroll: 1,
    arrows: true,
    autoplay: true,
    autoplaySpeed: 4000,
  };
  const images = detail && detail.images && detail.images.length ? detail.images : ['/HomePage/Car/ford.jpg']

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

  return <>
    {
      detail &&
      <>
        <h1>Car Detail</h1>
        <Slider {...sliderSettings}>
          {images.map(url =>
            <div>
              <div style={{
                backgroundImage: `url(${url})`,
                backgroundSize: 'contain',
                backgroundRepeat: 'no-repeat',
                backgroundPosition: 'center',
                height: '40vh'
              }}></div>
            </div>
          )}
        </Slider>
        <h3>{detail.name}</h3>
        <p className={styles.price}>{formatter.format(detail.price!)}</p>
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