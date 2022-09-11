import { Grid, Typography, Link } from "@mui/material";
import LanguageIcon from '@mui/icons-material/Language';
import styles from './Car.module.css';

const Car = ({ details }: any) => {
    var formatter = new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD',

        // These options are needed to round to whole numbers if that's what you want.
        //minimumFractionDigits: 0, // (this suffices for whole numbers, but will print 2500.10 as $2,500.1)
        //maximumFractionDigits: 0, // (causes 2500.99 to be printed as $2,501)
    });

    const imageUrl = details.images?.length ? details.images[0] : '/HomePage/Car/ford.jpg';

    return (
        <Grid item md={6}>
            <Grid container>
                <Grid item xs={5}>
                    <Link href={`car-detail/${details.id}`} >
                        <img className={styles.carImage} src={imageUrl}></img>
                    </Link>
                </Grid>
                <Grid item xs={7}>
                    <p className={styles.carName}>{details.name}</p>
                    <p className={styles.carPrice} >{formatter.format(details.price)}</p>
                    <Typography variant="body2" ><img src="/HomePage/Car/hangxe.png"></img>{details.brand}</Typography>
                    <Typography variant="body2" ><LanguageIcon></LanguageIcon>{details.madeIn}</Typography>
                    <Typography variant="body2" ><img src="/HomePage/Car/Calendar.png"></img>{details.year}</Typography>
                </Grid>
            </Grid>

        </Grid>
    )
};

export default Car;