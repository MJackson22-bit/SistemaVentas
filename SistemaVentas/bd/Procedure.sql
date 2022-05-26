

Create proc SP_REGISTRARUSUARIO(
@Documento varchar(50),
@Nombre_Completo varchar(100),
@Correo varchar(100),
@Clave varchar(100),
@IdRol int,
@Estado bit,
@IdUsuarioResultado int output,
@Mensaje varchar(500) output
)
as
begin
	set @IdUsuarioResultado = 0
	set @Mensaje = ''
	if not exists(select * from USUARIO where Documento = @Documento)
	begin
		insert into USUARIO(Documento, Nombre_Completo, Correo, Clave, IdRol, Estado) values
		(@Documento, @Nombre_Completo, @Correo, @Clave, @IdRol, @Estado)
		set @IdUsuarioResultado = SCOPE_IDENTITY()
	end
	else
		set @Mensaje = 'El documento ya existe'
end

Create proc SP_EDITARUSUARIO(
@IdUsuario int,
@Documento varchar(50),
@Nombre_Completo varchar(100),
@Correo varchar(100),
@Clave varchar(100),
@IdRol int,
@Estado bit,
@Respuesta bit output,
@Mensaje varchar(500) output
)
as
begin
	set @Respuesta = 0
	set @Mensaje = ''
	if not exists(select * from USUARIO where Documento = @Documento and IdUsuario != @IdUsuario)
	begin
		update USUARIO set
		Documento = @Documento, 
		Nombre_Completo = @Nombre_Completo, 
		Correo = @Correo, 
		Clave = @Clave, 
		IdRol = @IdRol, 
		Estado = @Estado
		where IdUsuario = @IdUsuario
		set @Respuesta = 1
	end
	else
		set @Mensaje = 'El documento ya existe'
end


Create proc SP_ELIMINARUSUARIO(
@IdUsuario int,
@Respuesta bit output,
@Mensaje varchar(500) output
)
as
begin
	set @Respuesta = 0
	set @Mensaje = ''
	declare @pasoreglas bit = 1

	IF EXISTS (SELECT * FROM COMPRA C
	INNER JOIN USUARIO U ON U.IdUsuario = C.IdUsuario
	WHERE U.IdUsuario = @IdUsuario)
	BEGIN
		SET @pasoreglas = 0
		SET @Respuesta = 0
		SET @Mensaje = 'EL USUARIO ESTÁ RELACIONADO CON UNA COMPRA'
	END
	IF EXISTS (SELECT * FROM VENTA V
	INNER JOIN USUARIO U ON U.IdUsuario = V.IdUsuario
	WHERE U.IdUsuario = @IdUsuario)
	BEGIN
		SET @pasoreglas = 0
		SET @Respuesta = 0
		SET @Mensaje = 'EL USUARIO ESTÁ RELACIONADO CON UNA VENTA'
	END
	IF(@pasoreglas = 1)
	BEGIN
		DELETE FROM USUARIO WHERE IdUsuario = @IdUsuario
		SET @Respuesta = 1
	END

end

--Procedimiento para guardar categoría
create PROC SP_REGISTRARCATEGORIA(
@Descripcion varchar(50),
@Estado bit,
@Resultado bit output,
@Mensaje varchar(500) output
)
as
begin
	SET @Resultado = 0
	IF NOT EXISTS (SELECT * FROM CATEGORIA WHERE Descripcion = @Descripcion)
	begin
		insert into CATEGORIA(Descripcion, Estado) values (@Descripcion, @Estado)
		set @Resultado = SCOPE_IDENTITY()
	end
	ELSE
		set @Mensaje = 'La descripcion de esta categoría ya existe'
end

go

--Procedimiento para editar categoría
CREATE PROC SP_EDITARCATEGORIA(
@IdCategoria int,
@Descripcion varchar(50),
@Estado bit,
@Resultado bit output,
@Mensaje varchar(500) output
)
as
begin
	SET @Resultado = 1
	IF NOT EXISTS (SELECT * FROM CATEGORIA WHERE Descripcion = @Descripcion and IdCategoria != @IdCategoria)
		update CATEGORIA set
		Descripcion = @Descripcion,
		Estado = @Estado
		where IdCategoria = @IdCategoria
	ELSE
	begin
		set @Resultado = 0
		set @Mensaje = 'La descripcion de esta categoría ya existe'
	end
end

go


--Procedimiento para Eliminar categoría
CREATE PROC SP_ELIMINARCATEGORIA(
@IdCategoria int,
@Resultado int output,
@Mensaje varchar(500) output
)
as
begin
	SET @Resultado = 1
	IF NOT EXISTS (select * from CATEGORIA c
	inner join PRODUCTO p on p.IdCategoria = c.IdCategoria
	where c.IdCategoria = @IdCategoria)
	begin
		delete top(1) from CATEGORIA where IdCategoria = @IdCategoria
	end
	ELSE
	begin
		set @Resultado = 0
		set @Mensaje = 'La categoria se encuentra relacionada a un porducto'
	end
end

