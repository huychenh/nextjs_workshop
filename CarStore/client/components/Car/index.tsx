import { Grid, Typography, Link } from "@mui/material";
import LanguageIcon from '@mui/icons-material/Language';
import styles from './Car.module.css';
import { formatCurrency } from "../../lib/formatCurrency";

const Car = ({ details }: any) => {
    const imageUrl = details.images?.length ? details.images[0] : '/HomePage/Car/car.png';

    return (
        <Grid item md={6}>
            <Grid container>
                <Grid item xs={5}>
                    <div className={styles.imageContainer}>
                        <Link href={`car-detail/${details.id}`} >
                            <img className={styles.carImage} src={imageUrl}></img>
                        </Link>
                    </div>
                </Grid>
                <Grid item xs={7}>
                    <p className={styles.carName}>{details.name}</p>
                    <p className={styles.carPrice} >{formatCurrency(details.price)}</p>
                    <Typography variant="body2" ><img src="/HomePage/Car/hangxe.png"></img>{details.brand}</Typography>
                    <Typography variant="body2" ><LanguageIcon></LanguageIcon>{details.madeIn}</Typography>
                    <Typography variant="body2" ><img src="/HomePage/Car/Calendar.png"></img>{details.year}</Typography>
                </Grid>
            </Grid>

        </Grid>
    )
};

export default Car;