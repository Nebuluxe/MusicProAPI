# MusicProAPI

Este proyecto esta creado con el fin de simular la compra en linea de productos mediante API REST teniendo la posibilidad de tener un sistema de login y manejo de usuarios, creacion de productos con sus respectivas categorias y stock, carrito de compras y ademas de tener la posibilidad de realizar una transaccion de compra en linea simulada esta simulacion se logra utilizando la api de FakeBankApi_MSP: https://github.com/eljavexe/FakeBankApi_MSP todo esto funcionando de manera local sin uso de ningun tipo de base de datos, todos los datos son almacenados de manera local en archivos TXT funcionando solo en LocalHost, la api se encuentra configurada para ejecutarse en el puerto 400 "https://localhost:4000".

# Uso de Endpoints 
------------------

# 'Usuario'

POST /Usuario/CrearUsuario

la funcion de este endpoint es poder crear un usuario para poder realizar la creacion de un suuario es necesario mandar por el Request body un json con el siguiente formato:

Ejemplo:

  URL = https://localhost:4000/Usuario/CrearUsuario

  {
    "id_Usuario": 0,
    "nombre": "Juanito",
    "apellido": "Alcachofa",
    "correo": "JuanitoAlcachofa@gmail.com",
    "password": "123456"
  }

  Respuesta:

  {
    "resultTransaccion": true,
    "message": "El usuario Juanito Alcachofa fue registrado correcctamente"
  }

importante mencionar que no es necesario llenar el 'id_usuario' ya que la api lo genera de manera automatica por lo que solo dejar en 0, los demas campos son obligatorios para la creacion de un usuario.


GET /Usuario/GetUsuarios

la funcion de este endpoint es traer la lista completa de los usuarios generados mediante el uso del endpoint /Usuario/CrearUsaurio, a continuacion un ejemplo del resultado que retornaria el endpoint.

Ejemplo:

  URL = https://localhost:4000/Usuario/GetUsuarios

  Respuesta:

  [
    {
      "id_Usuario": 1,
      "nombre": "Juanito",
      "apellido": "Alcachofa",
      "correo": "JuanitoAlcachofa@gmail.com",
      "password": "123456"
    },
    {
      "id_Usuario": 2,
      "nombre": "Elva",
      "apellido": "Silon",
      "correo": "ElvaSilon@gmail.com",
      "password": "654321"
    }
  ]


GET https://localhost:4000/Usuario/GetUsuario

la funcion de este endpoint es poder buscar un usuario en los registros generado mediante el uso del endpoint /Usuario/CrearUsaurio el parametro de busqueda es el id del usaurio en cuestion, a continuacion un ejemplo del resultado que retornaria el endpoint.

Ejemplo:

  URL = https://localhost:4000/Usuario/CrearUsuario?id_usuario=1

  Respuesta:

  {
    "id_Usuario": 1,
    "nombre": "Juanito",
    "apellido": "Alcachofa",
    "correo": "JuanitoAlcachofa@gmail.com",
    "password": "123456"
  }


PUT /Usuario/ModificarUsuario

la funcion de este endpoint es poder modificar la informacion de un usuario tales como, nombree, apellido, correo, password. para poder modificar un usuario debemos mandar en el reques body un json con el siguiente formato:

Ejemplo:

  URL = https://localhost:4000/Usuario/ModificarUsuario

  {
    "id_Usuario": 1,
    "nombre": "",
    "apellido": "",
    "correo": "JuanitoAlcachofa@hotmail.com",
    "password": "781516"
  }

  Respuesta:

  {
    "resultTransaccion": true,
    "message": "El usuario Juanito Alcachofa fue modificado correcctamente"
  }
  
importante mencionar que para modificar un usaurio debemos llenar el campo de id_usuario como se muestra en el ejemplo y ademas solo debemos mandar en el json los campos que deseamos modificar, en el ejemplo se muestra como solo se llanan los campos de correo y password ya que son los que deseamos modificar.


