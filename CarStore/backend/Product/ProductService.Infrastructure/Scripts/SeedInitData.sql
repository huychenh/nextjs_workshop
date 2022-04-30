CREATE OR REPLACE FUNCTION random_brand ()
RETURNS varchar(50)
LANGUAGE plpgsql
AS $$
DECLARE
	brand varchar(50);
	i integer;
BEGIN
	select floor(random() * 8)::int into i;
	case i
		when 0 then brand = 'Audi';
		when 1 then brand = 'Bentley';
		when 2 then brand = 'BMW';
		when 3 then brand = 'Ferrari';
		when 4 then brand = 'Ford';
		when 5 then brand = 'Honda';
		when 6 then brand = 'Toyota';
		else brand = 'Mazda';
	end case;
	RETURN brand;
END;
$$;

CREATE OR REPLACE FUNCTION random_model ()
RETURNS varchar(50)
LANGUAGE plpgsql
AS $$
BEGIN
	return 'M' || floor(random() * 10 + 1)::int;
END;
$$;

CREATE OR REPLACE FUNCTION random_category ()
RETURNS varchar(50)
LANGUAGE plpgsql
AS $$
DECLARE
category varchar(50);
i integer;
BEGIN
	select floor(random() * 9)::int into i;
	case i
		when 0 then category = 'Sedan';
		when 1 then category = 'Coupe';
		when 2 then category = 'Sport';
		when 3 then category = 'Station wagon';
		when 4 then category = 'Hatchback';
		when 5 then category = 'Convertible';
		when 6 then category = 'SUV';
		when 7 then category = 'Pickup Truck';
		else category = 'Minivan';
	end case;
	RETURN category;
END;
$$;

CREATE OR REPLACE FUNCTION random_seat ()
RETURNS int
LANGUAGE plpgsql
AS $$
DECLARE
i int;
BEGIN
	select floor(random() * 2)::int into i;
	case i
		when 0 then return 4;
		when 1 then return 7;
	end case;
	RETURN category;
END;
$$;

CREATE OR REPLACE FUNCTION random_country ()
RETURNS varchar(50)
LANGUAGE plpgsql
AS $$
DECLARE
i int;
BEGIN
	select floor(random() * 6)::int into i;
	case i
		when 0 then return 'China';
		when 1 then return 'Korea';
		when 2 then return 'Japan';
		when 3 then return 'Thailand';
		when 4 then return 'USA';
		else return 'Germany';
	end case;
	RETURN category;
END;
$$;

CREATE OR REPLACE FUNCTION random_color ()
RETURNS varchar(50)
LANGUAGE plpgsql
AS $$
DECLARE
i int;
BEGIN
	select floor(random() * 6)::int into i;
	case i
		when 0 then return 'Red';
		when 1 then return 'Black';
		when 2 then return 'White';
		when 3 then return 'Silver';
		when 4 then return 'Brown';
		else return 'Yellow';
	end case;
	RETURN category;
END;
$$;

do $$
declare
	brand varchar(50);
	model varchar(50);
	category varchar(50);
	color varchar(50);
	country varchar(50);
	name varchar(200);
	year int;
	price int;
	description varchar(200);
	fuelType int;
	kmDriven int;
	seatCapacity int;
	transmission int;
begin
	for i in 1..100 loop
		select random_brand() into brand;
		select random_model() into model;
		select random_category() into category;
		select random_color() into color;
		select random_country() into country;
		select floor(random() * 10 + 2012)::int into year;
		select brand || ' ' || model || ' ' || year || ' ' || color into name;
		select floor(random() * 20000 + 10000)::int into price;
		select 'Lorem ipsum dolor sit amet, consectetur adipiscing elit' into description;
		select floor(random() * 2)::int into fuelType;
		select floor(random() * 500000)::int into kmDriven;
		select random_seat() into seatCapacity;
		select floor(random() * 4 + 1)::int into transmission;
		
		INSERT INTO public.products(
			id, name, year, price, created, brand, category, color, description, fuel_type, has_installment, km_driven, made_in, model, owner_id, seating_capacity, transmission, active)
			VALUES (gen_random_uuid(), name, year, price, '2022-04-30', brand, category, color, description, fuelType, true, kmDriven, country, model, gen_random_uuid(), seatCapacity, transmission, true);
	end loop;
end; $$;

	
	
	