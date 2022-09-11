import { Grid } from '@mui/material';
import Car from '../Car';

function CarList(props: any) {
    return (
        <>
            {props.cars ? (
                <Grid container direction="row" spacing={5}>
                    {props.cars.map((car: any) => <Car key={car.id} details={car}></Car>)}
                </Grid>
            ) : (
                <h5>Something went wrong!!!</h5>
            )}
        </>
    )
}

export default CarList;