Create proc SP_REGISTRARPRODUCTO(
@Codigo varchar(50),
@Nombre varchar(100),
@Descripcion varchar(100),
@IdCategoria varchar(100),
@Estado bit,
@Resultado int output,
@Mensaje varchar(500) output
)
as
begin
	set @Resultado = 0
	if not exists(select * from PRODUCTO where Codigo= @Codigo)
	begin
		insert into PRODUCTO(Codigo, Nombre, Descripcion, IdCategoria, Estado) values
		(@Codigo, @Nombre, @Descripcion, @IdCategoria, @Estado)
		set @Resultado = SCOPE_IDENTITY()
	end
	else
		set @Mensaje = 'El producto ya existe'
end
go
Create proc SP_MODIFICARPRODUCTO(
@IdProducto int,
@Codigo varchar(50),
@Nombre varchar(100),
@Descripcion varchar(100),
@IdCategoria varchar(100),
@Estado bit,
@Resultado int output,
@Mensaje varchar(500) output
)
as
begin
	set @Resultado = 1
	set @Mensaje = ''
	if not exists(select * from PRODUCTO where Codigo= @Codigo and IdProducto != @IdProducto)
		Update PRODUCTO set 
		Codigo = @Codigo, 
		Nombre = @Nombre,
		Descripcion = @Descripcion,
		IdCategoria = @IdCategoria, 
		Estado = @Estado
		WHERE IdProducto = @IdProducto
	else
	begin
	set @Resultado = 0
		set @Mensaje = 'El producto ya existe'
	end
end
go 
alter proc SP_ELIMINARPRODUCTO(
@IdProducto int,
@Respuesta bit output,
@Mensaje varchar(500) output
)
as
begin
	set @Respuesta = 0
	set @Mensaje = ''
	declare @pasoreglas bit = 1
	if exists(select * from DETALLE_COMPRA dc
	INNER JOIN PRODUCTO p ON p.IdProducto = dc.IdProducto
	WHERE p.IdProducto = @IdProducto)
	begin
		set @pasoreglas = 0
		set @Respuesta = 0
		set @Mensaje = @Mensaje + 'No se puede eliminar poeque se encuentra relacionado con una compra\n'
	end
	if exists(select * from DETALLE_VENTA dv
	INNER JOIN PRODUCTO p ON p.IdProducto = dv.IdProducto
	WHERE p.IdProducto = @IdProducto)
	begin
		set @pasoreglas = 0
		set @Respuesta = 0
		set @Mensaje = @Mensaje + 'No se puede eliminar poeque se encuentra relacionado con una venta\n'
	end
	IF(@pasoreglas = 1)
	begin
		delete from PRODUCTO where IdProducto = @IdProducto
		set @Respuesta = 1
	end
end

go

alter proc SP_REGISTRARCLIENTE(
@Documento varchar(50),
@Nombre_Completo varchar(100),
@Correo varchar(100),
@Telefono varchar(100),
@Estado bit,
@Resultado int output,
@Mensaje varchar(500) output
)as 
begin
	set @Resultado = 0
	declare @IDPERSONA int
	if not exists(select * from CLIENTE where Documento = @Documento)
	begin
		insert into CLIENTE (Documento, Nombre_Completo, Correo, Telefono, Estado) values (@Documento, @Nombre_Completo, @Correo, @Telefono, @Estado)
		set @Resultado = SCOPE_IDENTITY()
	end
	else
		set  @Mensaje = 'El cliente ya existe Create'
end

go

create proc SP_MODIFICARCLIENTE(
@IdCliente int,
@Documento varchar(50),
@Nombre_Completo varchar(100),
@Correo varchar(100),
@Telefono varchar(100),
@Estado bit,
@Resultado bit output,
@Mensaje varchar(500) output
)as 
begin
	set @Resultado = 1
	declare @IDPERSONA int
	if not exists(select * from CLIENTE where Documento = @Documento and IdCliente != @IdCliente)
	begin
		update CLIENTE set 
		Documento = @Documento, 
		Nombre_Completo = @Nombre_Completo, 
		Correo = @Correo, Telefono = @Telefono, 
		Estado = @Estado
		where IdCliente = @IdCliente
	end
	else
	begin
		set @Resultado = 0
		set  @Mensaje = 'El cliente ya existe Edit'
	end
end

create proc SP_REGISTRARPROVEEDOR(
@Documento varchar(50),
@Razon_Social varchar(100),
@Correo varchar(100),
@Telefono varchar(100),
@Estado bit,
@Resultado int output,
@Mensaje varchar(500) output
) as
begin
	set @Resultado = 0
	declare @IDPERSONA int
	if not exists(select * from PROVEEDOR where Documento = @Documento)
	begin
		insert into PROVEEDOR(Documento, Razon_Social, Correo, Telefono, Estado) values
		(@Documento, @Razon_Social, @Correo, @Telefono, @Estado)
		set @Resultado = SCOPE_IDENTITY()
	end
	else
		set @Mensaje = 'El proveedor ya existe'
end

go

