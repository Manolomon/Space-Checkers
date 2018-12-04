'use strict';

const nodemailer = require('nodemailer');

var express = require('express')();
var loopback = require('loopback');
var boot = require('loopback-boot');
var http = require('http').Server(express);
var io = require("socket.io")(http, { 'pingInterval': 5000,'pingTimeout': 15000 });
var app = module.exports = loopback();

var loggeando = {}

// Variables utilizadas para enviar los dos tipos de correos
var correo;
var nombre;
var codigo;
var senderName;

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
  console.log("Usuario conectado a servidor " + cliente.id);
  cliente.on("error", (reason) =>
  {
    console.log(reason + " " + cliente.id);
  });
  cliente.on('disconnect', (reason) => {
    console.log(reason + " " + cliente.id);
  });
  // Evento llamado por cliente al clicker iniciar sesion (ButtonLogin.cs)
  cliente.on("login", function(usernameData){
    var filtro = {where: {username : usernameData}};
    console.log(filtro);
    loggeando[cliente.id] = usernameData;
    app.models.Jugador.findOne(filtro, function (err, user) {
      if (err) throw err;
    
      if (user != null) {
        // manda la info del usuario si lo encuentra
        console.log(user['pass']);
        cliente.emit("loginCliente", user['pass']);
      }
    });
  });

  // Evento llamado por cliente al confirmar que la contrasena es correcta
  cliente.on("loginSuccess", function(){
    var filtro = {where: {username : loggeando[cliente.id]}};
    console.log(filtro);
    app.models.Jugador.findOne(filtro, function (err, user) {
      if (err) throw err;
      
      if (user != null) {
        // manda la info del usuario si lo encuentra
        cliente.emit("loginSuccessCliente", user);
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
    var codigo = randomCode(5); //generacion de codigo
    cliente.join(codigo);
    cliente.emit("createLobby", codigo);
  });

  // Evento llamado por cliente al clicker join game (TO DO in client)
  cliente.on("joinGame", function(user){
    var codigo = '1A2B'; //codigo que llega del cliente
    io.to(codigo).emit('userJoinedRoomCliente', user);
    cliente.join(codigo);
    //console.log(io.sockets.clients(codigo));
    io.sockets.in(codigo).emit("getLobbyInfo");
    console.log("Usuario uniendose a sala " + codigo);
  });

  cliente.on("setLobbyInfo", function(lobby){
    console.log("info de lobby raw");
    console.log(lobby);
    var jsonlobby = JSON.parse(lobby);
    console.log("lobby en json");
    console.log(jsonlobby);
    var stringlobby = JSON.stringify(jsonlobby);
    io.sockets.in(jsonlobby['idLobby']).emit("setLobbyInfo", stringlobby);
  });

  // Evento llamado por cliente al dar click en Start Game (no se ha definido aun, probably clase nueva)
  cliente.on("startGame", function(room){
    console.log("iniciando juego en: " + room);
    io.sockets.in(room).emit("startGameCliente");
  });

  // Evento llamado por cliente al hacer un movimiento en su turno correspondiente
  cliente.on("moverPieza", function(datos) {
    var movement = {ficha: datos[1], casilla: datos[2]};
    io.sockets.in('1A2B').emit('moverPiezaCliente', movement); //codigo del lobby
  });

  // Metodos para enviar correos
  cliente.on("sendInvitation", function(usernameData) {
    var usuario = {where: {username : usernameData}};
    console.log(usuario);
    app.models.Jugador.findOne(usuario, function(err, user) {
      if (err) throw err;

      if (user != null) {
        // si encuentra el username
        nombre = user['username'];
        correo = user['correo'];
        codigo = ""; // como lo paso del lobby hasta aca
        senderName = ""; // en donde debo  guardarlo antes para poderlo usar aqui
             
        loadTemplate('Invitation', emailData).then((results) => {
          return Promise.all(results.map((result) => {
              sendEmail({
                  to: result.context.email,
                  from: 'Space Checkers',
                  subject: result.email.subject,
                  html: result.email.html,
              });
          }));
        }).then(() => {
          console.log('El correo de invitacion ha sido enviado');
        })
        
        cliente.emit("sendInvitation", user);
        console.log(user);
      }
    });
  });

  cliente.on("sendActivationCode", function(newuserData) {
    nombre = newuserData['username'];
    correo = newuserData['correo'];
    codigo = randomCode(5);
    //senderName = '';
          
    loadTemplate('Activation Code', emailData).then((results) => {
      return Promise.all(results.map((result) => {
          sendEmail({
              to: result.context.email,
              from: 'Space Checkers',
              subject: result.email.subject,
              html: result.email.html,
          });
      }));
    }).then(() => {
      console.log('El correo de activacion ha sido enviado');
    })
    
    cliente.emit("sendActivationCode", codigo);
  });

  cliente.on('mensaje', function(data) {
    io.sockets.emit('mensaje', {
      name : 
      msj : data.message;
    });
  });

});

function getKeyByValue(object, value) {
  return Object.keys(object).find(key => object[key] === value);
}

// Bootstrap the application, configure models, datasources and middleware.
// Sub-apps like REST API are mounted via boot scripts.
boot(app, __dirname, function(err) {
  //if (err) throw err;

  // start the server if `$ node server.js`
  if (require.main === module)
    app.start();
});

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


// Metodo generador de codigos
var randomCode = function(length){
  var codigo = "";
  var caracteresPermitidos = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm1234567890";
  for(var i = 0; i < length; i++) {
      codigo += caracteresPermitidos.charAt(Math.floor(Math.random() * caracteresPermitidos.length)); 
  }
  return codigo;
}