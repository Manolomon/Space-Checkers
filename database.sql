CREATE DATABASE spacecheckers;

USE spacecheckers;

CREATE USER 'spacecheckers'@'localhost' IDENTIFIED BY 'space123';

GRANT ALL PRIVILEGES ON spacecheckers.* TO 'spacecheckers'@'localhost';

CREATE TABLE jugador (
    username varchar(50),
    correo varchar(100),
    pass varchar(64), 
    partidasJugadas int,
    partidasGanadas int,
    id int AUTO_INCREMENT,
    PRIMARY KEY (id)    
);

INSERT INTO jugador VALUES (
    'Deklok','danix100@hotmail.com','5994471abb01112afcc18159f6cc74b4f511b99806da59b3caf5a9c173cacfc5',1,1,1
);