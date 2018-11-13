CREATE DATABASE spacecheckers;

USE spacecheckers;

GRANT ALL PRIVILEGES ON *.* TO 'spacecheckers'@'localhost' IDENTIFIED BY 'space123';

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
    'Deklok','danix100@hotmail.com','5994471ABB01112AFCC18159F6CC74B4F511B99806DA59B3CAF5A9C173CACFC5',1,1,1
);