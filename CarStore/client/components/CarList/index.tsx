import { Container, Grid } from '@mui/material';
import Car from '../Car';
import Pagination from "@material-ui/lab/Pagination";
import { useEffect, useState } from "react";
import { useRouter } from "next/router";
import ProductService from '../../services/ProductService';

function CarList(props: any) {

    const [cars, setCars] = useState(props.cars || []);
    const [page, setPage] = useState(1);
    const router = useRouter();
    
    useEffect(() => {
        if (router.query.page) {
            var pageValue = router.query.page as string;        
            if(pageValue !== undefined) {
                setPage(parseInt(pageValue));
            }
        }
    }, [router.query.page]);

    
    function handlePaginationChange(e: any, value: number) {              
        setPage(value);
        if(value != 1) {
            router.push(`?page=${value}`, undefined, { shallow: true });
        } else {
            router.replace('/', undefined, { shallow: true });
        }       

        ProductService.getProducts("", value).then(response => {
            setCars(response.data);
        });
    }

    return (
        <>
            <Container>

                <Pagination
                    count={cars ? cars[0].totalPages : 1}
                    variant='outlined'
                    color='primary'
                    className='pagination'
                    page={page}
                    onChange={handlePaginationChange}
                />
                <br />

                {cars ? (
                    <Grid container direction="row" spacing={5}>
                        {cars.map((car: any) => <Car key={car.id} details={car}></Car>)}
                    </Grid>
                ) : (
                    <h5>Something went wrong!!!</h5>
                )}
                <br />
                
                <Pagination
                    count={cars ? cars[0].totalPages : 1}
                    variant='outlined'
                    color='primary'
                    className='pagination'
                    page={page}
                    onChange={handlePaginationChange}
                />
                
            </Container>
        </>
    )
}



export default CarList;