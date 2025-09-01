(function () {
    'use strict';

    modalFiltrosController.$inject = [
        'obj',
        '$uibModalInstance',
        'utilidades',
        'servicioConsolaReportes', '$sessionStorage'
    ];

    function modalFiltrosController(
        obj,
        $uibModalInstance,
        utilidades,
        servicioConsolaReportes,
        $sessionStorage
    ) {

        var vm = this;
        var detalleReporte = obj.detalleReporte;
        vm.idReporte = obj.idReporte;
        vm.cerrar = $uibModalInstance.dismiss;
        vm.idUsuario = obj.idUsuario;
        vm.listaFiltros = [];
        vm.tempNombreFiltros = [];
        vm.listaEntidades = obj.listaEntidades;
        vm.check = check;
        vm.limpiarCamposFiltro = limpiarCamposFiltro;
        vm.filtrar = filtrar;


        /// Comienzo
        vm.init = function () {
            ObtenerFiltrosReportes();
        }

        function ObtenerFiltrosReportes() {
            servicioConsolaReportes.ObtenerFiltrosReportes(vm.idReporte, vm.idUsuario)
                .then(function (response) {
                    var filtrosJs = jQuery.parseJSON(response.data);
                    var listaFiltros = jQuery.parseJSON(filtrosJs)
                    var listaFiltrosFiltros = [];

                    listaFiltros.forEach(item => {

                        var templst = '';
                        var tempFiltro = '';

                        if (item.listaFiltro != '')
                            templst = jQuery.parseJSON(item.listaFiltro);

                        tempFiltro = {
                            idfiltro: item.idfiltro,
                            nombre: item.nombre,
                            descripcion: item.descripcion,
                            nombreTabla: item.nombreTabla,
                            listaFiltro: templst
                        }

                        vm.tempNombreFiltros.push(item.nombre);
                        listaFiltrosFiltros.push(tempFiltro);
                    });

                    vm.listaFiltros = listaFiltrosFiltros;

                }, function (error) {
                    console.log("error: obtenerFiltrosReportes", error);
                });
        }

        vm.guardar = function () {

            var tempContadorFiltros = 0;
            var cadena = "{";
            vm.filtro = {}
            filtrar();
            vm.tempNombreFiltros.forEach(item => {
                if ($("#" + item)[0] != undefined) {
                    if ($("#" + item)[0].type == 'select-one') {
                        var temNombre = $("#" + item)[0].id;
                        var temValue = $("#" + item)[0].value;
                        item = { temNombre: temValue }

                        if (temValue == "")
                            cadena = cadena + '"' + temNombre + '":null,';
                        else {
                            cadena = cadena + '"' + temNombre + '":"' + temValue + '",';
                            tempContadorFiltros = tempContadorFiltros + 1;
                        }
                    }
                    else {
                        var temNombre = $("#" + item)[0].id;
                        var temValue = $("#" + item)[0].value;
                        item = { temNombre: temValue }

                        if (temValue == "")
                            cadena = cadena + '"' + temNombre + '":' + "null,";
                        else {
                            cadena = cadena + '"' + temNombre + '":"' + temValue + '",';
                            tempContadorFiltros = tempContadorFiltros + 1;
                        }
                    }
                }
            });

            cadena = cadena.substring(0, cadena.length - 1);
            cadena = cadena + "}";
            vm.filtro = cadena;


            var listaEntidades = "";
            vm.listaEntidades.forEach(ent => {
                if (listaEntidades == "")
                    listaEntidades = ent.IdEntidad;
                else
                    listaEntidades = listaEntidades + "," + ent.IdEntidad;
            })


            if (tempContadorFiltros == 0) {
                utilidades.mensajeError("Debe seleccionar un filtro.");
                return;
            }

            servicioConsolaReportes.ObtenerDatosReportePIIP(vm.idReporte, cadena, vm.idUsuario, detalleReporte, listaEntidades)
                .then(function (response) {
                    if (response.data != null) {
                        var resultadoReporte = jQuery.parseJSON(response.data);
                        JSONToCSVConvertor(resultadoReporte, "_" + Date.now(), true);
                        vm.cerrar();
                        utilidades.mensajeSuccess('Se genero el reporte.', false, null, null);
                    }
                    else
                        utilidades.mensajeError("Su consulta no arrojo resultados.");

                }, function (error) {
                    console.log("error: ObtenerDatosReportePIIP", error);
                    utilidades.mensajeError("Su consulta excedio el tiempo de respuesta.");
                });

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

        vm.validateFormat = function (event) {

            filtrar();

            const forbiddenChars = ['#', '$', '%', '^', '&', '*', '(', ')', '¡', '!', '"', '{', '}', '?', '+', '=', '/', '\'', '¿', '´', '[', ']', '¨', '^', '|', '<', '>', '¨', '\,', ';', '`']

            if (forbiddenChars.includes(event.key)) {
                console.log('Key prevented')
                event.preventDefault();
                return false;
            }
        }

        function check(e) {
            filtrar();
            tecla = (document.all) ? e.keyCode : e.which;

            //Tecla de retroceso para borrar, siempre la permite
            if (tecla == 8) {
                return true;
            }

            // Patrón de entrada, en este caso solo acepta numeros y letras
            patron = /[A-Za-z0-9-@]/;
            tecla_final = String.fromCharCode(tecla);
            return patron.test(tecla_final);
        }

        function filtrar() {
            var iconoLimpiar = document.getElementById('icoLimpiarBusqueda');
            iconoLimpiar.src = "Img/IcolimpiarBusq.svg";
        }

        function limpiarCamposFiltro() {
            vm.filtro = null;
            var iconoLimpiar = document.getElementById('icoLimpiarBusqueda');
            iconoLimpiar.src = "Img/IcolimpiarBusqD.svg";
            vm.tempNombreFiltros.forEach(item => {
                if ($("#" + item)[0] != undefined) {
                    if ($("#" + item)[0].type == 'select-one') {
                        var temNombre = $("#" + item)[0].id;
                        $("#" + temNombre)[0].value = "";
                    }
                    else {
                        var temNombre = $("#" + item)[0].id;
                        $("#" + temNombre)[0].value = "";
                    }
                }
            });
        }

    }

    angular.module('backbone').controller('modalFiltrosController', modalFiltrosController);

})();