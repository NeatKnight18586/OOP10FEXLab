-- For testing at home only (for example XAMPP / MySQL Workbench).
-- Do NOT run this on the instructor's server: fastfooddb already exists there.
-- Table and column names match the instructor's sample queries; the column
-- types and the id columns are our guess, since the sample did not include them.

CREATE DATABASE IF NOT EXISTS fastfooddb;
USE fastfooddb;

CREATE TABLE IF NOT EXISTS package_tbl (
    package_id   INT AUTO_INCREMENT PRIMARY KEY,
    package_name VARCHAR(100) NOT NULL
);

CREATE TABLE IF NOT EXISTS transactions_tbl (
    transaction_id   INT AUTO_INCREMENT PRIMARY KEY,
    package_name     VARCHAR(100) NOT NULL,
    transaction_date DATETIME NOT NULL
);

INSERT INTO package_tbl (package_name) VALUES
    ('Chicken Adobo Meal'), ('Pork Sinigang Meal'), ('Bicol Express Meal'),
    ('Tapsilog'), ('Tocilog'), ('Longsilog'),
    ('Pancit Canton'), ('Lumpiang Shanghai'), ('Iced Tea'), ('Calamansi Juice');

-- See the saved orders:
-- SELECT * FROM transactions_tbl ORDER BY transaction_date DESC;
