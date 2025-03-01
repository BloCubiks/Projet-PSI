-- Peuplement Cuisinier
LOAD DATA INFILE 'Cuisinier.csv'
INTO TABLE Cuisinier
FIELDS TERMINATED BY ',' 
ENCLOSED BY '"' 
LINES TERMINATED BY '\n' 
IGNORE 1 ROWS;

-- Peuplement Client
LOAD DATA INFILE 'Client.csv'
INTO TABLE Client
FIELDS TERMINATED BY ',' 
ENCLOSED BY '"' 
LINES TERMINATED BY '\n' 
IGNORE 1 ROWS;

select * from cuisinier;

select * from client;