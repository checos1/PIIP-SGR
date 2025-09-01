(function () {
    'use strict';

    calendarioController.$inject = ['$scope',
        'utilidades',
        'calendarioServicio', '$sessionStorage'];

    function calendarioController($scope, utilidades,
        calendarioServicio, $sessionStorage,) {
        var vm = this;
        vm.listaCalendario = [];
        vm.listaCalendarioTemp = [];
        vm.mostrarOcultarFlujo = mostrarOcultarFlujo;
        vm.listaSectores = [];
        vm.Sectores = [];
        vm.listaFiltroEntidades = [];
        vm.listaEntidades = [];
        vm.showBtn = true;
        vm.dsblBtn = false;
        vm.activar = true;
        vm.activarExcepcion = true;
        vm.editar = "EDITAR";
        vm.editarExcepcion = "EDITAR";
        vm.disabled = false;
        vm.disabledExcepcion = false;
        vm.flujoid = $sessionStorage.flujoid;
        vm.flujoid = '1E436ACE-9155-3651-2B4C-D00BC4A57FE3';
        vm.listaExcepcionesValor = [];
        vm.listaExcepciones = [];
        vm.validaExcepcion = false;
        vm.mensajeValidacion = "";
        vm.idObjeto = 0;

        vm.init = function () {
            ConsultarCalendario();
            ConsultaSectores();
        };
        vm.init();

        function ConsultarCalendario() {
            calendarioServicio.ObtenerCalendarioProgramacion(vm.flujoid).then(
                function (response) {
                    if (response.data != null && response.data != "") {
                        vm.BusquedaRealizada = true;
                        vm.dsblBtn = true;
                        vm.listaCalendario = angular.copy(jQuery.parseJSON(jQuery.parseJSON(response.data)));
                        vm.listaCalendarioTemp = jQuery.parseJSON(jQuery.parseJSON(response.data));
                    } else {
                        vm.BusquedaRealizada = true;
                        vm.dsblBtn = false;
                        vm.listaCalendario = null;
                        utilidades.mensajeError("No se encontraron resultados para los criterios de búsqueda.");
                    }
                });
        }

        function ConsultaSectores() {
            calendarioServicio.catalogoTodosSectores(0).then(
                function (response) {
                    if (response.data != null && response.data != "") {
                        vm.listaSectores = jQuery.parseJSON(JSON.parse(response.data));
                    } else {
                        $("#ddlSectorCalendario").empty();
                    }
                });
        }

        vm.aplicar = function () {

            if (!vm.FechaInicio) {
                swal("Error al realizar la operación", "El campo fecha inicio es obligatorio.", 'error');
                return;
            }
            if (!vm.HoraInicio) {
                swal("Error al realizar la operación", "El campo hora inicio es obligatorio.", 'error');
                return;
            }
            if (!vm.FechaFin) {
                swal("Error al realizar la operación", "El campo fecha fin es obligatorio.", 'error');
                return;
            }
            if (!vm.HoraFin) {
                swal("Error al realizar la operación", "El campo hora fin es obligatorio.", 'error');
                return;
            }

            let anioInicio = vm.FechaInicio.getUTCFullYear();
            let mesInicio = vm.FechaInicio.getUTCMonth() + 1;
            let diaInicio = vm.FechaInicio.getUTCDate();
            let horaInicio = vm.HoraInicio.getHours();
            let minutosInicio = vm.HoraInicio.getMinutes();

            let anioFin = vm.FechaFin.getUTCFullYear();
            let mesFin = vm.FechaFin.getUTCMonth() + 1;
            let diaFin = vm.FechaFin.getUTCDate();
            let horaFin = vm.HoraFin.getHours();
            let minutosFin = vm.HoraFin.getMinutes();

            let fechaDesde = new Date(anioInicio + "-" + pad(mesInicio, 2) + "-" + pad(diaInicio, 2) + " " + pad(horaInicio, 2) + ":" + pad(minutosInicio, 2)),
                fechaHasta = new Date(anioFin + "-" + pad(mesFin, 2) + "-" + pad(diaFin, 2) + " " + pad(horaFin, 2) + ":" + pad(minutosFin, 2));

            if (fechaDesde.valueOf() > fechaHasta.valueOf()) {
                swal("Error al realizar la operación", "La fecha inicial no puede ser mayor a la fecha final.", 'error');
                return;
            }

            //Validar si se tienen excepciones
            let valExcep = false;
            for (var i = 0; i < vm.listaCalendario.length; i++) {
                if (vm.listaCalendario[i].Excepciones !== null) {
                    if (vm.listaCalendario[i].Excepciones.length > 0) {
                        valExcep = true;
                        break;
                    }
                }
            }

            if (valExcep) {
                utilidades.mensajeWarning("Al realizar esta modificación, es posible que deban ajustarse algunas excepciones ya diligenciadas. ¿Está seguro de continuar?", function funcionContinuar() {
                    vm.listaCalendario.forEach((element) => {
                        element.FechaDesde = fechaDesde;
                        element.FechaHasta = fechaHasta;
                    });

                }, function funcionCancelar(reason) {
                    return;
                });
            }
            else {
                vm.listaCalendario.forEach((element) => {
                    element.FechaDesde = fechaDesde;
                    element.FechaHasta = fechaHasta;
                });
            }
        }

        vm.changeSector = function (index) {
            vm.listaExcepcionesValor.EntidadId = [];
            document.getElementById("chkEntidadTodas_" + index).checked = false;
            $("#ddlEntidadCalendario_" + index).trigger("chosen:updated");
            let idSector = document.getElementById("ddlSectorCalendario_" + index).value;
            if (idSector === "") {
                vm.listaEntidades = [];
            }
            else {
                idSector = idSector.replace('number:', '');
                calendarioServicio.ObtenerEntidadesSector(idSector).then(
                    function (response) {
                        if (response.data != null && response.data != "") {
                            vm.listaEntidades = [];
                            vm.listaEntidades = jQuery.parseJSON(JSON.parse(response.data));
                        }
                    });
            }
        }

        vm.ActivarEditarExcepcion = function () {
            if (vm.activarExcepcion == true) {
                vm.editarExcepcion = "CANCELAR";
                vm.activarExcepcion = false;
            }
            else {
                vm.editarExcepcion = "EDITAR";
                vm.activarExcepcion = true;
                vm.Cancelar();
            }
        }

        vm.ActivarEditar = function () {
            if (vm.activar == true) {
                vm.editar = "CANCELAR";
                vm.activar = false;
            }
            else {
                vm.editar = "EDITAR";
                vm.activar = true;
                vm.Cancelar();
            }
        }

        vm.Cancelar = function () {
            utilidades.mensajeWarning("Los datos que posiblemente haya modificado se perderán. Se visualizarán únicamente los últimos guardados ¿Está seguro de continuar?", function funcionContinuar() {
                vm.listaCalendario = angular.copy(vm.listaCalendarioTemp);
                OkCancelar("La edición ha sido cancelada.");
            }, function funcionCancelar(reason) {
                return;
            });
        }

        vm.SeleccionarTodasEntidades = function (estado) {
            vm.estadoTodasEntidades = false;
            if (estado) {
                if (vm.listaEntidades.length > 0) {
                    vm.listaEntidades.forEach((element) => {
                        vm.listaExcepcionesValor.EntidadId[element.Id] = estado;
                    });
                }
            }
            else {
                vm.listaExcepcionesValor.EntidadId = [];
            }
        }

        vm.ChangeEntidad = function (estado) {

            if (vm.listaEntidades.length > 0) {
                for (var i = 0; i < vm.listaEntidades.length; i++) {
                    if (vm.listaExcepcionesValor.EntidadId[vm.listaEntidades[i].Id] !== estado) {
                        document.getElementById("chkEntidadTodas_" + vm.idObjeto).checked = false;
                        return false;
                    }
                }
            }

            document.getElementById("chkEntidadTodas_" + vm.idObjeto).checked = estado;
        }

        vm.AgregarExcepcion = function (index) {

            if (!$('#ddlSectorCalendario_' + index).find(":selected").val()) {
                swal("Error al realizar la operación", "El campo sector es obligatorio.", 'error');
                return;
            }

            if (vm.listaExcepcionesValor.EntidadId.length <= 0) {
                swal("Error al realizar la operación", "El campo entidad es obligatorio.", 'error');
                return;
            }

            let lstExcepcion = vm.listaCalendario.filter(x => x.CalendarioId == index);

            for (var i = 0; i < vm.listaEntidades.length; i++) {

                if (vm.listaExcepcionesValor.EntidadId[vm.listaEntidades[i].Id] === true) {
                    if (!lstExcepcion[0].Excepciones) {
                        lstExcepcion[0].Excepciones = [];
                    }

                    let existExcepcion = lstExcepcion[0].Excepciones.filter(x => x.CodigoEntidad == vm.listaEntidades[i].Code);
                    if (existExcepcion.length <= 0) {
                        let rowExcepcion = {};
                        let sector = $('#ddlSectorCalendario_' + index).find(":selected").text().split('-');
                        rowExcepcion.Sector = sector[1];
                        rowExcepcion.CodigoEntidad = vm.listaEntidades[i].Code;
                        rowExcepcion.Entidad = vm.listaEntidades[i].name;
                        rowExcepcion.CalendarioId = index;
                        lstExcepcion[0].Excepciones.push(rowExcepcion);
                    }
                }
            }
        }

        function OkCancelar(mensaje) {
            setTimeout(function () {
                utilidades.mensajeSuccess("", false, false, false, mensaje);
            }, 500);
        }

        function mostrarOcultarFlujo(campo) {
            let variable = $("#ico" + campo)[0].innerText;
            let imgmas = document.getElementById("imgmas" + campo);
            let imgmenos = document.getElementById("imgmenos" + campo);
            if (variable === "+") {
                $("#ico" + campo).html('-');
                imgmas.style.display = 'none';
                imgmenos.style.display = 'block';
            }
            else {
                $("#ico" + campo).html('+');
                imgmas.style.display = 'block';
                imgmenos.style.display = 'none';
            }
        }

        function pad(str, max) {
            str = str.toString();
            return str.length < max ? pad("0" + str, max) : str;
        }

        vm.AbrilNivel = function (objeto) {

            vm.idObjeto = objeto;
            vm.editarExcepcion = "EDITAR";
            vm.activarExcepcion = true;

            document.getElementById("ddlSectorCalendario_" + objeto).value = '';
            document.getElementById("chkEntidadTodas_" + objeto).checked = false;
            vm.listaEntidades = [];
            let variable = $("#icoObjet" + objeto).attr("src");

            if (variable === "Img/btnMas.svg") {
                $("#icoObjet" + objeto).attr("src", "Img/btnMenos.svg");
            }
            else {
                $("#icoObjet" + objeto).attr("src", "Img/btnMas.svg");
            }

            vm.listaCalendario.forEach((element) => {
                if (objeto !== element.CalendarioId) {
                    if ($("#icoObjet" + element.CalendarioId).attr("src") === "Img/btnMenos.svg") {
                        $("#icoObjet" + element.CalendarioId).attr("src", "Img/btnMas.svg");
                        $("#ind" + element.CalendarioId).collapse('toggle');
                    }
                }
            });
        }

        vm.GuardarProgramacion = function () {
            let validaFechas = false;

            if (vm.listaCalendario) {
                if (vm.listaCalendario.length > 0) {
                    for (var i = 0; i < vm.listaCalendario.length; i++) {
                        if (!vm.listaCalendario[i].FechaDesde) {
                            swal("Error al realizar la operación", "La fecha inicio del día para la sección " + vm.listaCalendario[i].NombreSeccion + " y el paso " + vm.listaCalendario[i].NombrePaso + " es obligatoria.", 'error');
                            return false;
                        }
                        if (new Date(vm.listaCalendario[i].FechaDesde).valueOf() > new Date(vm.listaCalendario[i].FechaHasta).valueOf()) {
                            swal("Error al realizar la operación", "La fecha inicio del día para la sección " + vm.listaCalendario[i].NombreSeccion + " y el paso " + vm.listaCalendario[i].NombrePaso + " no puede ser mayor a la fecha fin.", 'error');
                            return false;
                        }
                        if (!vm.listaCalendario[i].FechaHasta) {
                            swal("Error al realizar la operación", "La fecha fin del día para la sección " + vm.listaCalendario[i].NombreSeccion + " y el paso " + vm.listaCalendario[i].NombrePaso + " es obligatoria.", 'error');
                            return false;
                        }
                    }
                }
            }

            const formatDate = "YYYY-MM-DDTHH:mm:ss";

            vm.listaCalendario.forEach((cal) => {
                cal.FechaDesde = moment(cal.FechaDesde).format(formatDate);
                cal.FechaHasta = moment(cal.FechaHasta).format(formatDate);
                if (cal.Excepciones != null) {
                    if (cal.Excepciones != null) {
                        cal.Excepciones.forEach((exc) => {
                            exc.FechaDesde = moment(exc.FechaDesde).format(formatDate);
                            exc.FechaHasta = moment(exc.FechaHasta).format(formatDate);
                        });
                    }
                }
            });

            calendarioServicio.RegistrarCalendarioProgramacion(vm.listaCalendario)
                .then(resultado => {
                    if (resultado.data != undefined) {
                        if (resultado.data.Exito) {
                            vm.editar = "EDITAR";
                            vm.activar = true;
                            vm.FechaInicio = "";
                            vm.HoraInicio = "";
                            vm.FechaFin = "";
                            vm.HoraFin = "";
                            ConsultarCalendario();
                            utilidades.mensajeSuccess('', false, false, false, "Los datos han sido guardados con éxito.");
                            cerrar('ok');
                        } else {
                            swal("Error al realizar la operación", resultado.data.Mensaje, 'error');
                        }
                    } else {
                        swal('', "Error al realizar la operación", 'error');
                    }
                });
        }

        vm.GuardarExcepcion = function (index) {

            vm.mensajeValidacion = "";
            vm.validaExcepcion = false;

            let lstExcepcion = vm.listaCalendario.filter(x => x.CalendarioId == index);
            if (lstExcepcion) {
                if (lstExcepcion.length > 0) {
                    lstExcepcion.forEach((element) => {
                        if (element.Excepciones) {
                            if (element.Excepciones.length > 0) {
                                element.Excepciones.forEach((elementExc, indexExc) => {
                                    //Valida Fecha Desde
                                    document.getElementById("msgError_" + index + "_" + indexExc).innerHTML = "";
                                    if (!elementExc.FechaDesde) {
                                        document.getElementById("msgError_" + index + "_" + indexExc).style.display = '';
                                        document.getElementById("msgError_" + index + "_" + indexExc).innerHTML += "La fecha inicio del día de la excepción es obligatoria.";
                                        document.getElementById("msgError_" + index + "_" + indexExc).innerHTML += "<br>";
                                        vm.validaExcepcion = true;
                                    }
                                    if (new Date(elementExc.FechaDesde).valueOf() > new Date(elementExc.FechaHasta).valueOf()) {
                                        document.getElementById("msgError_" + index + "_" + indexExc).innerHTML += "La fecha inicio del día de la excepción no puede ser mayor a la fecha fin.";
                                        document.getElementById("msgError_" + index + "_" + indexExc).innerHTML += "<br>";
                                        vm.validaExcepcion = true;
                                    }
                                    else {
                                        if (document.getElementById("msgError_" + index + "_" + indexExc).innerHTML === "") {
                                            document.getElementById("msgError_" + index + "_" + indexExc).style.display = 'none';
                                        }
                                    }
                                    //Valida Fecha Hasta
                                    if (!elementExc.FechaHasta) {
                                        document.getElementById("msgError_" + index + "_" + indexExc).style.display = '';
                                        document.getElementById("msgError_" + index + "_" + indexExc).innerHTML += "La fecha fin del día de la excepción es obligatoria.";
                                        document.getElementById("msgError_" + index + "_" + indexExc).innerHTML += "<br>";
                                        vm.validaExcepcion = true;
                                    }
                                    else {
                                        if (document.getElementById("msgError_" + index + "_" + indexExc).innerHTML === "") {
                                            document.getElementById("msgError_" + index + "_" + indexExc).style.display = 'none';
                                        }
                                    }
                                });
                            }
                        }
                    });
                }
            }

            if (vm.validaExcepcion) {
                vm.editarExcepcion = "CANCELAR";
                vm.activarExcepcion = false;
                return;
            }

            const formatDate = "YYYY-MM-DDTHH:mm:ss";

            lstExcepcion.forEach((cal) => {
                cal.FechaDesde = moment(cal.FechaDesde).format(formatDate);
                cal.FechaHasta = moment(cal.FechaHasta).format(formatDate);
                if (cal.Excepciones != null) {
                    if (cal.Excepciones != null) {
                        cal.Excepciones.forEach((exc) => {
                            exc.FechaDesde = moment(exc.FechaDesde).format(formatDate);
                            exc.FechaHasta = moment(exc.FechaHasta).format(formatDate);
                        });
                    }
                }
            });

            calendarioServicio.RegistrarCalendarioProgramacion(lstExcepcion)
                .then(resultado => {
                    if (resultado.data != undefined) {
                        if (resultado.data.Exito) {
                            vm.editarExcepcion = "EDITAR";
                            vm.activarExcepcion = true;
                            ConsultarCalendario();
                            utilidades.mensajeSuccess('', false, false, false, "Los datos han sido guardados con éxito.");
                            cerrar('ok');
                        } else {
                            swal("Error al realizar la operación", resultado.data.Mensaje, 'error');
                        }
                    } else {
                        swal('', "Error al realizar la operación", 'error');
                    }
                });
        }

        vm.EliminarExcepcion = function (indexCal, entidad) {

            utilidades.mensajeWarning("Se eliminará esta excepción. ¿está seguro de continuar?", function funcionContinuar() {
                OkCancelar("Los datos fueron eliminados con éxito.");

                var index = vm.listaCalendario[indexCal].Excepciones.findIndex(p => p.CodigoEntidad == entidad);
                if (index > -1) {
                    vm.listaCalendario[indexCal].Excepciones.splice(index, 1);
                }
            }, function funcionCancelar(reason) {
                return;
            });
        }
    }

    angular.module('backbone').component('calendario', {
        templateUrl: 'src/app/programacion/componentes/calendario/calendario.html',
        controller: calendarioController,
        controllerAs: "vm",
    });
})()