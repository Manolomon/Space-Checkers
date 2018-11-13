'use strict';

var loopback = require('loopback');
var boot = require('loopback-boot');
var io = require("socket.io")(5000);

var app = module.exports = loopback();

app.start = function() {
  // start the web server
  return app.listen(function() {
    app.emit('started');
    var baseUrl = app.get('url').replace(/\/$/, '');
    console.log('Web server listening at: %s', baseUrl);
    if (app.get('loopback-component-explorer')) {
      var explorerPath = app.get('loopback-component-explorer').mountPath;
      console.log('Browse your REST API at %s%s', baseUrl, explorerPath);
    }
  });
};

io.on("connection", function(cliente) {
  console.log("Usuario conectado a servidor");
  // Evento llamado por cliente al clicker iniciar sesion (ButtonLogin.cs)
  cliente.on("login", function(usernameData){
    var filtro = {where: {username : usernameData}};
    console.log(filtro);
    app.models.Jugador.findOne(filtro, function (err, user) {
      if (err) throw err;
      
      if (user != null) {
        // manda la info del usuario si lo encuentra
        cliente.emit("loginCliente", user);
        console.log(user);
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
