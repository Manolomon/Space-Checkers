'use strict';

var express = require('express')();
var loopback = require('loopback');
var boot = require('loopback-boot');
var http = require('http').Server(express);
var io = require("socket.io")(http, { pingInterval: 500 });

var app = module.exports = loopback();

http.listen(5000, function() {
  console.log("socket.io escuchando en 5000");
});

app.start = function() {
  // start the web server
  return app.listen(function() {
    app.emit('started');
    var baseUrl = app.get('url').replace(/\/$/, '');
    console.log("Servidor listo...");
  });
};

io.on("connection", function(cliente) {
  console.log("Usuario conectado a servidor");
  cliente.emit("test", "aaaaa!");
  cliente.on("error", (reason) =>
  {
    console.log(reason);
  });
  // Evento llamado por cliente al clicker iniciar sesion (ButtonLogin.cs)
  cliente.on("login", function(usernameData){
    var filtro = {where: {username : usernameData}};
    console.log(filtro);
    app.models.Jugador.findOne(filtro, function (err, user) {
      if (err) throw err;
      
      cliente.emit("loginCliente", user);
      cliente.emit("test", "aaaaa!");
      if (user != null) {
        // manda la info del usuario si lo encuentra
        cliente.emit("loginCliente", user);
        console.log(user);
        io.to(cliente.id).emit("test", "aaaaaaaaaaa");
      }
    });
  });

  // Evento llamado por cliente al clicker registrar (TO DO in client)
  cliente.on("registro", function(datos){
    app.models.Jugador.create(datos, function(err, response) {
      if (err) throw err;
    });
  });

  // Evento llamado por cliente al clickear create game (implemented in ButtonLogin.cs atm)
  cliente.on("createGame", function(){
    cliente.join('1A2B');
    cliente.emit("createLobby", "1A2B");
  });

  // Evento llamado por cliente al clicker join game (TO DO in client)
  cliente.on("joinGame", function(user){
    cliente.join('1A2B');
    io.to('1A2B').emit('userJoinedRoomCliente', user);
    cliente.on("getLobbyInfo", function(lobbyInfo) {
      cliente.emit("setLobbyInfo", lobbyInfo);
    });
  });

  // Evento llamado por cliente al dar click en Start Game (no se ha definido aun, probably clase nueva)
  cliente.on("startGame", function(room){
    io.to('1A2B').emit('startGameCliente');
  });

  // Evento llamado por cliente al hacer un movimiento en su turno correspondiente
  cliente.on("moverPieza", function(datos) {
    var movement = {ficha: datos[1], casilla: datos[2]};
    io.to('1A2B').emit('moverPiezaCliente', movement);
  });
});

// Bootstrap the application, configure models, datasources and middleware.
// Sub-apps like REST API are mounted via boot scripts.
boot(app, __dirname, function(err) {
  if (err) throw err;

  // start the server if `$ node server.js`
  if (require.main === module)
    app.start();
});
