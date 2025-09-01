(function () {
    'use strict';

    consolaReportesController.$inject = ['$scope', 'servicioConsolaReportes', '$uibModal', 'utilidades', '$sessionStorage', 'Blob', 'FileSaver'];

    function consolaReportesController($scope, servicioConsolaReportes, $uibModal, utilidades, sessionStorage, Blob, FileSaver) {

        //variables
        var vm = this;
        var tempIdReporte = "";
        var detalleReporte = {}

        vm.listadoAgrupador = [];
        vm.listadoReportes = [];
        vm.init = init;
        vm.idUsuario = sessionStorage.usuario.permisos.IdUsuarioDNP;
        vm.listaRoles = sessionStorage.usuario.roles;
        vm.listaEntidades = sessionStorage.usuario.permisos.Entidades;
       // vm.idFormulario = "";   //$sessionStorage.idNivel;

        //Métodos sessionStorage.usuario.permisos.Entidades
        function init() {
            ObtenerAgrupadorReportes()
        }

        function ObtenerAgrupadorReportes() {
            servicioConsolaReportes.obtenerAgrupadorReportes(vm.idUsuario)
                .then(function (response) {

                    var tempAgrupadores = jQuery.parseJSON(response.data);
                    var agrupadores = jQuery.parseJSON(tempAgrupadores)

                    agrupadores.forEach(item => {
                        item.id = item.id;
                        item.descripcion = item.descripcion;
                        item.nombre = item.nombre;
                        item.subproceso = item.subproceso;
                        item.estadoAgr = "+";
                    });

                    vm.listadoAgrupador = agrupadores;
                    ObtenerListadoReportes();

                }, function (error) {
                    console.log("error: obtenerAgrupadorReportes", error);
                });
        }

        function ObtenerListadoReportes() {

            //var idroles = [];

           var idroles = vm.listaRoles.map(function (rol) {
                return rol.IdRol;
            });


            servicioConsolaReportes.obtenerListadoReportes(vm.idUsuario, idroles)
                .then(function (respuesta) {
                    if (respuesta.data != null) {
                        var tempListado = jQuery.parseJSON(respuesta.data);
                        vm.listadoReportes = jQuery.parseJSON(tempListado);
                    }
                    else
                        utilidades.mensajeError("El usuario no tiene permisos para generar reportes.");

                }, function (error) {
                    console.log("error: ObtenerListadoReportes", error);
                    utilidades.mensajeError("Se presento un error consultado los reportes.");
                });
        }

        function ObtenerFiltrosReportes(idReporte) {
            servicioConsolaReportes.ObtenerFiltrosReportes(idReporte, vm.idUsuario)
                .then(function (response) {
                    var filtrosJs = jQuery.parseJSON(response.data);
                    var listaFiltros = jQuery.parseJSON(filtrosJs);
                    var listaNombreFiltros = [];

                    listaFiltros.forEach(item => {
                        listaNombreFiltros.push(item.nombre);
                    });
                    return listaNombreFiltros;

                }, function (error) {
                    console.log("error: obtenerFiltrosReportes", error);
                })
        }

        vm.abrirModalFiltros = function (idReporte) {

            vm.listadoReportes.forEach(item => {
                if (item.idReporte == idReporte) {
                    detalleReporte = { esquema: item.esquema, otroEsquema: item.otroEsquema }
                }
            });

            var obj = {
                idReporte: idReporte,
                idUsuario: vm.idUsuario,
                listaEntidades: vm.listaEntidades,
                detalleReporte: detalleReporte
            }

            $uibModal.open({
                templateUrl: '/src/app/reportesPIIP/modalFiltros.html',
                controller: 'modalFiltrosController',
                resolve: {
                    obj: obj,
                }
            }).result.then(function (result) {
                init();
            }, function (reason) {

            }), err => {
                toastr.error("Ocurrió un error al consultar el idReporte" + err);
            };
        }

        vm.AbrilNivel = function (idAgrupador) {
            vm.listadoAgrupador.forEach(function (value, index) {
                if (value.id == idAgrupador) {
                    if (value.estadoAgr == '+')
                        value.estadoAgr = '-';
                    else
                        value.estadoAgr = '+';
                }
            });
        }

        vm.downloadPdf = function (idReporte) {
            tempIdReporte = idReporte;

            utilidades.mensajeWarning("Estimado usuario: es posible que el reporte tarde en generarse más de lo esperado, debido a la cantidad de información, confirma la descarga? Recomendamos el uso de filtros para los reportes" +
                '<img src="/Img/u1341.svg" width="20" height="20" >',

                function funcionContinuar() {
                    var filtros = null;
                    servicioConsolaReportes.ObtenerDatosReportePIIP(tempIdReporte, filtros, vm.idUsuario)
                        .then(function (response) {
                            if (response.data != null) {
                                var resultadoReporte = jQuery.parseJSON(response.data);
                                JSONToCSVConvertor(resultadoReporte, "_" + Date.now(), true);
                            }
                            else
                                utilidades.mensajeError("Su consulta no arrojo resultados.");

                        }, function (error) {
                            console.log("error: ObtenerDatosReportePIIP", error);
                            utilidades.mensajeError("Su consulta excedio el tiempo de respuesta.");
                        });

                }, function funcionCancelar() {
                    return;
                })
               
        }

        vm.downloadExcel = function (idReporte) {

            tempIdReporte = idReporte;
            vm.listadoReportes.forEach(item => {
                if (item.idReporte == idReporte) {
                    detalleReporte = { esquema: item.esquema, otroEsquema: item.otroEsquema}}
            });

            var listaEntidades = "";

            vm.listaEntidades.forEach(ent => {
                if (listaEntidades == "")
                    listaEntidades = ent.IdEntidad;
                else
                    listaEntidades = listaEntidades + "," + ent.IdEntidad;
            })

            utilidades.mensajeWarning("Estimado usuario: es posible que el reporte tarde en generarse más de lo esperado, debido a la cantidad de información, confirma la descarga? Recomendamos el uso de filtros para los reportes" +
                '<img src="/Img/u1341.svg" width="20" height="20" >',

                function funcionContinuar() {
                    var filtros = null;
                    servicioConsolaReportes.ObtenerDatosReportePIIP(tempIdReporte, filtros, vm.idUsuario, detalleReporte, listaEntidades)
                        .then(function (response) {
                            if (response.data != null) {
                               var resultadoReporte = jQuery.parseJSON(response.data);
                                JSONToCSVConvertor(resultadoReporte, "_" + Date.now(), true);
                                utilidades.mensajeSuccess('Se genero el reporte.', false, null, null);
                            }
                            else
                                utilidades.mensajeError("Su consulta no arrojo resultados.");

                        }, function (error) {
                            console.log("error: ObtenerDatosReportePIIP", error);
                            utilidades.mensajeError("Su consulta excedio el tiempo de respuesta.");
                        });

                }, function funcionCancelar() {
                    return;
                })
        }

        function JSONToCSVConvertor(JSONData, ReportTitle, ShowLabel) {
            //If JSONData is not an object then JSON.parse will parse the JSON string in an Object
            var arrData = typeof JSONData != 'object' ? JSON.parse(JSONData) : JSONData;

            var CSV = 'sep=,' + '\r\n\n';

            //This condition will generate the Label/Header
            if (ShowLabel) {
                var row = "";

                //This loop will extract the label from 1st index of on array
                for (var index in arrData[0]) {

                    //Now convert each value to string and comma-seprated
                    row += index + ',';
                }

                row = row.slice(0, -1);

                //append Label row with line break
                CSV += row + '\r\n';
            }

            //1st loop is to extract each row
            for (var i = 0; i < arrData.length; i++) {
                var row = "";

                //2nd loop will extract each column and convert it in string comma-seprated
                for (var index in arrData[i]) {
                    row += '"' + arrData[i][index] + '",';
                }

                row.slice(0, row.length - 1);

                //add a line break after each row
                CSV += row + '\r\n';
            }

            if (CSV == '') {
                alert("Invalid data");
                return;
            }

            //Generate a file name
            var fileName = "Reporte_";
            //this will remove the blank-spaces from the title and replace it with an underscore
            fileName += ReportTitle.replace(/ /g, "_");

            //Initialize file format you want csv or xls
            var uri = 'data:text/csv;charset=utf-8,' + escape(CSV);

            // Now the little tricky part.
            // you can use either>> window.open(uri);
            // but this will not work in some browsers
            // or you will not get the correct file extension    

            //this trick will generate a temp <a /> tag
            var link = document.createElement("a");
            link.href = uri;

            //set the visibility hidden so it will not effect on your web-layout
            link.style = "visibility:hidden";
            link.download = fileName + ".csv";

            //this part will append the anchor tag and remove it after automatic click
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        }

    };

    angular.module('backbone').controller('consolaReportesController', consolaReportesController);

})();