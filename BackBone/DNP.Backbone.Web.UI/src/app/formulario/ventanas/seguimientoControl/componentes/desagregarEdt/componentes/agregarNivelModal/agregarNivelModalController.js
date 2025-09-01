(function () {
    'use strict';

    angular.module('backbone')
        .controller('agregarNivelModalController', agregarNivelModalController);

    agregarNivelModalController.$inject = [
        '$scope',
        '$uibModalInstance',
        'utilidades',
        '$filter',
        'agregarNivelModalServicio',
        'Nivel',
        'Configuracion'
    ];

    function agregarNivelModalController(
        $scope,
        $uibModalInstance,
        utilidades,
        $filter,
        agregarNivelModalServicio,
        Nivel,
        Configuracion
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
            Nivel: ''
        };

        vm.NivelActual = [];
        vm.CatalogoEntregables = [];
        vm.NombreEntregableNuevo = '';
        vm.NuevoEntregableEnabled = true;
        vm.NuevoEntregable = false;

        function guardar() {
            var entregablesCatalogo = vm.NivelActual.filter(p => p.Selected)

            if (vm.NuevoEntregable) {
                if (vm.NombreEntregableNuevo.length < Configuracion["cantMinimaNombreEntregable"]) {
                    utilidades.mensajeError('El nuevo entregable no cumple con la cantidad de carateres permitidos. Verificar el nombre.');
                    return;
                }
                entregablesCatalogo.push({
                    DeliverableCatalogId: null,
                    Nivel: vm.NivelCaracteristicas.Nivel,
                    NombreEntregable: vm.NombreEntregableNuevo,
                    ProductoId: vm.NivelCaracteristicas.ProductoId,
                    Padreid: vm.NivelCaracteristicas.PadreId,
                    ActividadId: vm.NivelCaracteristicas["ActividadId"],
                });
            } else if (!vm.NuevoEntregable && entregablesCatalogo.length <= 0) {
                utilidades.mensajeError('Debe seleccionar al menos un entregable.');
                return;
            }
            agregarNivelModalServicio.registrarNiveles(entregablesCatalogo)
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
        
        function seleccionarNuevo(nuevo) {
            vm.agregarOtro = nuevo.isChecked;
            toggleGuardar();
        };

        function init() {
            var nivelesRegistrados = vm.Nivel["Hijos"];
            var nivelesCatalogo = [];
            var nivel = { ...vm.Nivel };
            var catalogoEntregables = nivel["CatalogoEntregables"];
            var nivelCatalogo = [];

            vm.NivelCaracteristicas["PadreId"] = nivel["PadreId"];
            vm.NivelCaracteristicas["ProductoId"] = nivel["Producto"]["ProductoId"];
            vm.NivelCaracteristicas["ProductoInx"] = nivel["Producto"]["Numeracion"];
            vm.NivelCaracteristicas["Nivel"] = nivel["NivelCatalogo"];
            vm.NivelCaracteristicas["ActividadId"] = nivel["ActividadId"];


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

        function getEntregableId(entregableCatalogId, catalogo) {
            var nivelActual = catalogo.find(t => t.DeliverableCatalogId == entregableCatalogId);
            return nivelActual != undefined ? nivelActual.DeliverableCatalogId : '';
        }

        vm.init = init;
        vm.guardar = guardar;
        vm.cerrar = cerrar;
        vm.seleccionarNuevo = seleccionarNuevo;
    }
})();