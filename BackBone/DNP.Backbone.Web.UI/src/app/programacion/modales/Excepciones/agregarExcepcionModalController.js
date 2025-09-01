(function () {
    'use strict';

    angular.module('backbone')
        .controller('agregarExcepcionModalController', agregarExcepcionModalController);

    agregarExcepcionModalController.$inject = [
        '$scope',
        '$uibModalInstance',
        'servicioAgregarExcepcionModal',
        'IdProgramacion',
        'TipoEntidad',
        'utilidades',
        'Cerrado'
    ];

    function agregarExcepcionModalController(
        $scope,
        $uibModalInstance,
        servicioAgregarExcepcionModal,
        IdProgramacion,
        TipoEntidad,
        utilidades,
        Cerrado
    ) {
        const vm = this;

        //#region Variables
        vm.tipoEntidad = TipoEntidad;
        vm.cerrado = Cerrado;

        vm.excepciones = {
            Itens: [
                {
                    IdProgramacionExcepcion: 0,
                    IdProgramacion: IdProgramacion,
                    EntidadId: null,
                    FechaDesde: null,
                    FechaHasta: null,
                    
                }
            ]
        };
        vm.entidades = [
        ];
        vm.eliminados = [];

        const _validadores = [
            (model) => {
                return model.Itens.reduce((acc, excepcion, index) => {
                    const numero = ++index;
                    acc.push(...[
                        { invalido: !excepcion.EntidadId, mensaje: `Seleccione una entidad para la excepción número ${numero}` },
                        { invalido: excepcion.EntidadId < 0, mensaje: `Seleccione una Entidad para la excepción número ${numero}, EntityTypeCatalogOptionId falta en la entidad seleccionada` },
                        { invalido: !excepcion.FechaDesde, mensaje: `Seleccione la fecha desde para la excepción número ${numero}` },
                        { invalido: !excepcion.FechaHasta, mensaje: `Seleccione la fecha hasta para la excepción número ${numero}` },
                        { invalido: excepcion.FechaDesde >= excepcion.FechaHasta, mensaje: `La fecha de inicio debe ser anterior a la fecha de termino ${numero}` }
                    ])

                    return acc;
                }, [])
            }
        ];

        //#endregion

        //#region Metodos

        function adicionarExcepcion() {
            vm.excepciones.Itens.push({
                IdProgramacionExcepcion: 0,
                IdProgramacion: IdProgramacion,
                EntidadId: null,
                FechaDesde: null,
                FechaHasta: null,
            });
        }

        function removerExcepcion(index) {

            if (index == null || index == undefined)
                return;

            if (vm.excepciones.Itens.length == 1) {
                toastr.error("Debe tener al menos un item");
                return;
            }

            //Si es un registro nuevo, no se guarda en los pendientes de eliminación
            if (!esRegistroNuevo(vm.excepciones.Itens[index]))
                vm.eliminados.push(vm.excepciones.Itens[index]);

            //Elimina el registro de la colección
            vm.excepciones.Itens.splice(index, 1);
            return;
        }

        function _mostarToast(toasMessages = []) {
            toasMessages.forEach(message => {
                if (!message)
                    return;

                toastr.warning(message);
            })
        }

        function _validar(model) {
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


        async function guardar() {
            const valido = _validar(vm.excepciones);
            if (!valido)
                return;

            let exito = true;

            //Nuevos y modificaciones
            for (let i = 0; i < vm.excepciones.Itens.length; i++) {
                try {
                    let result = {};
                    let excepcion = vm.excepciones.Itens[i];
                    if (vm.excepciones.Itens[i].IdProgramacionExcepcion === 0) {
                        let fecha = new Date(excepcion.FechaDesde.getFullYear(), excepcion.FechaDesde.getMonth(), excepcion.FechaDesde.getDate(), excepcion.FechaDesde.getHours(), excepcion.FechaDesde.getMinutes(), excepcion.FechaDesde.getSeconds());
                        result = await servicioAgregarExcepcionModal.guardar({
                            IdProgramacionExcepcion: excepcion.IdProgramacionExcepcion,
                            IdProgramacion: IdProgramacion,
                            EntidadId: excepcion.EntidadId,
                            FechaDesde: new Date(Date.UTC(excepcion.FechaDesde.getFullYear(), excepcion.FechaDesde.getMonth(), excepcion.FechaDesde.getDate(), excepcion.FechaDesde.getHours(), excepcion.FechaDesde.getMinutes(), excepcion.FechaDesde.getSeconds())),
                            FechaHasta: new Date(Date.UTC(excepcion.FechaHasta.getFullYear(), excepcion.FechaHasta.getMonth(), excepcion.FechaHasta.getDate(), excepcion.FechaHasta.getHours(), excepcion.FechaHasta.getMinutes(), excepcion.FechaHasta.getSeconds())),
                        });
                    }
                    else {
                        result = await servicioAgregarExcepcionModal.modificar({
                            IdProgramacionExcepcion: excepcion.IdProgramacionExcepcion,
                            IdProgramacion: IdProgramacion,
                            EntidadId: excepcion.EntidadId,
                            FechaDesde: new Date(Date.UTC(excepcion.FechaDesde.getFullYear(), excepcion.FechaDesde.getMonth(), excepcion.FechaDesde.getDate(), excepcion.FechaDesde.getHours(), excepcion.FechaDesde.getMinutes(), excepcion.FechaDesde.getSeconds())),
                            FechaHasta: new Date(Date.UTC(excepcion.FechaHasta.getFullYear(), excepcion.FechaHasta.getMonth(), excepcion.FechaHasta.getDate(), excepcion.FechaHasta.getHours(), excepcion.FechaHasta.getMinutes(), excepcion.FechaHasta.getSeconds())),
                        });
                    }
                    if (result.data.Exito) {
                        if (esRegistroNuevo(vm.excepciones.Itens[i])) {
                            vm.excepciones.Itens[i].IdProgramacionExcepcion = +result.data.IdRegistro; //Actualiza el Id
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
            for (let i = 0; i < vm.eliminados.length;) {
                try {
                    let result = await servicioAgregarExcepcionModal.eliminar(vm.eliminados[i]);
                    if (result.data.Exito) {
                        //Quita el registro de los pendientes de eliminar
                        vm.eliminados.splice(i, 1);
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
            }
            else {
                toastr.error("Hubieron algunos errores al guardar");
            }
        }

        function esRegistroNuevo(excepcion) {
            return excepcion.IdProgramacionExcepcion === 0;
        }


        function cerrar() {
            $uibModalInstance.close(false);
        }

        function obtenerExcepciones() {
            servicioAgregarExcepcionModal.obtenerExcepciones(IdProgramacion).then(exito, error);

            function exito(respuesta) {
                if (respuesta.data && respuesta.data.length > 0) {

                    vm.excepciones = {
                        Itens: respuesta.data.map(p => ({
                            IdProgramacionExcepcion: p.IdProgramacionExcepcion,
                            IdProgramacion: p.IdProgramacion,
                            EntidadId: p.EntidadId,
                            FechaDesde: p.FechaDesde ? new Date(p.FechaDesde) : null,
                            FechaHasta: p.FechaHasta ? new Date(p.FechaHasta) : null,
                        }))
                    };
                }
            }

            function error(err) {
                console.log(err);
            }
        }

        function obtenerEntidades() {
            servicioAgregarExcepcionModal.obtenerEntidades(TipoEntidad).then(result => {
                vm.entidades = result.data.map(p => {
                    if (p.EntityTypeCatalogOptionId == null)
                        return { Id: -1, NombreEntidad: p.Entidad }
                    else
                        return { Id: p.EntityTypeCatalogOptionId, NombreEntidad: p.Entidad }
                });
            });
        }

        async function init() {
            obtenerExcepciones();
            obtenerEntidades();
        }

        vm.init = init;
        vm.guardar = guardar;
        vm.cerrar = cerrar;
        vm.adicionarExcepcion = adicionarExcepcion;
        vm.removerExcepcion = removerExcepcion;

        //#endregion
    }
})();