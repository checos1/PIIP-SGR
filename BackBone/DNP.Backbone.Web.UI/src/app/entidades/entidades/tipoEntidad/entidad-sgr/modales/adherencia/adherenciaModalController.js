(function () {
    'use strict';

    angular.module('backbone.entidades')
        .controller('adherenciaModalController', adherenciaModalController);

    adherenciaModalController.$inject = [
        '$scope',
        '$uibModalInstance',
        '$http',
        'constantesBackbone',
        'entidad',
        'sesionServicios',
        'backboneServicios',
        'servicioAdherencia',
        'utilidades'
    ];

    function adherenciaModalController(
        $scope,
        $uibModalInstance,
        $http,
        constantesBackbone,
        entidad,
        sesionServicios,
        backboneServicios,
        servicioAdherencia,
        utilidades
    ) {
        const vm = this;
        vm.entidad = entidad;
        vm.yaSeCargo = false;
        vm.peticionObtener;

        //#region Variables

        vm.adherencia = {
            Itens: []
        };
        vm.adherenciasEliminadas = [];
        vm.entidades = [];

        vm.tiposEntidade = ['Nacional', 'Territorial', 'SGR', 'Pública', 'Privada'];

        const _validadores = [
            (model) => {
                return model.Itens.reduce((acc, item, index) => {
                    const numero = ++index;
                    acc.push(...[
                        { invalido: !item.FechaInicio, mensaje: `Ingrese una fecha de Inicio en el item número ${numero}` },
                        { invalido: !item.FechaFin, mensaje: `Ingrese una fecha de Fin en el item número ${numero}` },
                        { invalido: !item.TipoEntidad, mensaje: `Seleccione un tipo del entidad en el item número ${numero}` },
                        { invalido: !item.AdherenciaEntidad, mensaje: `Seleccione una entidad en el item número ${numero}` },
                        { invalido: item.FechaInicio >= item.FechaFin, mensaje: `La fecha de inicio debe ser anterior a la fecha de termino ${numero}` },
                    ])

                    return acc;
                }, [])
            }
        ];

        //#endregion

        //#region Metodos

        function adicionarAdherencia() {
            vm.adherencia.Itens.push({
                AdherenciaId: 0,
                TipoEntidad: null,
                EntidadId: entidad.IdEntidad,
                AdherenciaEntidadId: null,
                FechaFin: null,
                FechaInicio: null
            });
        }

        function removerAdherencia(index) {

            if (index == null || index == undefined)
                return;

            if (vm.adherencia.Itens.length == 1) {
                toastr.error("La adherencia debe tener al menos un item");
                return;
            }

            //Agrega el registro a la lista de pendientes por eliminar
            vm.adherenciasEliminadas.push(vm.adherencia.Itens[index]);
            //Elimina el registro de la colección del grid.
            vm.adherencia.Itens.splice(index, 1);
            return;
        }

        function _validarAdherencia(model) {
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
                toastr.error("Error inesperado al validar la Adherencia");
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

        function esRegistroNuevo(adherencia) {
            return adherencia.AdherenciaId === 0;
        }

        async function guardar() {
            const valido = _validarAdherencia(vm.adherencia);
            let exito = true;
            if (!valido)
                return;

            //Nuevos y modificaciones
            for (let i = 0; i < vm.adherencia.Itens.length; i++) {
                try {
                    vm.adherencia.Itens[i].AdherenciaEntidadId = vm.adherencia.Itens[i].AdherenciaEntidad.Id;
                    let result = await servicioAdherencia.guardarAdherencia(vm.adherencia.Itens[i]);
                    if (result.data.Exito) {
                        if (esRegistroNuevo(vm.adherencia.Itens[i])) {
                            vm.adherencia.Itens[i].AdherenciaId = +result.data.IdRegistro; //Actualiza el Id
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
            for (let i = 0; i < vm.adherenciasEliminadas.length;) {
                try {
                    let result = await servicioAdherencia.eliminarAdherencia(vm.adherenciasEliminadas[i]);
                    if (result.data.Exito) {
                        //Quita el registro de los pendientes de eliminar
                        vm.adherenciasEliminadas.splice(i, 1);
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
                    //toastr.error(`Eliminar registro: ${err.data.ExceptionMessage}`);
                    toastr.error("Ocurrió un error al intentar eliminar el registro");
                }
            }

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

        async function listarEntidades() {
            let entidadesObtenidas = [];
            try {
                vm.entidades = [];

                ////Cargar las entidades por tipo de entidad
                //for (let i = 0; i < vm.tiposEntidade.length; i++) {
                //    entidadesObtenidas = await servicioAdherencia.obtenerEntidades(vm.peticionObtener, vm.tiposEntidade[i]);
                //    vm.entidades.push(...entidadesObtenidas.data.map(x => ({ Id: x.IdEntidad, NombreEntidad: x.Entidad, TipoEntidad: x.TipoEntidad })));
                //}

                entidadesObtenidas = await servicioAdherencia.obtenerEntidades(vm.peticionObtener, "General");
                vm.entidades.push(...entidadesObtenidas.data.map(x => ({ Id: x.IdEntidad, NombreEntidad: x.Entidad, TipoEntidad: x.TipoEntidad })));

                //Eliminar la entidad seleccionada
                let index = vm.entidades.findIndex(x => x.Id === entidad.IdEntidad);
                vm.entidades.splice(index, 1);

                //Ordenar la lista
                vm.entidades.sort((a, b) => {
                    let aSinAcento = a.NombreEntidad.toLowerCase().replace("á", "a").replace("é", "e").replace("í", "i").replace("ó", "o").replace("ú", "u");
                    let bSinAcento = b.NombreEntidad.toLowerCase().replace("á", "a").replace("é", "e").replace("í", "i").replace("ó", "o").replace("ú", "u");
                    if (aSinAcento < bSinAcento) {
                        return -1;
                    }
                    if (aSinAcento > bSinAcento) {
                        return 1;
                    }
                    return 0;
                })
            }
            catch (err) {
                console.log(err);
            }
        }

        function crearPrimerAdherecia() {
            vm.adherencia = {
                Itens: [
                    {
                        AdherenciaId: 0,
                        TipoEntidad: null,
                        EntidadId: entidad.IdEntidad,
                        AdherenciaEntidadId: null,
                        FechaFin: null,
                        FechaInicio: null
                    }
                ]
            };
        }

        function obtenerEntidadDeCatalogo(idEntidad) {
            let encontrado = vm.entidades.find(x => x.Id === idEntidad);
            if (encontrado)
                return encontrado;
            return null;
        }

        function obtenerAdherencia() {
            servicioAdherencia.obtenerAdherenciasPorEntidadId(entidad.IdEntidad).then(exito, error);

            function exito(respuesta) {
                if (respuesta.data && respuesta.data.length > 0) {
                    vm.adherencia = {
                        Itens: respuesta.data.map(x => ({
                            AdherenciaId: x.AdherenciaId,
                            TipoEntidad: x.TipoEntidad,
                            AdherenciaEntidad: obtenerEntidadDeCatalogo(x.AdherenciaEntidadId),
                            FechaInicio: new Date(x.FechaInicio),
                            FechaFin: new Date(x.FechaFin)
                        }))
                    };
                }
                else {
                    vm.crearPrimerAdherecia();
                }
            }

            function error(err) {
                vm.crearPrimerAdherecia();
            }
        }

        async function init() {
            if (!vm.yaSeCargo) {
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
        }

        async function activar() {
            vm.yaSeCargo = true;
            try {
                await listarEntidades();
                obtenerAdherencia();
            }
            catch (err) {
                console.log(err);
            }
        }

        vm.init = init;
        vm.adicionarAdherencia = adicionarAdherencia;
        vm.removerAdherencia = removerAdherencia;
        vm.guardar = guardar;
        vm.cerrar = cerrar;
        vm.crearPrimerAdherecia = crearPrimerAdherecia;
        //#endregion
    }
})();