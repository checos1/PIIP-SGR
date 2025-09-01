(function () {
    'use strict';

    angular.module('backbone.entidades')
        .controller('flujosViabilidadModalController', flujosViabilidadModalController);

    flujosViabilidadModalController.$inject = [
        '$scope',
        '$uibModalInstance',
        'entidad',
        'servicioEntidades',
        'utilidades'
    ];

    function flujosViabilidadModalController(
        $scope,
        $uibModalInstance,
        entidad,
        servicioEntidades,
        utilidades
    ) {
        const vm = this;

        //#region Variables
        vm.entidad = entidad;

        vm.flujo = {
            idEntidad: vm.entidad.idEntidad,
            nombre: vm.entidad.nombreCompleto,
            tipoEntidad: vm.entidad.tipoEntidad,
            itens: []
        };

        vm.adicionarFlujo = adicionarFlujo;
        vm.removerFlujo = removerFlujo;
        vm.guardar = guardar;
        vm.cerrar = cerrar;

        const _validadores = [
            (model) => {
                return model.itens.reduce((acc, item, index) => {
                    const numero = ++index;
                    acc.push(...[
                        { invalido: !item.tipoId, mensaje: `Seleccione un tipo flujo en el item número ${numero}` },
                        { invalido: !item.faseGuid, mensaje: `Seleccione una fase en el item número ${numero}` },
                        { invalido: !item.idCRType, mensaje: `Seleccione un CRProyecto en el item número ${numero}` },
                        { invalido: !item.idSector, mensaje: `Seleccione un sector en el item número ${numero}` },
                        { invalido: !item.idRol, mensaje: `Seleccione una role en el item número ${numero}` },
                        { invalido: !item.idEntidadEjecutora, mensaje: `Seleccione una Entidad Ejecutora en el item número ${numero}, EntityTypeCatalogOptionId falta en la entidad destino` },
                    ])

                    return acc;
                }, [])
            }
        ];

        //#endregion

        //#region Metodos
        function adicionarFlujo() {
            vm.flujo.itens.push({
                tipoId: null,
                faseGuid: null,
                idCRType: null,
                idSector: null,
                idEntidadEjecutora: null,
                idRol: null,
            });
        }

        function removerFlujo(index) {

            if (index == null || index == undefined)
                return;

            if (vm.flujo.itens.length == 1) {
                toastr.warning("La entidad debe tener al menos un flujo");
                return;
            }

            vm.flujo.itens.splice(index, 1);
            return;
        }

        function _validarFlujos(model) {
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
                toastr.error("Error inesperado al validar la flujos de viabilidad");
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

        function guardar() {
            const valido = _validarFlujos(vm.flujo);
            if (!valido)
                return;

            let flujos = []

            vm.flujo.itens.forEach(item => {
                let sector = vm.entidad.listaSector.find(x => x.SectorNegocioId == item.idSector);

                flujos.push({
                    EntidadResponsableId: vm.entidad.entityTypeCatalogOptionId,
                    EntidadResponsable: vm.entidad.nombreCompleto,
                    SectorId: sector.SectorNegocioId,
                    Sector: sector.Nombre,
                    RolId: item.idRol,
                    Rol: vm.entidad.listaRoles.find(x => x.IdRol.indexOf(item.idRol) != -1).Nombre,
                    EntidadDestinoAccionId: item.idEntidadEjecutora,//vm.entidad.listaEntidades.find(x => x.IdEntidad.indexOf(item.idEntidadEjecutora) != -1).EntityTypeCatalogOptionId,
                    EntidadDestinoAccion: vm.entidad.listaEntidades.find(x => x.EntityTypeCatalogOptionId == item.idEntidadEjecutora).Entidad,
                    CRTypeId: item.idCRType,
                    FaseGuid: item.faseGuid,
                    TipoFlujo: item.tipoId
                })
            });

            servicioEntidades.guardarFlujo(flujos)
                .then(function (response) {
                    if (response.data.Exito) {
                        utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                        vm.cerrar()
                    } else
                        utilidades.mensajeError(response.data.Mensaje, false);
                },
                    function (error) {
                        if (error) {
                            utilidades.mensajeError(error.data.Mensaje, false);
                        }
                    }
                )

        }

        function cerrar() {
            $uibModalInstance.close(false);
        }

        /// Comienzo
        vm.$onInit = function () {

            if (vm.entidad.entityTypeCatalogOptionId != null) {
                servicioEntidades.obtenerMatrizFlujo(vm.entidad.entityTypeCatalogOptionId)
                    .then(function (response) {
                        if (response.data) {
                            response.data.forEach(item => {
                                vm.flujo.itens.push(
                                    {
                                        tipoId: item.TipoFlujo,
                                        faseGuid: item.FaseGuid,
                                        idCRType: item.CRTypeId,
                                        idSector: item.SectorId,
                                        idRol: item.RolId,
                                        idEntidadEjecutora: item.EntidadDestinoAccionId,
                                    });

                            });
                            adicionarFlujo();
                        } else
                            adicionarFlujo();
                    });
            } else
                adicionarFlujo();
        }
        //#endregion
    }
})();