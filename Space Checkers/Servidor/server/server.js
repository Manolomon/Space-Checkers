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
  cliente.on("login", function(datos){
    var filtro = {where: {username : "Deklok"}};
    app.models.Jugador.findOne(filtro, function (err, user) {
      if (err) throw err;
      
      console.log(user);
      if (user != null) {
        cliente.emit("loginCliente",user);
      } else {
        cliente.emit("loginCliente",false);
      }
    });
  });

  cliente.on("registro", function(datos){
    app.models.Jugador.create(datos, function(err, response) {
      if (err) throw err;
    });
  });

  cliente.on("createGame", function(){
    //generacion de codigo. ej: e4g2s
    //cliente.join('e4g2s');
    //enviar evento a cliente de respuesta
  });

  cliente.on("joinGame", function(partida){
    cliente.join(partida);
    io.to(partida).emit('userJoined');
    //enviar evento a cliente de respuesta
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
