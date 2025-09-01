(function () {
    'use strict';
    angular.module('backbone').controller('agregarActividadModalController', agregarActividadModalController);
    agregarActividadModalController.$inject = [
        '$uibModalInstance',
        'utilidades',
        'Nivel',
        'Configuracion','agregarActividadModalServicio'
    ];

    function agregarActividadModalController(
        $uibModalInstance,
        utilidades,
        Nivel,
        Configuracion,
        agregarActividadModalServicio
    ) {
        var vm = this;

        vm.options = [];
        vm.Nivel = Nivel;
        vm.Configuracion = Configuracion
        vm.agregarOtro = false;

        vm.mostrarErrorLongitud = false;
        vm.btnGuardarDisable = true;

        vm.NivelCaracteristicas = {
            PadreId: 0,
            ProductoId: '',
            ProductoInx: '',
            Nivel: ''
        };

        vm.NivelActual = [];
        vm.CatalogoEntregables = [];
        vm.NombreEntregableNuevo = '';
        vm.NuevoEntregableEnabled = true;
        vm.NuevoEntregable = false;
        vm.unidadMedidaId = '';
        vm.UnidadesMedida = [];

        function init() {
            var nivelesRegistrados = vm.Nivel["Hijos"];
            var nivelesCatalogo = [];
            var nivel = { ...vm.Nivel };
            var catalogoEntregables = nivel["CatalogoEntregables"];
            var nivelCatalogo = [];

            console.log("nivel", nivel)
            vm.NivelCaracteristicas["PadreId"] = nivel["PadreId"];
            vm.NivelCaracteristicas["ProductoId"] = nivel["Producto"]["ProductoId"];
            vm.NivelCaracteristicas["ProductoInx"] = nivel["Producto"]["Numeracion"];
            vm.NivelCaracteristicas["Nivel"] = nivel["NivelCatalogo"];
            vm.NivelCaracteristicas["ActividadId"] = nivel["ActividadId"];
            vm.UnidadesMedida = nivel["UnidadesMedida"];

            if (catalogoEntregables != null) {
                nivelCatalogo = catalogoEntregables.filter(p => p.Nivel == vm.Nivel["NivelCatalogo"] && p.DeliverableCatalogPadreId == vm.Nivel["DeliverableCatalogId"]);
                if (nivelesRegistrados != null && nivelesRegistrados.length > 0) {
                    nivelesCatalogo = nivelesRegistrados.filter(p => p.EntregableCatalogId != null && p.EntregableCatalogId == getEntregableId(p.EntregableCatalogId, catalogoEntregables));
                }

                nivelCatalogo.map(p => {
                    p["Padreid"] = vm.NivelCaracteristicas["PadreId"]
                    p["Selected"] = false;
                    p["Nivel"] = vm.NivelCaracteristicas.Nivel;
                    p["Disabled"] = nivelesCatalogo.find(t => t.EntregableCatalogId == p.DeliverableCatalogId) != undefined;
                    p["ActividadId"] = vm.NivelCaracteristicas["ActividadId"];
                });

                vm.NivelActual = [...nivelCatalogo];
                //vm.NuevoEntregableEnabled = (nivelesCatalogo.length >= Configuracion["cantMaxEntregablesCatalogo"]);
            }
        }

        function guardar() {
            var entregablesCatalogo = vm.NivelActual.filter(p => p.Selected)

            if (vm.NuevoEntregable) {
                if (vm.NombreEntregableNuevo.length < Configuracion["cantMinimaNombreEntregable"]) {
                    utilidades.mensajeError('La nueva actividad no cumple con la cantidad de carateres permitidos. Verificar el nombre.');
                    return;
                }

                if (vm.unidadMedidaId == undefined || vm.unidadMedidaId == null || vm.unidadMedidaId == '') {
                    utilidades.mensajeError('Debe seleccionar la unidad de medida');
                    return;
                }
                entregablesCatalogo.push({
                    DeliverableCatalogId: null,
                    Nivel: vm.NivelCaracteristicas.Nivel,
                    NombreEntregable: vm.NombreEntregableNuevo,
                    ProductoId: vm.NivelCaracteristicas.ProductoId,
                    Padreid: vm.NivelCaracteristicas.PadreId,
                    UnidadMedidaId: vm.unidadMedidaId,
                    ActividadId: vm.NivelCaracteristicas["ActividadId"],
                });
            } else if (!vm.NuevoEntregable && entregablesCatalogo.length <= 0) {
                utilidades.mensajeError('Debe seleccionar al menos una actividad.');
                return;
            }

            agregarActividadModalServicio.registrarActividades(entregablesCatalogo)
                .then(resultado => {
                    if (resultado.data != undefined) {
                        if (resultado.data.Status) {
                            utilidades.mensajeSuccess('', false, false, false, "Los datos fueron guardados con éxito.");
                            cerrar('ok');
                        } else {
                            swal("Error al realizar la operación", resultado.data.Message, 'error');
                        }
                    } else {
                        swal('', "Error al realizar la operación", 'error');
                    }
            });
        }

        function cerrar(result) {
            vm.NivelActual = [];
            $uibModalInstance.close(result);
        }

        function getEntregableId(entregableCatalogId, catalogo) {
            var nivelActual = catalogo.find(t => t.DeliverableCatalogId == entregableCatalogId);
            return nivelActual != undefined ? nivelActual.DeliverableCatalogId : '';
        }

        vm.init = init;
        vm.guardar = guardar;
        vm.cerrar = cerrar;
    }
})();