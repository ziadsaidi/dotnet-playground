

-- CREATE TABLE customer s

CREATE TABLE customers (
     id uuid   NOT NULL,
     name text not null,
     CONSTRAINT pk_customer_id  PRIMARY KEY(id)
);

-- CREATE TABLE employees

CREATE TABLE employees(
    id uuid NOT NULL,
    name text NOT NULL,
    CONSTRAINT pk_employee_id PRIMARY KEY(id)
);

-- CREATE TABLE products 
CREATE TABLE products(
    id uuid  NOT NULL,
    name text not null,
    price   DOUBLE PRECISION CHECK (price > 0),
    CONSTRAINT pk_product_id PRIMARY Key(id)
);

-- CREATE TABLE Sales

CREATE TABLE sales(
    id uuid NOT NULL,
    unit_price DOUBLE PRECISION,
    quantity int ,
    total_price DOUBLE PRECISION,
    customer_id uuid not null,
    product_id uuid not null,
    employee_id uuid not null,
    CONSTRAINT pk_sales_id PRIMARY KEY(id),
    CONSTRAINT fk_sales_customer FOREIGN KEY (customer_id) REFERENCES customers(id) ON DELETE CASCADE,
    CONSTRAINT fk_sales_employee FOREIGN Key(employee_id) REFERENCES employees(id) ON DELETE CASCADE,
    CONSTRAINT fk_sales_product FOREIGN KEY(product_id) REFERENCES products(id) On DELETE CASCADE
);