DELETE /Usuario/EliminarUsuario

la funcion de este endpoint es poder eliminar un usuario utilizando como parametro el id del usuario, a continuacion un ejemplo de lo que retornaria la api al eliminar el usuario en cuestion.

Ejemplo

  URL = https://localhost:4000/Usuario/EliminarUsuario?id_usuario=1

  Respuesta:

  {
    "resultTransaccion": true,
    "message": "El usuario Juanito Alcachofa fue eliminado correcctamente"
  }



# 'CategoriaProducto'

POST /CategoriaProducto/CrearCategoria

la funcion de este endpoint es poder crear la categoria de un producto, para la creacion de dicha categoria debemos por el Request body un json con el siguiente formato:

Ejemplo:

  URL = https://localhost:4000/CategoriaProducto/CrearCategoria

  {
    "id_Categoria": 0,
    "nombre": "Instrumento de cuerda",
    "descripcion": "Esta es una categoria para instrumentos de cuerda"
  }

  Respuesta:

  {
    "resultTransaccion": true,
    "message": "La categoria Instrumento de cuerda fue resgistrada exitosamente"
  }


GET /CategoriaProducto/GetCategorias

la funcion de este endpoint es poder traer la lista completa de categorias generadas medainte el endpoint /CategoriaProducto/CrearCategoria, a continuacion un ejemplo del restultado que retornaria el endpoint.

Ejemplo:

  URL = https://localhost:4000/CategoriaProducto/GetCategorias

  Respuesta:

  [
    {
      "id_Categoria": 1,
      "nombre": "Instrumento de cuerda",
      "descripcion": "Esta es una categoria para instrumentos de cuerda"
    },
    {
      "id_Categoria": 2,
      "nombre": "Instrumento de viento",
      "descripcion": "Esta es una categoria para instrumentos de viento"
    }
  ]


GET /CategoriaProducto/GetCategoria

la funcion de este endpoint es poder buscar una categoria en los registros generado mediante el uso del endpoint /CategoriaProducto/CrearCategoria, el parametro de busqueda es el id de la categoria que se desea buscar, a continuacion un ejemplo del resultado que retornaria el endpoint.

  URL = https://localhost:4000/CategoriaProducto/GetCategoria?id_categoria=1

  Respuesta:

  {
    "id_Categoria": 1,
    "nombre": "Instrumento de cuerda",
    "descripcion": "Esta es una categoria para instrumentos de cuerda"
  }


PUT /CategoriaProducto/ModificarCategoria

la funcion de este endpoint es poder modificar una categoria ya sea el nombre o la descripcion. para poder modificar una categoria debemos mandar en el reques body un json con el siguiente formato:

Ejemplo:

  URL = https://localhost:4000/CategoriaProducto/ModificarCategoria

  {
    "id_Categoria": 1,
    "nombre": "",
    "descripcion": "Esta es la nueva descripcion de la categoria instrumento de cuerda"
  }

  Respuesta:

  {
    "resultTransaccion": true,
    "message": "La categoria Instrumento de cuerda fue modificada exitosamente"
  }

importante mencionar que para modificar una categoria debemos llenar el campo de id_Categoria como se muestra en el ejemplo y ademas solo debemos mandar en el json los campos que deseamos modificar, en el ejemplo se muestra como solo se llanan solo el campo de descripcion para modificar y el campo de nombre se deja vacio ya que no se desea modificar.


DELETE /CategoriaProducto/EliminarCategoria

la funcion de este endpoint es poder eliminar una categoria enviadno como parametro el id de dicha categoria que deseamos eliminar, a continuacion un ejemplo de lo que retornaria el endpoint.

Ejemplo:

  URL = https://localhost:4000/CategoriaProducto/EliminarCategoria?id_categoria=1

  Respuesta:
  {
    "resultTransaccion": true,
    "message": "La categoria Instrumento de cuerda fue eliminada exitosamente"
  }



# 'Producto'

POST /Producto/CrearProducto

