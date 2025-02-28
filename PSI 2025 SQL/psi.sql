DROP DATABASE IF EXISTS psi;
CREATE DATABASE IF NOT EXISTS psi;
USE psi;

-- Table Cuisinier
CREATE TABLE Cuisinier (
    NumeroCuis INT PRIMARY KEY,
    NomCuis VARCHAR(50),
    PrenomCuis VARCHAR(50),
    Adresse TEXT,
    CodePostal VARCHAR(10),
    Ville VARCHAR(50),
    Telephone VARCHAR(15),
    Mail VARCHAR(100),
    Metro VARCHAR(50),
    NbreLivraison INT,
    Note FLOAT check (0<=note AND note<=5)
);

-- Table Client
CREATE TABLE Client (
    NumeroClient INT PRIMARY KEY,
    NomClient VARCHAR(50),
    PrenomClient VARCHAR(50),
    Adresse TEXT,
    CodePostal VARCHAR(10),
    Ville VARCHAR(50),
    Telephone VARCHAR(15),
    Mail VARCHAR(100),
    Metro VARCHAR(50)
);

-- Table Particulier (héritant de Client)
CREATE TABLE Particulier (
    IdParticulier INT PRIMARY KEY,
    MDPParticulier VARCHAR(100),
    FOREIGN KEY (IdParticulier) REFERENCES Client(NumeroClient) ON DELETE CASCADE
);

-- Table Entreprise (héritant de Client)
CREATE TABLE Entreprise (
    NumeroE INT PRIMARY KEY,
    NomE VARCHAR(50),
    MDPEntreprise VARCHAR(100),
    FOREIGN KEY (NumeroE) REFERENCES Client(NumeroClient) ON DELETE CASCADE
);

-- Table Commande
CREATE TABLE Commande (
    IdCommande INT PRIMARY KEY,
    DateCommande DATE,
    Statut VARCHAR(20) CHECK (Statut IN ('en prep', 'en chemin', 'en bas', 'livré')),
    AdresseDepart TEXT,
    AdresseArrivee TEXT
);

-- Table Plat
CREATE TABLE Plat (
    IdPlat INT PRIMARY KEY,
    NomPlat VARCHAR(50),
    Prix DECIMAL(10,2),
    Quantite INT,
    TypePlat VARCHAR(50),
    DateFabrication DATE,
    DatePeremption DATE,
    RegimeAlim VARCHAR(50),
    Nationalite VARCHAR(50)
);

-- Table Ingredient
CREATE TABLE Ingredient (
    IdIngredient INT PRIMARY KEY AUTO_INCREMENT,
    Nom VARCHAR(255) NOT NULL,
    Quantite INT NOT NULL
);


-- Table EstCompose (relation Plat - Ingredient)
CREATE TABLE EstCompose (
    IdPlat INT,
    IdIngredient INT,
    PRIMARY KEY (IdPlat, IdIngredient),
    FOREIGN KEY (IdPlat) REFERENCES Plat(IdPlat) ON DELETE CASCADE,
    FOREIGN KEY (IdIngredient) REFERENCES Ingredient(IdIngredient) ON DELETE CASCADE
);

-- Table Passe (relation Client - Commande)
CREATE TABLE Passe (
    NumeroClient INT,
    IdCommande INT,
    PRIMARY KEY (NumeroClient, IdCommande),
    FOREIGN KEY (NumeroClient) REFERENCES Client(NumeroClient) ON DELETE CASCADE,
    FOREIGN KEY (IdCommande) REFERENCES Commande(IdCommande) ON DELETE CASCADE
);

-- Table Contient (relation Commande - Plat)
CREATE TABLE Contient (
    IdCommande INT,
    IdPlat INT,
    PRIMARY KEY (IdCommande, IdPlat),
    FOREIGN KEY (IdCommande) REFERENCES Commande(IdCommande) ON DELETE CASCADE,
    FOREIGN KEY (IdPlat) REFERENCES Plat(IdPlat) ON DELETE CASCADE
);

-- Table Créer (relation Cuisinier - Plat)
CREATE TABLE Creer (
    NumeroCuis INT,
    IdPlat INT,
    PRIMARY KEY (NumeroCuis, IdPlat),
    FOREIGN KEY (NumeroCuis) REFERENCES Cuisinier(NumeroCuis) ON DELETE CASCADE,
    FOREIGN KEY (IdPlat) REFERENCES Plat(IdPlat) ON DELETE CASCADE
);

-- Table Specialise (relation Client - Particulier/Entreprise)
CREATE TABLE Specialise (
    NumeroClient INT PRIMARY KEY,
    TypeClient ENUM('Particulier', 'Entreprise') NOT NULL,
    FOREIGN KEY (NumeroClient) REFERENCES Client(NumeroClient) ON DELETE CASCADE
);

