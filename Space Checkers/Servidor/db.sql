CREATE DATABASE spacecheckers;

USE spacecheckers;

GRANT ALL PRIVILEGES ON *.* TO 'spacecheckers'@'localhost' IDENTIFIED BY 'space123';

CREATE TABLE jugador (
    username varchar(50),
    correo varchar(100),
    pass varchar(50), 
    partidasJugadas int,
    partidasGanadas int,
    id int AUTO_INCREMENT,
    PRIMARY KEY (id)    
);

INSERT INTO jugador VALUES (
    'Deklok','danix100@hotmail.com','12345',1,1,1
);