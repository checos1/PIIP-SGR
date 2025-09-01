(function () {
    'use strict';

    controladorEjemplo.$inject = [
        'utilidades',
        '$uibModal',
        '$scope'
    ];

    function controladorEjemplo(utilidades, $uibModal, $scope) {
        var vm = this;
        vm.bande = "es";
        vm.Bandera1 = "+";
        vm.Bandera2 = "+";
        vm.Bandera3 = "+";
        vm.Bandera4 = "+";
        vm.BanderaTa = "+";
        vm.BanderaTa2 = "+";
        vm.TextoAccordeon = "Obtener información técnica eficiente para dar viabilidad a nuevos proyectos de construcción y ampliación de cupos";
        vm.arrowIcoDown = "glyphicon-chevron-down";
        vm.arrowIcoUp = "glyphicon-chevron-up";
        vm.arrowIcoDown2 = "glyphicon-chevron-down-busqDNP";
        vm.arrowIcoUp2 = "glyphicon-chevron-up-busqDNP";
        vm.HabilitaEditarBandera = false;
        vm.HabilitaEditar = HabilitaEditar;
        vm.ConvertirNumero2decimales = ConvertirNumero2decimales;
        vm.ConvertirNumero4decimales = ConvertirNumero4decimales;
        vm.abrirModalDocumentosAdjuntos = abrirModalDocumentosAdjuntos;
        vm.abrirModalInstanciaDocumentos = abrirModalInstanciaDocumentos;
        vm.BanderaDivPerfiles = false;
        vm.BanderaDivPerfiles2 = true;

        vm.MostrarDivPerfilesUsuario = function (idbandera) {
            if (idbandera == 1) {
                vm.BanderaDivPerfiles = true;
            }
            else {
                vm.BanderaDivPerfiles = false;
            }
        }
        vm.AbrilNivel1 = function () {
            if (vm.Bandera1 == '+') {
                vm.Bandera1 = '-'
            } else {
                vm.Bandera1 = '+'
            }
        }
        vm.AbrilNivel2 = function () {
            if (vm.Bandera2 == '+') {
                vm.Bandera2 = '-'
            } else {
                vm.Bandera2 = '+'
            }
        }
        vm.AbrilNivel3 = function () {
            if (vm.Bandera3 == '+') {
                vm.Bandera3 = '-'
            } else {
                vm.Bandera3 = '+'
            }
        }
        vm.AbrilNivel4 = function () {
            if (vm.Bandera4 == '+') {
                vm.Bandera4 = '-'
            } else {
                vm.Bandera4 = '+'
            }
        }
        vm.AbrilNivelTabla = function (fila) {
            if (fila == 1) {
                if (vm.BanderaTa == '+') {
                    vm.BanderaTa = '-'
                } else {
                    vm.BanderaTa = '+'
                }
            }
            if (fila == 2) {
                if (vm.BanderaTa2 == '+') {
                    vm.BanderaTa2 = '-'
                } else {
                    vm.BanderaTa2 = '+'
                }
            }
        }
        vm.changeArrow = function (nombreModificado) {
            var idSpanArrow = 'arrow-' + nombreModificado;
            var arrowCapitulo = document.getElementById(idSpanArrow);
            var arrowClasses = arrowCapitulo.classList;
            for (var i = 0; i < arrowClasses.length; i++) {
                if (arrowClasses[i] == vm.arrowIcoDown) {
                    document.getElementById(idSpanArrow).classList.add(vm.arrowIcoUp);
                    document.getElementById(idSpanArrow).classList.remove(vm.arrowIcoDown);
                    break;
                } else if (arrowClasses[i] == vm.arrowIcoUp) {
                    document.getElementById(idSpanArrow).classList.add(vm.arrowIcoDown);
                    document.getElementById(idSpanArrow).classList.remove(vm.arrowIcoUp);
                    break;
                }

            }
        }
        vm.changeArrow2 = function (nombreModificado) {
            var idSpanArrow = 'arrow-' + nombreModificado;
            var arrowCapitulo = document.getElementById(idSpanArrow);
            var arrowClasses = arrowCapitulo.classList;
            for (var i = 0; i < arrowClasses.length; i++) {
                if (arrowClasses[i] == vm.arrowIcoDown2) {
                    document.getElementById(idSpanArrow).src = "Img/IcoBusqUp.svg";
                    document.getElementById(idSpanArrow).classList.add(vm.arrowIcoUp2);
                    document.getElementById(idSpanArrow).classList.remove(vm.arrowIcoDown2);
                    break;
                } else if (arrowClasses[i] == vm.arrowIcoUp2) {
                    document.getElementById(idSpanArrow).src = "Img/IcoBusqDow.svg";
                    document.getElementById(idSpanArrow).classList.add(vm.arrowIcoDown2);
                    document.getElementById(idSpanArrow).classList.remove(vm.arrowIcoUp2);
                    break;
                }
            }
        }
        function HabilitaEditar(band) {

            vm.HabilitaEditarBandera = band;
        }
        function ConvertirNumero2decimales(numero) {
            return utilidades.ConverNum2Decimal(numero);
        }
        function ConvertirNumero4decimales(numero) {
            return utilidades.ConverNum4Decimal(numero);
        }
        function abrirModalDocumentosAdjuntos(row) {

          
            var modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'src/app/inicio/modalEjemplo.html',
                controller: 'modalEjemploController',
                controllerAs: "vm",
                openedClass: "consola-modal-soportesDNP",
                //size: 'lg',
                resolve: {
                    objProyecto: row
                }
               
            });

            modalInstance.result.then(function (selectedItem) {


            }, function () {

            });
        }
        function abrirModalInstanciaDocumentos(row) {

          
            var modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'src/app/inicio/modal2Ejemplo.html',
                controller: 'modal2EjemploController',
                controllerAs: "vm",
                openedClass: "consola-modal-soportesDNP",
                //size: 'lg',
                resolve: {
                    objProyecto: row
                }

            });

            modalInstance.result.then(function (selectedItem) {


            }, function () {

            });
        }

    }

    angular.module('backbone').controller('controladorEjemplo', controladorEjemplo);

})();


