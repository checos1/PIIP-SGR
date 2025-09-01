(function () {
    'use strict';

    presupuestalController.$inject = ['$scope', 'utilidades', 'presupuestalServicio'];

    function presupuestalController($scope, utilidades, presupuestalServicio) {
        var vm = this;
        vm.buscar = buscar;
        vm.LimpiarChecks = LimpiarChecks;
        vm.listaSectores = [];
        vm.Sectores = [];
        vm.listaFiltroEntidades = [];
        vm.listaEntidades = [];
        vm.listaPresupuestales = [];
        vm.listaPresupuestalesValidado = [];
        vm.showBtn = true;
        vm.dsblBtn = false;
        vm.showBtnSimular = false;
        vm.dsblBtnSimular = false;
        vm.permiteEditar = false;
        vm.showBtnEditar = false;
        vm.activar = false;
        vm.activarConsecutivo = false;
        vm.Editar = "EDITAR";
        vm.Consecutivo = 0;

        vm.init = function () {
            ConsultaSectores();
        };
        vm.init();

        function ConsultaSectores() {
            presupuestalServicio.catalogoTodosSectores(0).then(
                function (response) {
                    if (response.data != null && response.data != "") {
                        vm.listaSectores = jQuery.parseJSON(JSON.parse(response.data));

                        vm.listaSectores.unshift({ Code: 0, id: 0, name: 'Todos', Sector: 'Todos' });
                        vm.Sectores = vm.listaSectores;

                        vm.changeSector();

                    } else {
                        $("#ddlSector").empty();
                    }
                });
        }
        vm.changeSector = function () {
            var idSector = document.getElementById("ddlSector").value;
            idSector = idSector.replace('number:', '');

            if (!idSector || idSector == undefined || idSector == '') {
                idSector = 0;
            }

            presupuestalServicio.ObtenerEntidadesSector(idSector).then(
                function (response) {
                    if (response.data != null && response.data != "") {
                        vm.listaEntidades = jQuery.parseJSON(JSON.parse(response.data));
                        vm.listaFiltroEntidades = vm.listaEntidades;
                    }
                });
        }

        function buscar() {
            var idSector = document.getElementById("ddlSector").value;
            idSector = idSector.replace('number:', '');
            var idEntidad = document.getElementById("ddlEntidad").value;
            idEntidad = idEntidad.replace('number:', '');
            var bpin = document.getElementById("txtBPIN").value;

            if (bpin == "") {
                bpin = "0";
            }

            if (idSector == "" || idSector == "?") {
                idSector = 0;
            }

            if (idEntidad == "" || idEntidad == "?") {
                idEntidad = 0;
            }
            presupuestalServicio.ObtenerProyectosGenerarPresupuestal(idSector, idEntidad, bpin).then(
                function (response) {
                    if (response.data != null && response.data != "") {
                        vm.showBtn = true;
                        vm.showBtnEditar = true;
                        vm.BusquedaRealizada = true;
                        vm.listaPresupuestales = jQuery.parseJSON(JSON.parse(response.data));
                        console.log(vm.listaPresupuestales);
                        vm.cantidadDeProyectos = response.data.length;
                        LimpiarChecks();
                        //swal("Se encontraron" & vm.cantidadDeProyectos & ' resultados.', 'error');
                    } else {
                        vm.showBtn = true;
                        vm.showBtnEditar = false;
                        vm.BusquedaRealizada = false;
                        vm.listaPresupuestales = null;
                        vm.cantidadDeProyectos = 0;
                        vm.showBtnSimular = false;
                        utilidades.mensajeError("No se encontraron resultados para los criterios de búsqueda.");
                    }
                });
        }

        vm.primeraVez = 0;
        vm.ActivarEditar = function () {
            if (vm.activar == true || vm.primeraVez == 0) {
                vm.Editar = "CANCELAR";
                vm.activar = false;
                vm.activarConsecutivo = true;
                vm.showBtnSimular = true;
                vm.primeraVez++;
            }
            else {
                vm.Editar = "EDITAR";
                vm.activar = true;
                vm.activarConsecutivo = false;
                vm.showBtnSimular = false;
                vm.Cancelar();
            }
        }

        vm.Cancelar = function () {
            OkCancelar();
        }

        function OkCancelar() {           
            vm.listaPresupuestales = [];
            vm.activar = true;
            setTimeout(function () {
                utilidades.mensajeSuccess("", false, false, false, "La edición ha sido cancelada con éxito.");
            }, 500);
        }

        vm.Guardar = function () {
            const data = [];
            var index = 0;            
            vm.listaPresupuestales.forEach(presupuestal => {
                var nombreChk = "#chkProyectoId" + index;
                var seleccionado = $(nombreChk).is(":checked");
                if (seleccionado && presupuestal.Consecutivo && presupuestal.Consecutivo != "") {
                    data.push({
                        CodigoPresupuestal: presupuestal.CodigoPresupuestal,
                        ProyectoId: presupuestal.ProyectoId,
                        EntityTypeCatalogOptionId: presupuestal.EntityTypeCatalogOptionId,
                        FecDesde: presupuestal.FecDesde,
                        FecHasta: presupuestal.FecHasta,
                        CodigoEntidad: presupuestal.CodigoEntidad,
                        Programa: presupuestal.Programa,
                        Subprograma: presupuestal.Subprograma,
                        Consecutivo: presupuestal.Consecutivo
                    });
                }
                index++;
            });

            presupuestalServicio.RegistrarProyectosSinPresupuestal(JSON.stringify(data)).then(
                function (response) {
                    if (response.data.Exito) {
                        utilidades.mensajeSuccess("", false, false, false, "Los códigos presupuestales han sido asignados con éxito a los proyectos seleccionados.");
                        vm.buscar();
                        vm.ActivarEditar();
                    } else {
                        utilidades.mensajeError("Error guardando códigos presupuestales.");
                    }
                });
        }

        vm.Validar = function () {
            const data = [];
            vm.activar = true;
            vm.listaPresupuestalesValidado = [];
            var index = 0;
            var flagMarcaRegistro = 0;
            vm.listaPresupuestales.forEach(presupuestal => {
                var nombreChk = "#chkProyectoId" + index;
                var seleccionado = $(nombreChk).is(":checked");
                let consecutivo = presupuestal.Consecutivo;
                if (consecutivo && consecutivo != null && !$.isNumeric(consecutivo)) {
                    consecutivo = 0;
                }
                if (seleccionado) {
                    flagMarcaRegistro = 1;
                        data.push({
                            CodigoPresupuestal: presupuestal.CodigoPresupuestal,
                            ProyectoId: presupuestal.ProyectoId,
                            EntityTypeCatalogOptionId: presupuestal.EntityTypeCatalogOptionId,
                            FecDesde: presupuestal.FecDesde,
                            FecHasta: presupuestal.FecHasta,
                            CodigoEntidad: presupuestal.CodigoEntidad,
                            Programa: presupuestal.Programa,
                            Subprograma: presupuestal.Subprograma,
                            Consecutivo: consecutivo
                        });                                        
                }
                index++;
            });
            if (flagMarcaRegistro == 0) {
                utilidades.mensajeError("No se encuentra ningún proyecto seleccionado para asignar presupuestal.");
            }
            else {           
               presupuestalServicio.ValidarConsecutivoPresupuestal(JSON.stringify(data)).then(
                function (response) {
                    if (response.data != null && response.data.Result != "") {
                        vm.listaPresupuestalesValidado = jQuery.parseJSON(response.data.Result);
                        vm.activar = true;
                        vm.listaPresupuestales.forEach(item => {
                            item.TieneError = false;
                            item.ValidacionError = "";
                            if (vm.listaPresupuestalesValidado != undefined) {
                                vm.listaPresupuestalesValidado.forEach(itemValidado => {
                                    if ((item.ProyectoId == itemValidado.ProyectoId) && (item.EntityTypeCatalogOptionId == itemValidado.EntityTypeCatalogOptionId)) {
                                        item.CodigoPresupuestal = itemValidado.CodigoPresupuestal;
                                        item.Consecutivo = itemValidado.ConsecutivoActual;
                                        item.ValidacionError = "";
                                        if (itemValidado.ValidacionError && itemValidado.ValidacionError != "") {
                                            item.ValidacionError = itemValidado.ValidacionError;
                                            item.TieneError = true;
                                            vm.activar = false;
                                            //let mensajeR = itemValidado.ValidacionError.split("").reverse().join("");
                                            //let cons = mensajeR.substring(0, mensajeR.indexOf(' ')).split("").reverse().join("");
                                        }
                                    }
                                    if ((item.CodigoPresupuestal == itemValidado.CodigoPresupuestal) && (item.ProyectoId != itemValidado.ProyectoId)) {
                                        item.ValidacionError = "El proceso de simulación indica que se generan códigos presupuestales repetidos, por favor ajustar el consecutivo.";
                                        item.TieneError = true;
                                        vm.activar = false;
                                    }
                                });
                            }
                        });
                        console.log(vm.listaPresupuestales);
                    } else {
                        utilidades.mensajeError("Error realizando el proceso de simulación.");
                    }
                    });
            }
        }

        function LimpiarChecks() {
            var index = 0;
            vm.listaPresupuestales.forEach(presupuestal => {
                var nombreChk = "#chkProyectoId" + index;
                $(nombreChk).prop('checked', false);;
                index++;
            });
        }

        $('#chkProyectoIdHeader').change(function () {
            vm.SeleccionarTodos();
        });

        vm.SeleccionarTodos = function () {
            var index = 0;
            var seleccionado = $("#chkProyectoIdHeader").is(":checked");
            vm.listaPresupuestales.forEach(presupuestal => {
                var nombreChk = "#chkProyectoId" + index;
                $(nombreChk).prop('checked', seleccionado);;
                index++;
            });
        }

    }

    angular.module('backbone').component('presupuestal', {
        templateUrl: 'src/app/programacion/componentes/generarPresupuestal/presupuestal.html',
        controller: presupuestalController,
        controllerAs: "vm",
    });
})()