la funcion de este endpoint es poder crear un producto, para la creacion de dicha categoria debemos por el Request body un json con el siguiente formato:

Ejemplo:

  URL = https://localhost:4000/Producto/CrearProducto

  {
    "id_Producto": 0,
    "nombre": "Guitarra",
    "descripcion": "Guitarra acustica",
    "serieProducto": "ASDJK768S",
    "marca": "Gibson",
    "categoria_id": 1,
    "precio": 150000,
    "fechaCreacion": "",
    "fechaModificacion": "",
    "estado": true
  }

  Respuesta:

  {
    "resultTransaccion": true,
    "message": "El producto Guitarra fue registrado exitosamente"
  }

Importante menccionar que el campo de id_usuario no es necesario llenarlo ya que el endpoint lo genera de forma automatica por lo que no es necesario llenaarlo al igual que el campo fechaCreacion y fechaModificacion.


GET /Producto/GetProductos

la funcion de este metodo es poder traer la lista de los productos generados por el endpoint /Producto/CrearProducto, a continuacion un ejemplo de lo que retornaria el endpoint.

Ejemplo:

  URL = https://localhost:4000/Producto/GetProductos

  Respuesta:

  [
    {
      "id_Producto": 1,
      "nombre": "Guitarra",
      "descripcion": "Guitarra acustica",
      "serieProducto": "ASDJK768S",
      "marca": "Gibson",
      "categoria_id": 1,
      "precio": 150000,
      "fechaCreacion": "27-05-2023 05:48:53",
      "fechaModificacion": "00-00-0000 00:00:00",
      "estado": true
    },
      {
      "id_Producto": 2,
      "nombre": "Trombon",
      "descripcion": "este es un lindo trombon",
      "serieProducto": "as54d465",
      "marca": "Tromboncitos",
      "categoria_id": 2s,
      "precio": 30000,
      "fechaCreacion": "22-05-2023 02:16:05",
      "fechaModificacion": "00-00-0000 00:00:00",
      "estado": true
    }
  ]


GET /Producto/GetProducto

la funcion de este endpoint es poder buscar un producto en los registros generados por el endpoint /Producto/CrearProducto pasando como parametro de busqueda el id del producto en cuestion, a continuacion un ejemplo de lo que retornaia el endpoint.

Ejemplo:

  URL = https://localhost:4000/Producto/GetProducto?id_producto=1

  Respuesta:

  {
    "id_Producto": 1,
    "nombre": "Guitarra",
    "descripcion": "Guitarra acustica",
    "serieProducto": "ASDJK768S",
    "marca": "Gibson",
    "categoria_id": 1,
    "precio": 150000,
    "fechaCreacion": "27-05-2023 05:48:53",
    "fechaModificacion": "00-00-0000 00:00:00",
    "estado": true
  }

PUT /Producto/ModificarProducto


la funcion de este endpoint es poder modificar un producto ya sea nombre, descripcion, precio, etc. para poder modificar una categoria debemos mandar en el reques body un json con el siguiente formato:

Ejemplo:

  URL = https://localhost:4000/Producto/ModificarProducto

  {
    "id_Producto": 1,
    "nombre": "",
    "descripcion": "Guitarra acustica Gibson",
    "serieProducto": "AS55368S",
    "marca": "",
    "categoria_id": 0,
    "precio": 0,
    "fechaCreacion": "",
    "fechaModificacion": "",
    "estado": true
  }

  Respuesta:

  {
    "resultTransaccion": true,
    "message": "El producto Guitarra fue registrado exitosamente"
  }


importante mencionar que para modificar una categoria debemos llenar el campo de id_Producto como se muestra en el ejemplo y ademas solo debemos mandar en el json los campos que deseamos modificar, en el ejemplo se muestra como solo se llanan solo el campo de descripcion y serieProducto para modificar y los demas campos se dejan vacio o en 0 ya que no se desea modificar.


DELETE /Producto/EliminarProducto

