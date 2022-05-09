import { useEffect, useState } from 'react';
import ProductService from '../../../services/ProductService';
import { Grid, Container } from '@mui/material';
import Car from '../Car';

function CarList() {
    const [cars, setCars] = useState([]);

    useEffect(() => {
        ProductService.getProducts().then(response => setCars(response.data));
    }, []);

    return (
        <>
            <Container>
                <Grid container direction="row" spacing={5}>
                    {cars.map(car => <Car key={car.id} details={car}></Car>)}
                </Grid>
            </Container>
        </>
    )
}

export default CarList;