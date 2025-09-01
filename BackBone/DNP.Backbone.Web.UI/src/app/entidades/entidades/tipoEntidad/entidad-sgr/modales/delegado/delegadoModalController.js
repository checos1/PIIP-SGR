(function () {
    'use strict';

    angular.module('backbone.entidades')
        .controller('delegadoModalController', delegadoModalController);

    delegadoModalController.$inject = [
        '$scope',
        '$uibModalInstance',
        '$filter',
        'entidad',
        'sesionServicios',
        'backboneServicios',
        'servicioDelegado',
        'utilidades',
        'archivoServicios',
        'FileSaver'
    ];

    function delegadoModalController(
        $scope,
        $uibModalInstance,
        $filter,
        entidad,
        sesionServicios,
        backboneServicios,
        servicioDelegado,
        utilidades,
        archivoServicios,
        FileSaver
    ) {
        const vm = this;
        vm.yaSeCargo = false;
        vm.idAplicacion = "delegados";
        vm.entitySeleccionadaParaSubirArchivo = "";

        //#region Variables
        vm.nombreEntidad = entidad.NombreCompleto;
        vm.model = {
            Delegados: [{
                DelegadoId: 0,
                EntidadId: entidad.IdEntidad,
                UsuarioId: null,
                IdUsuarioDnp: null,
                Nombre: null,
                Apodo: "",
                TipoDelegado: null,
                SubTipoDelegado: null,
                Fecha: null
            }
            ],
        };
        vm.Usuarios = [];
        vm.delegadosEliminados = [];

        vm.tiposDelegado = [{ Id: 1, Nombre: 'Delegado' }, { Id: 2, Nombre: 'Secretario' }, { Id: 3, Nombre: 'Presidente' }, { Id: 4, Nombre: 'Invitado' }];
        vm.subTiposDelegado = ['Nacional', 'Departamental', 'Municipal', 'Minorías étnicas', 'Cámara de representantes'];

        const _validadores = [
            (model) => {
                return model.Delegados.reduce((acc, delegado, index) => {
                    const numero = ++index;
                    acc.push(...[
                        { invalido: !delegado.IdUsuarioDnp, mensaje: `Ingrese un identificador en el delegado número ${numero}` },
                        { invalido: !delegado.Nombre, mensaje: `Ingrese un Nombre en el delegado número ${numero}` },
                        { invalido: !delegado.TipoDelegado, mensaje: `Seleccione un Tipo en el delegado número ${numero}` },
                        { invalido: !delegado.SubTipoDelegado, mensaje: `Seleccione un SubTipo en el delegado número ${numero}` },
                        { invalido: !delegado.Fecha, mensaje: `Ingrese una Fecha en el delegado número ${numero}` }
                    ])

                    return acc;
                }, [])
            }
        ];

        //#endregion

        //#region Metodos

        function adicionarDelegado() {
            vm.model.Delegados.push({
                DelegadoId: 0,
                EntidadId: entidad.IdEntidad,
                UsuarioId: null,
                IdUsuarioDnp: null,
                Nombre: null,
                Apodo: null,
                Tipo: null,
                SubTipo: null,
                Fecha: null,
            });
        }

        function removerDelegado(index) {

            if (index == null || index == undefined)
                return;

            if (vm.model.Delegados.length == 1) {
                toastr.error("Debe tener al menos un delegado");
                return;
            }

            //Si es un registro nuevo, no se guarda en los pendientes de eliminación
            if (!esRegistroNuevo(vm.model.Delegados[index]))
                vm.delegadosEliminados.push(vm.model.Delegados[index]);
            //Elimina el registro de la colección
            vm.model.Delegados.splice(index, 1);
            return;
        }

        //TODO: Implementar
        function editarDelegado() {
            alert('method not implemented');
        }

        function _validarDelegados(model) {
            try {
                const mensajes = [];
                _validadores.forEach(validator => {
                    validator(model).forEach(resultado => {
                        if (resultado.invalido)
                            mensajes.push(resultado.mensaje);
                    });
                })

                if (mensajes.length)
                    _mostarToast(mensajes);

                return !mensajes.length;
            }
            catch (e) {
                console.log(e);
                toastr.error("Error inesperado al validar");
                return false;
            }
        }

        function _mostarToast(toasMessages = []) {
            toasMessages.forEach(message => {
                if (!message)
                    return;

                toastr.warning(message);
            })
        }

        function obtenerDelegados() {
            servicioDelegado.obtenerDelegadosPorEntidadId(entidad.IdEntidad).then(exito, error);

            function exito(respuesta) {
                if (respuesta.data && respuesta.data.length > 0) {
                    

                    vm.model = {
                        Delegados: respuesta.data.map(p => ({
                            DelegadoId: p.DelegadoId,
                            EntidadId: p.EntidadId,
                            UsuarioId: p.UsuarioId,
                            IdUsuarioDnp: BuscarDatoUsuario(p.UsuarioId, "IdUsuarioDnp"),
                            Nombre: retornaNombre(p.UsuarioId),
                            Apodo: retornaApellido(p.UsuarioId),
                            TipoDelegado: p.TipoDelegado,
                            SubTipoDelegado: p.SubtipoDelegado,
                            Fecha: new Date(p.Fecha),
                            IdArchivoBlob: p.IdArchivoBlob,
                            IdArchivo: p.IdArchivo
                        }))
                    };
                }
            }

            function error(err) {
                console.log(err);
            }
        }

        function retornaNombre(usuarioId) {
            let nombreCompleto = BuscarDatoUsuario(usuarioId, "Nombre");
            let nombre = "", apellido = "";
            if (nombreCompleto) {
                let split = [];
                split = nombreCompleto.split(" ");
                nombre = split[0];
                return nombre;
                //if (split.length > 1) {
                //    apellido = split.slice(1, split.length).join(' ');
                //}
            }
            return null;
        }

        function retornaApellido(usuarioId) {
            let nombreCompleto = BuscarDatoUsuario(usuarioId, "Nombre");
            let nombre = "", apellido = "";
            if (nombreCompleto) {
                let split = [];
                split = nombreCompleto.split(" ");
                //nombre = split[0];
                if (split.length > 1) {
                    apellido = split.slice(1, split.length).join(' ');
                    return apellido;
                }
            }
            return null;
        }

        function BuscarDatoUsuario(idUsuario, propiedad) {
            let usr = vm.Usuarios.find(p => p.IdUsuario === idUsuario);
            if (usr) {
                return usr[propiedad];
            }
        }

        async function guardar() {
            const valido = _validarDelegados(vm.model);
            if (!valido)
                return;

            let exito = true;

            //Nuevos y modificaciones
            for (let i = 0; i < vm.model.Delegados.length; i++) {
                try {
                    let result = await servicioDelegado.guardarDelegado(vm.model.Delegados[i]);
                    if (result.data.Exito) {
                        if (esRegistroNuevo(vm.model.Delegados[i])) {
                            vm.model.Delegados[i].DelegadoId = +result.data.IdRegistro; //Actualiza el Id
                        }
                    }
                    else {
                        exito = false;
                        toastr.error(`No se puedo guardar el registro ${i + 1}`);
                    }
                }
                catch (err) {
                    exito = false;
                    let mensaje = `No se pudo guardar el registro ${i + 1} : ${err.data.ExceptionMessage}`;
                    toastr.error(mensaje);
                }
            }

            //Eliminaciones
            for (let i = 0; i < vm.delegadosEliminados.length;) {
                try {
                    let result = await servicioDelegado.eliminarDelegado(vm.delegadosEliminados[i]);
                    if (result.data.Exito) {
                        //Quita el registro de los pendientes de eliminar
                        vm.delegadosEliminados.splice(i, 1);
                    }
                    else {
                        //Marca el guardado como no exitoso e informa que hubo un error al eliminar un registro.
                        exito = false;
                        toastr.error('Ocurrió un error durante la eliminación de un registro');
                    }
                }
                catch (err) {
                    //Pasa a la siguiente adherencia por eliminar
                    i++;
                    exito = false;
                    toastr.error("Ocurrió un error al intentar eliminar el registro");
                }
            }

            imprimirResultadoGlobalGuardar(exito);
        }

        function imprimirResultadoGlobalGuardar(exito) {
            if (exito) {
                utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                vm.cerrar()
                //toastr.success("Datos guardados");
            }
            else {
                toastr.error("Hubieron algunos errores al guardar");
            }
        }

        function cerrar() {
            $uibModalInstance.close(false);
        }

        function buscarUsuario(item) {
            let usuario = vm.Usuarios.find(p => p.IdUsuarioDnp === item.IdUsuarioDnp);
            if (usuario) {
                let split = [];
                let nombre = "", apellido = "";
                split = usuario.Nombre.split(" ");
                nombre = split[0]
                if (split.length > 1) {
                    apellido = split.slice(1, split.length).join(' ');
                }

                item.UsuarioId = usuario.IdUsuario;
                item.Nombre = nombre;
                item.Apodo = apellido;
            }
            else {
                toastr.error("No se encuentra un usuario con ese Id");
                item.Nombre = "";
            }
        }

        function esRegistroNuevo(delegado) {
            return delegado.DelegadoId === 0;
        }

        async function init() {
            if (!vm.yaSeCargo) {
                try {
                    var roles = sesionServicios.obtenerUsuarioIdsRoles();

                    if (backboneServicios.estaAutorizado() && roles != null && roles.length > 0) {
                        vm.peticionObtener = {
                            IdUsuario: usuarioDNP,
                            IdObjeto: idTipoProyecto,
                            Aplicacion: nombreAplicacionBackbone,
                            ListaIdsRoles: roles
                        };

                        activar(); //Realizar la carga
                    }
                }
                catch (err) {
                    console.log(err);
                }
            }
        }

        async function activar() {
            try {
                let data = await servicioDelegado.obtenerUsuarios();
                vm.Usuarios = data.data;

                //cargar entidades
                //let entidades = await servicioDelegado.obtenerEntidades();

                //Obtener los delegados
                await obtenerDelegados();
                vm.yaSeCargo = true;
            }
            catch (err) {
                console.log(err);
            }
        }

        function enviarDocumento(entity) {
            if (!entity.DelegadoId) {
                toastr.warning('Primero guarde el registro antes de enviar un archivo');
                return;
            }
            vm.entitySeleccionadaParaSubirArchivo = entity;
            document.getElementById("file").click();
        }

        function validarExtension(extension) {
            switch (extension.toLowerCase()) {
                case 'exe': case 'bin': case 'src': case 'vbs': return false;
                default: return true;
            }
        }

        function obtenerExtension(nombreArchivo) {
            let partes = nombreArchivo.split('.');
            return partes[partes.length - 1];
        }

        vm.cargarArchivo = function () {
            let fileControl = document.getElementById("file");
            vm.extension = obtenerExtension(fileControl.files[0].name);
            let archivo = {
                FormFile: fileControl.files[0],
                Nombre: fileControl.files[0].name,
                Metadatos: {
                    extension: vm.extension
                }
            };

            if (!validarExtension(vm.extension)) {
                toastr.warning('Extensión no permitida');
                return;
            }
            vm.contentType = fileControl.files[0].type;
            archivoServicios.cargarArchivo(archivo, vm.idAplicacion).then(function (response) {
                if (response === undefined || typeof response === 'string') {
                    vm.mensajeError = response;
                    utilidades.mensajeError(response);
                } else {
                    vm.entitySeleccionadaParaSubirArchivo.IdArchivoBlob = response[0].idArchivoBlob;
                    vm.entitySeleccionadaParaSubirArchivo.IdArchivo = response[0].idArchivo;
                    servicioDelegado.guardarDelegado(vm.entitySeleccionadaParaSubirArchivo).then(result => {
                        if (result.data.Exito) {
                            utilidades.mensajeSuccess($filter('language')('ArchivoGuardado'), false, false, false);
                        }
                        else {
                            utilidades.mensajeError(response.data.Mensaje, false);
                        }
                    }, error => {
                        utilidades.mensajeError("Ocurrió un error al intentar actualizar la información del delegado", false);
                    })
                }
            }, error => {
                console.log(error);
            });
        };

        function visualizar(entity) {
            if (entity.IdArchivo) {
                //Consultar los metadatos del archivo
                archivoServicios.obtenerArchivoInfo(entity.IdArchivo, vm.idAplicacion).then(result => {
                    descargarArchivo({
                        IdArchivoBlob: entity.IdArchivoBlob,
                        ContenType: result.metadatos.contenttype,
                        NombreCompleto: `comprobante.${result.metadatos.extension}`
                    });
                }, error => {
                    toastr.warning("Ocurrió un error al consultar la información del archivo");
                })
            }
            else {
                toastr.warning("No hay archivo para descargar");
            }
        }

        /**
         * Permite descargar el archivo seleccionado
         * @param {object} entity
         */
        function descargarArchivo(entity) {
            archivoServicios.obtenerArchivoBytes(entity.IdArchivoBlob, vm.idAplicacion).then(function (retorno) {
                const blob = utilidades.base64toBlob(retorno, entity.ContenType);
                //const blob = new Blob([retorno], { type: entity.ContenType });
                FileSaver.saveAs(blob, entity.NombreCompleto);
            }, function (error) {
                toastr.error("Error inesperado al descargar");
            });
        }

        vm.init = init;
        vm.adicionarDelegado = adicionarDelegado;
        vm.removerDelegado = removerDelegado;
        vm.editarDelegado = editarDelegado;
        vm.enviarDocumento = enviarDocumento;
        vm.visualizar = visualizar;
        vm.guardar = guardar;
        vm.cerrar = cerrar;
        vm.buscarUsuario = buscarUsuario;
        //#endregion
    }
})();