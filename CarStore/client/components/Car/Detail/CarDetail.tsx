import { Avatar, Grid, IconButton, ImageList, ImageListItem, ImageListItemBar, List, ListItem, ListItemAvatar, ListItemSecondaryAction, ListItemText } from "@material-ui/core";
import { Label, LocalGasStation, Palette, Public, Speed, Today } from "@material-ui/icons";
import React from "react";
import { Models } from "../../../models/product";
import styles from './CarDetail.module.css';

export class CarDetailProps {
  detail?: Models.Product
}

const CarDetail = ({detail} : CarDetailProps) => {
  var formatter = new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'USD',
  });

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
        <img src="/HomePage/Car/ford.jpg" />
        <h3>{detail.name}</h3>
        <p className={styles.price}>{formatter.format(detail.price!)}</p>
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