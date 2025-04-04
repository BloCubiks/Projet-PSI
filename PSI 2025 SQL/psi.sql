DROP DATABASE IF EXISTS psi;
CREATE DATABASE psi;
USE psi;

-- Création de la table Cuisinier
CREATE TABLE Cuisinier (
    NumeroCuisinier INT PRIMARY KEY,
    NomC VARCHAR(50),
    PrenomC VARCHAR(50),
    AdresseC VARCHAR(50),
    CodePostalC VARCHAR(10),
    VilleC VARCHAR(50),
    TelC VARCHAR(15),
    EmailC VARCHAR(100),
    MetroC VARCHAR(50)
);

-- Table Paticulier
CREATE TABLE Particulier (
    NumeroParticulier INT PRIMARY KEY,
    NomP VARCHAR(50),
    PrenomP VARCHAR(50),
    AdresseP VARCHAR(50),
    CodePostalP VARCHAR(10),
    TelP VARCHAR(15),
    EmailP VARCHAR(100),
    MetroP VARCHAR(50)
);

-- Table Entreprise
CREATE TABLE Entreprise (
    NumeroEntreprise INT PRIMARY KEY,
    NomE VARCHAR(50),
    ContactE VARCHAR(50),
    AdresseE VARCHAR(50),
    CodePostalE VARCHAR(10),
    TelE VARCHAR(15),
    EmailE VARCHAR(100),
    MetroE VARCHAR(50)
);

-- Table Commande
CREATE TABLE Commande (
    IDCommande INT AUTO_INCREMENT NOT NULL PRIMARY KEY,
    DateCommande DATE,
    AdresseLivraison VARCHAR(255),
    Satisfaction INT,
    CHECK (Satisfaction >= 0 AND Satisfaction <= 10),
    NumeroCuisinier INT,
    NumeroParticulier INT,
    NumeroEntreprise INT,
    FOREIGN KEY (NumeroCuisinier) REFERENCES Cuisinier(NumeroCuisinier) ON DELETE CASCADE,
    FOREIGN KEY (NumeroParticulier) REFERENCES Particulier(NumeroParticulier) ON DELETE CASCADE,
    FOREIGN KEY (NumeroEntreprise) REFERENCES Entreprise(NumeroEntreprise) ON DELETE CASCADE
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
    Nationalite VARCHAR(50),
    NumeroCuisinier INT,
    FOREIGN KEY (NumeroCuisinier) REFERENCES Cuisinier(NumeroCuisinier) ON DELETE CASCADE
);

-- Table Ingredient
CREATE TABLE Ingredient (
    IdIngredient INT PRIMARY KEY AUTO_INCREMENT,
    Nom VARCHAR(255) NOT NULL,
    Quantite INT NOT NULL
);

-- Table EstCompose
CREATE TABLE EstCompose (
    IdPlat INT,
    IdIngredient INT,
    FOREIGN KEY (IdPlat) REFERENCES Plat(IdPlat) ON DELETE CASCADE,
    FOREIGN KEY (IdIngredient) REFERENCES Ingredient(IdIngredient) ON DELETE CASCADE
);

-- Table Contient
CREATE TABLE Contient (
    IdCommande INT,
    IdPlat INT,
    FOREIGN KEY (IdCommande) REFERENCES Commande(IdCommande) ON DELETE CASCADE,
    FOREIGN KEY (IdPlat) REFERENCES Plat(IdPlat) ON DELETE CASCADE
);

-- Peuplement de la table Cuisinier
INSERT INTO Cuisinier VALUES 
(1, 'Dupont', 'Jean', 'Rue de Paris', '75001', 'Paris', '0601020304', 'jean.dupont@mail.com', 'Châtelet'),
(2, 'Lemoine', 'Claire', 'Boulevard Haussmann', '75009', 'Paris', '0611223344', 'claire.lemoine@mail.com', 'Opéra'),
(3, 'Bernard', 'Luc', 'Rue Lafayette', '75010', 'Paris', '0622334455', 'luc.bernard@mail.com', 'Gare du Nord');

-- Peuplement de la table Particulier
INSERT INTO Particulier VALUES 
(1, 'Martin', 'Sophie', 'Rue de Lyon', '69001', '0612345678', 'sophie.martin@mail.com', 'Bellecour'),
(2, 'Durand', 'Paul', 'Rue de Rivoli', '75001', '0623456789', 'paul.durand@mail.com', 'Châtelet');

-- Peuplement de la table Entreprise
INSERT INTO Entreprise VALUES 
(1, 'TechCorp', 'Julie Morel', 'Avenue des Champs', '75008', '0634567890', 'contact@techcorp.com', 'Franklin D. Roosevelt'),
(2, 'DataSoft', 'Marc Legrand', 'Boulevard Voltaire', '75011', '0645678901', 'marc.legrand@datasoft.com', 'République');

-- Peuplement de la table Commande
INSERT INTO Commande (IDCommande, DateCommande, AdresseLivraison, Satisfaction, NumeroCuisinier, NumeroParticulier, NumeroEntreprise) VALUES 
(1, '2024-03-30', 'Rue de Lyon, 45', 8, 1, 1, NULL),
(2, '2024-03-29', 'Avenue des Champs, 50', 10, 2, NULL, 1),
(3, '2024-03-28', 'Boulevard Voltaire, 20', 7, 3, NULL, 2);

-- Peuplement de la table Plat
INSERT INTO Plat VALUES 
(1, 'Pizza Margherita', 12.50, 5, 'Italien', '2024-03-28', '2024-04-02', 'Végétarien', 'Italien', 1),
(2, 'Sushi Saumon', 15.00, 10, 'Japonais', '2024-03-29', '2024-04-03', 'Pescetarien', 'Japonais', 2),
(3, 'Burger Classic', 10.00, 7, 'Américain', '2024-03-27', '2024-04-01', 'Omnivore', 'Américain', 3);

-- Peuplement de la table Ingredient
INSERT INTO Ingredient (IdIngredient, Nom, Quantite) VALUES 
(1, 'Tomate', 10), 
(2, 'Mozzarella', 5), 
(3, 'Basilic', 2), 
(4, 'Saumon', 4), 
(5, 'Riz', 6),
(6, 'Viande hachée', 8), 
(7, 'Pain burger', 6);

-- Peuplement de la table EstCompose
INSERT INTO EstCompose VALUES 
(1, 1), (1, 2), (1, 3), 
(2, 4), (2, 5), 
(3, 6), (3, 7);

-- Peuplement de la table Contient
INSERT INTO Contient VALUES 
(1, 1), 
(2, 2), 
(3, 3);
