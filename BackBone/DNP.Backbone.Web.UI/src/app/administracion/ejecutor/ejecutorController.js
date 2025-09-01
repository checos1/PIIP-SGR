(function () {
    'use strict';

    ejecutorController.$inject = [
        '$sessionStorage',
        '$scope',
        'constantesBackbone',
        'ejecutorServicio',
        'catalogoServicio',
        'utilidades'
    ];

    function ejecutorController(
        $sessionStorage,
        $scope,
        constantesBackbone,
        ejecutorServicio,
        catalogoServicio,
        utilidades
    )
    {
        var vm = this;

        vm.init = function () {
            vm.tieneErrorNit = false;
            vm.mostrarResultadoEditar = false;
            vm.mostrarResultadoAgregar = false;
            vm.nombreEjecutorDisabled = true;
            vm.buscadorDisabled = false;
            vm.mostrarIconoEditar = true;
            vm.mostrarIconoGuardar = false;
            vm.mostrarIconoCancelar = true;
            vm.Activo = true;
            vm.Editar = false;
            vm.Agregar = false;
            vm.nit = "";
            vm.digito = "";
            vm.nombreEjecutor = "";
            vm.mensajeValidacion = "";
            vm.tieneMensajeValidacion = false;
        }

        vm.consultarEjecutor = function (nit) {
            if (nit === undefined || nit === '') {
                vm.tieneErrorNit = true;
            }
            else {
                vm.tieneErrorNit = false;
                ejecutorServicio.consultarEjecutor(nit)
                    .then(function (response) {
                        if (response.data != null) {
                           vm.objEjecutor = response.data;
                           vm.mostrarResultadoEditar = true;
                           vm.mostrarResultadoAgregar = false;
                           vm.Editar = true;
                           vm.Agregar = false;
                           vm.buscadorDisabled = true;
                           vm.mensajeWarning = "Se va a realizar una modificación en el nombre de la entidad con Nit " + vm.nit.toString() + " -" + vm.digito.toString()
                        }
                        else {
                            catalogoTodosTiposEntidades();
                            vm.Agregar = true;
                            vm.Editar = false;
                            vm.mostrarResultadoEditar = false;
                            vm.mostrarResultadoAgregar = true;
                            vm.buscadorDisabled = true;
                            vm.mensajeWarning = "La entidad con Nit " + vm.nit.toString() + " -" + vm.digito.toString() + ", será guardada como Ejecutor SGR."
                        }
                    }, function (error) {
                        utilidades.mensajeError('No fue posible encontrar el nit');
                    });
            }
        }

        function actualizarEjecutor(data) {
            utilidades.mensajeWarning("El cambio se reflejará automáticamente en todos los espacios donde aparezca la entidad como SGR. Desea continuar?",
                function funcionContinuar() {
                        return respuesta = ejecutorServicio.guardarEjecutor(data).then(function (response) {
                            if (response.data) {
                                utilidades.mensajeSuccess("El nombre de la entidad fue modificado con éxito!", false, false, false);
                                vm.init();
                            } else {
                                utilidades.mensajeError("Error al realizar la operación", false);
                            }
                        })
                },
                function funcionCancelar() {
                }, "Aceptar", "Cancelar", vm.mensajeWarning)
        }

        function catalogoTodosTiposEntidades() {
            catalogoServicio.catalogoTodosTiposEntidades().then(function (response) {
                if (response.data) {
                    vm.tiposEntidad = response.data;
                }
            });
        }

        function validarGuardar (data) {
            var respuesta = true;
            if (vm.Editar) {
                if (data.NombreEjecutor.length == 0) {
                    vm.tieneMensajeValidacion = true;
                    vm.mensajeValidacion = "Favor digite el nombre de la entidad";
                    respuesta = false;
                }
            }
            else {
                if (data.EntityTypeId === undefined) {
                    vm.tieneMensajeValidacion = true;
                    vm.mensajeValidacion = "Favor seleccione un elemento de tipo de entidad.";
                    respuesta = false;
                }
                else if (data.EntityTypeId === "") {
                    vm.tieneMensajeValidacion = true;
                    vm.mensajeValidacion = "Favor seleccione un elemento de tipo de entidad.";
                    respuesta = false;
                }

                if (data.NombreEjecutor === undefined) {
                    vm.tieneMensajeValidacion = true;
                    vm.mensajeValidacion = "Favor digite el nombre de la entidad.";
                    respuesta = false;
                }
                else if (data.NombreEjecutor.length == 0) {
                    vm.tieneMensajeValidacion = true;
                    vm.mensajeValidacion = "Favor digite el nombre de la entidad.";
                    respuesta = false;
                }

            }
            return respuesta;
        }

        vm.calcularDigito = function (nit) {
            if (nit.toString().length > 0) {
                vm.digito = calcularDigitoVerificacion(nit.toString());
            }

        };

        vm.clickIconoEditar = function () {
            vm.mostrarIconoEditar = false;
            vm.mostrarIconoCancelar = true;
            vm.mostrarIconoGuardar = true;
            vm.nombreEjecutorDisabled = false;

        };

        vm.clickIconoCancelar = function () {
            vm.mostrarIconoEditar = true;
            vm.mostrarIconoCancelar = false;
            vm.mostrarIconoGuardar = false;
            vm.nombreEjecutorDisabled = true;
            vm.buscadorDisabled = false;
            vm.init();
        };

        vm.limpiarValidacion = function () {
            vm.mensajeValidacion = "";
            vm.tieneMensajeValidacion = false;
        };

        vm.clickIconoGuardar = function () {
            
            if (vm.Editar) {

                var data = {
                    Id: vm.objEjecutor.Id,
                    Nit: vm.nit,
                    Digito: vm.digito,
                    EntityTypeId: vm.objEjecutor.EntityTypeId,
                    CreadoPor: vm.objEjecutor.CreadoPor,
                    NombreEjecutor: vm.objEjecutor.NombreEjecutor,
                    Activo: vm.Activo
                }
                if (validarGuardar(data)) {
                    actualizarEjecutor(data);

                    vm.mostrarIconoEditar = true;
                    vm.mostrarIconoCancelar = false;
                    vm.mostrarIconoGuardar = false;
                    vm.nombreEjecutorDisabled = true;
                }
            }
            else {

                var data = {
                    Id: "0",
                    Nit: vm.nit,
                    Digito: vm.digito,
                    EntityTypeId: vm.idTipoEntidad,
                    CreadoPor: "",
                    NombreEjecutor: vm.nombreEjecutor,
                    Activo: vm.Activo
                }

                if (validarGuardar(data)) {
                    actualizarEjecutor(data);
                    vm.idTipoEntidad = "";
                }
            }
            
        };


        function calcularDigitoVerificacion(myNit) {
            var vpri, x, y,z;

            myNit = myNit.replace(/\s/g, ""); 
            myNit = myNit.replace(/,/g, ""); 
            myNit = myNit.replace(/\./g, ""); 
            myNit = myNit.replace(/-/g, ""); 

            if (isNaN(myNit)) {
                return "";
            };

            vpri = new Array(16);
            z = myNit.length;

            vpri[1] = 3;
            vpri[2] = 7;
            vpri[3] = 13;
            vpri[4] = 17;
            vpri[5] = 19;
            vpri[6] = 23;
            vpri[7] = 29;
            vpri[8] = 37;
            vpri[9] = 41;
            vpri[10] = 43;
            vpri[11] = 47;
            vpri[12] = 53;
            vpri[13] = 59;
            vpri[14] = 67;
            vpri[15] = 71;

            x = 0;
            y = 0;
            for (var i = 0; i < z; i++) {
                y = (myNit.substr(i, 1));
                x += (y * vpri[z - i]);
            }

            y = x % 11;

            return (y > 1) ? 11 - y : y;
        }
    }
    angular.module('backbone').controller('ejecutorController', ejecutorController);
})();