create proc SP_MODIFICARPROVEEDOR(
@IdProveedor int,
@Documento varchar(50),
@Razon_Social varchar(100),
@Correo varchar(100),
@Telefono varchar(100),
@Estado bit,
@Resultado bit output,
@Mensaje varchar(500) output
) as
begin
	set @Resultado = 1
	declare @IDPERSONA int
	if not exists(select * from PROVEEDOR where Documento = @Documento and IdProveedor != @IdProveedor)
	begin
		update PROVEEDOR set Documento = @Documento, 
		Razon_Social = @Razon_Social, 
		Correo = @Correo, 
		Telefono = @Telefono, 
		Estado = @Estado
	end
	else
	begin
		set @Resultado = 0
		set @Mensaje = 'El proveedor ya existe'
	end
end
go

create proc SP_ELIMINARPROVEEDOR(
@IdProveedor int,
@Resultado int output,
@Mensaje varchar(500) output
) as
begin
	set @Resultado = 1
	declare @IDPERSONA int
	if not exists(select * from PROVEEDOR p inner join COMPRA c on p.IdProveedor = c.IdProveedor where p.IdProveedor = @IdProveedor)
	begin
		delete top(1) from PROVEEDOR where IdProveedor = @IdProveedor
	end
	else
	begin
		set @Resultado = 0
		set @Mensaje = 'El proveedor se encuentra relacionado a una compra'
	end
end
go

/*Procesos para registrar una compra*/
create type [dbo].[EDetalle_Compra] as table(
	[IdProducto] int NULL,
	[Precio_Compra] decimal(18,2) NULL,
	[Precio_Venta] decimal(18,2) NULL,
	[Cantidad] int NULL,
	[Monto_Total] decimal(18,2) NULL
)
go
create procedure SP_REGISTRARCOMPRA(
@IdUsuario int,
@IdProveedor int,
@Tipo_Documento varchar(500),
@Numero_Dcumento varchar(500),
@Monto_Total decimal(18,2),
@Detalle_Compra [EDetalle_Compra] readonly,
@Resultado bit output,
@Mensaje varchar(500) output
) as
begin
	begin try
		declare @idCompra int = 0
		set @Resultado = 1
		set @Mensaje = ''
		begin transaction registro
		insert into COMPRA(IdUsuario , IdProveedor, Tipo_Documento, Numero_Documento, Monto_Total)
		values (@IdUsuario, @IdProveedor, @Tipo_Documento, @Numero_Dcumento, @Monto_Total)

		set @idCompra = SCOPE_IDENTITY()

		insert into DETALLE_COMPRA(IdCompra,IdProducto,Precio_Compra,Precio_Venta,Cantidad,Monto_Total)
		select @idCompra, IdProducto,Precio_Compra,Precio_Venta,Cantidad,Monto_Total from @Detalle_Compra

		update p set p.Stock = p.Stock + dc.Cantidad,
		p.Precio_Compra = dc.Precio_Compra,
		p.Precio_Venta = dc.Precio_Venta
		from PRODUCTO p
		inner join @Detalle_Compra dc on dc.IdProducto = p.IdProducto

		commit transaction registro
	end try
	begin catch
		set @Resultado = 0
		set @Mensaje = ERROR_MESSAGE()
		rollback transaction registro
	end catch
end
go

/*Procesos para registrar una venta*/
create type [dbo].[EDetalle_Venta] as table(
	[IdProducto] int NULL,
	[Precio_Venta] decimal(18,2),
	[Cantidad] int NULL,
	[Sub_Total] decimal(18,2)
)
go
create procedure SP_REGISTRARVENTA(
@IdUsuario int,
@Tipo_Documento varchar(500),
@Numero_Documento varchar(500),
@Documento_Cliente varchar(500),
@Nombre_Cliente varchar(500),
@Monto_Pago decimal(18,2),
@Monto_Cambio decimal(18,2),
@Monto_Total decimal(18,2),
@Detalle_Venta [EDetalle_Venta] readonly,
@Resultado bit output,
@Mensaje varchar(500) output
) as
begin
	begin try
		declare @idVenta int = 0
		set @Resultado = 1
		set @Mensaje = ''
		begin transaction registro
		insert into VENTA(IdUsuario,Tipo_Documento, Numero_Documento, Documento_Cliente, Nombre_Cliente, Monto_Pago, Monto_Cambio, Monto_Total)
		values (@IdUsuario, @Tipo_Documento, @Numero_Documento, @Documento_Cliente, @Nombre_Cliente, @Monto_Pago, @Monto_Cambio, @Monto_Total)

		set @idVenta = SCOPE_IDENTITY()

		insert into DETALLE_VENTA(IdVenta,IdProducto,Precio_Venta,Cantidad,Sub_Total)
		select @idVenta, IdProducto,Precio_Venta,Cantidad,Sub_Total from @Detalle_Venta

		commit transaction registro
	end try
	begin catch
		set @Resultado = 0
		set @Mensaje = ERROR_MESSAGE()
		rollback transaction registro
	end catch
end