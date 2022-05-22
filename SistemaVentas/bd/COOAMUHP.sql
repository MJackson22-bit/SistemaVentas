
Create database COOAMUHP_RL

GO

use COOAMUHP_RL

GO

Create table ROL(
IdRol int primary key identity,
Descripcion varchar (100),
Fecha_Registro datetime default getdate()
);

GO

Create table PERMISO(
IdPermiso int primary key identity,
IdRol int references ROL (IdRol),
Nombre_Menu varchar(100),
Fecha_Registro datetime default getdate()
);

GO

Create table PROVEEDOR(
IdProveedor int primary key identity,
Documento varchar(100),
Razon_Social varchar(100),
Correo varchar(100),
Telefono nvarchar(100),
Estado bit,
Fecha_Registro datetime default getdate()
);

GO

Create table CLIENTE(
IdCliente int primary key identity,
Documento varchar(100),
Nombre_Completo varchar(100),
Correo varchar (100),
Telefono nvarchar(100),
Estado bit,
Fecha_Registro datetime default getdate()
);

GO

Create table USUARIO(
IdUsuario int primary key identity,
Documento varchar(100),
Nombre_Completo varchar(100),
Correo varchar (100),
Clave Varchar(100),
IdRol int references ROL(IdRol),
Estado bit,
Fecha_Registro datetime default getdate()
);

GO

Create table CATEGORIA(
IdCategoria int primary key identity,
Descripcion varchar(100),
Estado bit,
Fecha_Registro datetime default getdate()
);

GO

Create table PRODUCTO(
IdProducto int primary key identity,
Codigo nvarchar(100),
Nombre varchar(100),
Descripcion varchar(100),
IdCategoria int references CATEGORIA(IdCategoria),
Stock int not null default 0,
Precio_Compra decimal (10,2) default 0,
Precio_Venta decimal (10,2) default 0,
Estado bit,
Fecha_Registro datetime default getdate(),
);

GO

Create table COMPRA(
IdCompra int primary key identity,
IdUsuario int references USUARIO(IdUsuario),
IdProveedor int references PROVEEDOR(IdProveedor),
Tipo_Documento varchar(100),
Numero_Documento nvarchar(100),
Monto_Total decimal(10,2),
Fecha_Registro datetime default getdate()
);

GO

Create table DETALLE_COMPRA(
IddetalleCompra int primary key identity,
IdCompra int references COMPRA(IdCompra),
IdProducto int references PRODUCTO(IdProducto),
Precio_Compra decimal (10,2) default 0,
Precio_Venta decimal (10,2) default 0,
Cantidad int,
Monto_Total decimal (10,2),
Fecha_Registro datetime default getdate()
);

GO

Create table VENTA(
IdVenta int primary key identity,
IdUsuario int references USUARIO(IdUsuario),
Tipo_Documento varchar(100),
Numero_Documento nvarchar(100),
Documento_Cliente varchar(100),
Nombre_Cliente varchar(100),
IdProveedor int references PROVEEDOR(IdProveedor),
Monto_Pago decimal(10,2),
Monto_Cambio decimal(10,2),
Monto_Total decimal(10,2),
Fecha_Registro datetime default getdate(),
);

GO

Create table DETALLE_VENTA(
IddetalleVenta int primary key identity,
IdVenta int references VENTA(IdVenta),
IdProducto int references PRODUCTO(IdProducto),
Precio_Venta decimal (10,2),
Cantidad int,
Sub_Total decimal (10,2),
Fecha_Registro datetime default getdate(),
);

GO