la funcion de este endpoint es poder eliminar un producto enviadno como parametro el id de dicho producto que deseamos eliminar, a continuacion un ejemplo de lo que retornaria el endpoint.

Ejemplo:

  URL = https://localhost:4000/Producto/EliminarProducto

  Respuesta:

  {
    "resultTransaccion": true,
    "message": "El producto Guitarra fue eliminado exitosamente"
  }



# 'Stock'

POST /Stock/CrearStock

la funcion de este endpoint es poder crear el stock de un producto, para crear el stock d eun producto debemos mandar como parametros el id del producto al cual se desea crear el stock y la cantidad que se desea dejar como stock inical, a continuacion un ejemplo:


Ejemplo:

  URL = https://localhost:4000/Stock/CrearStockid_producto=1&cantidad=10000

  Respuesta:

  {
    "resultTransaccion": true,
    "message": "Se ha creado stock para el producto Guitarra, stock: 10000"
  }


GET /Stock/GetStockProductos

la funcion de este enpoint es traer la lista de los productos que cuentan con un stosk registrado por el endpoint /Stock/CrearStock, a continuacion un ejemplo de lo que retornaria el endpoint.

Ejemplo:

  URL = https://localhost:4000/Stock/GetStockProductos

  Respuesta:

  [
    {
      "id_Producto": 1,
      "cantidadStock": 10000
    },
    {
      "id_Producto": 2,
      "cantidadStock": 600
    }
  ]


GET /Stock/GetStockProducto

la funcion de este endpoint es poder buscar el stock de un producto registrado por el endpoint /Stock/CrearStock pasando como parametro de entrada el id del producto que se desea buscar el stock, a continuacion un ejemplo de lo que retornaria el endpoin.

Ejemplo:

  URL = https://localhost:4000/Stock/GetStockProducto?id_producto=1

  Respuesta:

  {
    "id_Producto": 1,
    "cantidadStock": 10000
  }
  

GET /Stock/AumentarStock

la funcion de este endpoint es poder aumentar el stock de un producto registrado por el endpoint /Stock/CrearStock pasando como parametro el id del producto y la cantidad de stock que se desea aumentar, a continuacion un ejemplo de lo que retornaria el endpoint.
Ejemplo:

Ejemplo:

  URL = https://localhost:4000/Stock/AumentarStock?id_producto=1&cantidad=500

  Respuesta:

  {
    "resultTransaccion": true,
    "message": "Se aumento el stock del producto Guitarra, stock: 10500"
  }


GET /Stock/RebajarStock

la funcion de este endpoint es poder aumentar el stock de un producto registrado por el endpoint /Stock/CrearStock pasando como parametro el id del producto y la cantidad de stock que se desea aumentar, a continuacion un ejemplo de lo que retornaria el endpoint.
Ejemplo:

Ejemplo:

  URL = https://localhost:4000/Stock/RebajarStock?id_producto=1&cantidad=200

  Respuesta:

  {
    "resultTransaccion": true,
    "message": "Se rebajo el stock del producto Guitarra, stock: 10300"
  }


DELETE //Stock/EliminarStock

la funcion de este endpoint es poder eliminar el stock de un producto pasando como parametro el id del producto al cual queremos eliiminar el stock, a cpontinuacion un ejemplo de lo que retornaria el endpoint.

Ejemplo:
  URL = https://localhost:4000/Stock/EliminarStock?id_producto=1

  Respuesta:

  {
    "resultTransaccion": true,
    "message": "El stock del producto Guitarra fue eliminado exitosamente"
  }



# 'CarritoCompra'

POST /CarritoCompra/AñadirProductoCarrito

la funcion de este endpoint es poder añadir un prodcto al carrito de compras de un usuario pasando como parametros el id del usuario dueño del carrito, el id del producto que se desea añadir al carrito, y la cantidad de productos que comprara a continuacion un ejemplo de lo que retornaria el endpoint.

Ejemplo:

  URL = https://localhost:4000/CarritoCompra/AñadirProductoCarrito?id_usuario=10&id_producto=9&cantidad=2

  Respuesta:

  {
    "resultTransaccion": true,
    "message": "Producto añadido al carrito"
  }
  

