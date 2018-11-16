'use strict';

const nodemailer = require('nodemailer');

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

var transporter = nodemailer.createTransport({
  service: 'gmail',
  auth: {
      user: 'spacecheckers@gmail.com',
      pass: 'anitalavalatina',
  },
}),
EmailTemplate = require('email-templates').EmailTemplate,
path = require('path'),
Promise = require('bluebird');

// Metodo generador de codigos
var randomCode = function(length){
  var codigo = "";
  var caracteresPermitidos = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm1234567890";
  for(var i = 0; i < length; i++) {
      codigo += caracteresPermitidos.charAt(Math.floor(Math.random() * caracteresPermitidos.length)); 
  }
  return codigo;
}

//Metodo para sacar la informacion del usuario
var correo;
var nombre;
var codigo;
var senderName;
var emailType;
io.on("connection", function(cliente) {
     console.log("Email: Recolectando datos");
    // Evento llamado por cliente al clicker Partida (EnviarCodigo.cs)
    cliente.on("enviarCorreo", function(usernameData) {
        var usuario = {where: {username : usernameData}};
        console.log(usuario);
        app.models.Jugador.findOne(usuario, function(err, user) {
            if (err) throw err;

            if (user != null) {
                // si encuentra el username busca el correo de ese usuario
                nombre = user['username'];
                correo = user['correo'];
                codigo = randomCode(7);
                emailType = 'Activation Code';
                senderName = 'Manolo';
                cliente.emit("correoCliente", user);
                console.log(user);
            } else {
                // 
            }
         });
     });
 });

// codigo = randomCode(7);
// nombre = 'Dany';
// correo = 'dannyhvalenz@gmail.com'
// senderName = 'Manolo'
// emailType = 'Invitation'

let emailData = [
    {
        name: nombre,
        email: correo,
        code: codigo,
        sender: senderName,
    },
];

function sendEmail (obj) {
  return transporter.sendMail(obj);
}

function loadTemplate (templateName, contexts) {
  let template = new EmailTemplate(path.join(__dirname, 'templates', templateName));
  return Promise.all(contexts.map((context) => {
      return new Promise((resolve, reject) => {
          template.render(context, (err, result) => {
              if (err) reject(err);
              else resolve({
                  email: result,
                  context,
              });
          });
      });
  }));
}

// loadTemplate(emailType, emailData).then((results) => {
//   return Promise.all(results.map((result) => {
//       sendEmail({
//           to: result.context.email,
//           from: 'Space Checkers',
//           subject: result.email.subject,
//           html: result.email.html,
//       });
//   }));
// }).then(() => {
//   console.log('The email has been sent');
// });
