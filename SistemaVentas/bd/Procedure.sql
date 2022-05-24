

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
CREATE PROC SP_REGISTRARCATEGORIA(
@Descripcion varchar(50),
@Resultado int output,
@Mensaje varchar(500) output
)
as
begin
	SET @Resultado = 0
	IF NOT EXISTS (SELECT * FROM CATEGORIA WHERE Descripcion = @Descripcion)
	begin
		insert into CATEGORIA(Descripcion) values (@Descripcion)
		set @Resultado = SCOPE_IDENTITY()
	end
	ELSE
		set @Mensaje = 'La descripcion de esta categoría ya existe'
end

--Procedimiento para editar categoría
CREATE PROC SP_EDITARCATEGORIA(
@IdCategoria int,
@Descripcion varchar(50),
@Resultado int output,
@Mensaje varchar(500) output
)
as
begin
	SET @Resultado = 1
	IF NOT EXISTS (SELECT * FROM CATEGORIA WHERE Descripcion = @Descripcion and IdCategoria != @Descripcion)
	begin
		update CATEGORIA set
		Descripcion = @Descripcion
		where IdCategoria = @IdCategoria
		set @Resultado = SCOPE_IDENTITY()
	end
	ELSE
	begin
		set @Resultado = 0
		set @Mensaje = 'La descripcion de esta categoría ya existe'
	end
end