POST /CarritoCompra/QuitarProductoCarrito

la funcion de este endpoint es poder quitar un prodcto del carrito de compras de un usuario pasando como parametros el id del usuario dueño del carrito, el id del producto que se desea eliminar del carrito, a continuacion un ejemplo de lo que retornaria el endpoint.

Ejemplo:

  URL = https://localhost:4000/CarritoCompra/QuitarProductoCarrito?id_usuario=10&id_producto=9

  Respuesta:

  {
    "resultTransaccion": true,
  "message": "Producto quitado del carrito"
  }
  
  
GET /CarritoCompra/GetCarritos

la funcion de este endpoint es poder buscar los carritos de todos los usuarios a continuacion un ejemplo de lo que retornaria el endpoint.

Ejemplo:

  URL = https://localhost:4000/CarritoCompra/CarritoCompra/GetCarritos

  Respuesta:

  [
    {
      "id_Carrito": 1,
      "id_usuario": 1,
      "detalleCarrito": [
        {
          "id_Carrito": 1,
          "id_Producto": 1,
          "cantidad": 3
        }
      ]
    },
    {
      "id_Carrito": 2,
      "id_usuario": 2,
      "detalleCarrito": [
        {
          "id_Carrito": 2,
          "id_Producto": 1,
          "cantidad": 2
        },
        {
          "id_Carrito": 2,
          "id_Producto": 2,
          "cantidad": 1
        }
      ]
    }
  ]


GET /CarritoCompra/GetCarrito

la funcion de este endpoint es traer el carrito de compras de un usuario pasando como parametro de entrada el id del usuario dueño del carrito en cuestio, a continuacion un ejemplo de lo que retornaria el endpoint.

Ejemplo:

  URL = https://localhost:4000/CarritoCompra/CarritoCompra/GetCarritos?id_usuario=1

  Respuesta:
  {
    "id_Carrito": 1,
    "id_usuario": 1,
    "detalleCarrito": [
      {
        "id_Carrito": 1,
        "id_Producto": 1,
        "cantidad": 3
      }
    ]
  } 
  

POST /CarritoCompra/PagarCarrito

la funcion de este endpoint es poder efectuar una compra simulada gracias a la api FakeBankApi_MSP en donde debemos tener descargado este proyecto y ejecutado para poder realizar la solicitud POST con los datos requeridos para effectuar un pago, los datos requeridos son id del usuario, Rut, Numero Tarjeta, Clavetarjeta, Cvv, NumeroCUenta, Moneda, a continuacion un ejemplo de lo que retornaria el endpoint.

Ejemplo:

  URL = https://localhost:4000/CarritoCompra/PagarCarrito?id_usuario=1&Rut_Persona=21263498-0&NumeroTarjeta=4014714023172&Clavetarjeta=1234&Cvv=560&NumeroCuenta=815993448782&Moneda=peso

  Respuesta:
  {
    mesage = "Se ha el pago de los productos en el carrito por un total de 900000"
  }

importante debemos tenr la api de FakeBankApi_MSP levantada en localhost y previamente haber creado una cuenta en el banco con una tarjeta de esta manera podremos ver reflejados los cobros realizado, el pago despues de ser efectuado habra rebajado el stock segunn la cantidad de productos comprados y eliminando el carrito de compras ya que se efectuo el pago.


DELETE /CarritoCompra/ELiminarCarrito?

la funcion de este endpoint es poder eliminar el carrito de compras de un usuario passando como paramtro el id del usuario dueño del carrito, a continuacion un ejemplo de lo que retornaria el endpoint.

Ejemplo:
  
  URL = https://localhost:4000/CarritoCompra/ELiminarCarrito?id_usuario=1

  Respuesta:
  {
    "resultTransaccion": true,
    "message": "El carrito del usaurio Juanito Alcachofa fue eliminado exitosamente"
  }



