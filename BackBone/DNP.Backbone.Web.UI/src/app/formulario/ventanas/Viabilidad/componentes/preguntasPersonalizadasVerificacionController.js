(function () {
    'use strict';

    preguntasPersonalizadasVerificacionController.$inject = [
        '$scope',
        '$sessionStorage',
        'viabilidadServicio'
    ];

    function preguntasPersonalizadasVerificacionController(
        $scope,
        $sessionStorage,
        viabilidadServicio
    ) {
        var vm = this;
        vm.user = {};
        vm.lang = "es";

        vm.ProyectoId = $sessionStorage.idObjetoNegocio;
        vm.IdNivel = $sessionStorage.idNivel;
        vm.idInstancia = $sessionStorage.idInstancia;
        vm.DescripcionAccionNivel = $sessionStorage.DescripcionAccionNivel;
        vm.validaHabilita = validaHabilita;
        vm.cambiarTab = cambiarTab;

        vm.mostrarFlujo = false;
        vm.mostrarTecnico = false;   

        vm.revisiones;

        vm.DatosGeneralesProyectos = [{
            ProyectoId: 0,
            NombreProyecto: "",
            BPIN: "",
            EntidadId: 0,
            Entidad: "",
            SectorId: 0,
            Sector: "",
            EstadoId: 0,
            Estado: "",
            Horizonte: "",
            Valor: 0
        }];

        vm.ConfiguracionEntidades = [{
            ProyectoId: 0,
            FaseId: 0,
            Fase: "",
            AplicaTecnico: ""
        }];

        vm.init = function () {
            obtenerDatosGeneralesProyecto();
            obtenerConfiguracionEntidades();
            cargarLogs();
        };

        function obtenerDatosGeneralesProyecto() {
            return viabilidadServicio.obtenerDatosGeneralesProyecto(vm.ProyectoId, vm.IdNivel).then(
                function (respuesta) {
                    vm.DatosGeneralesProyectos = respuesta.data;
                    $sessionStorage.Entidad = respuesta.data.Entidad;
                }
            );
        }

        function obtenerConfiguracionEntidades() {
            return viabilidadServicio.obtenerConfiguracionEntidades(vm.ProyectoId, vm.IdNivel).then(
                function (respuesta) {
                    vm.ConfiguracionEntidades = respuesta.data;
                    $sessionStorage.Fase = vm.ConfiguracionEntidades.Fase;
                    $sessionStorage.AplicaTecnico = false;
                    if (respuesta.data.AplicaTecnico == "SI") {
                        vm.mostrarTecnico = true;
                        $sessionStorage.AplicaTecnico = true;
                        vm.modeloPreguntas = { perfilTecnico: true };
                    }
                    else {
                        $("#liTecnico").removeClass("active");
                        $("#liLider").addClass("active");
                    }
                }
            );
        }

        function cargarLogs() {
            viabilidadServicio.obtener(vm.idInstancia, "00000000-0000-0000-0000-000000000000").then((result) => {
                
                var nivel = "";
                var contador = 0;
                result.data.forEach(revision => {
                    if (nivel != revision.NivelId) {
                        contador++;
                    }
                    nivel = revision.NivelId;
                });

                vm.revisiones = contador;
            });
        }

        //Métodos
        vm.mostrarOcultarFlujo = mostrarOcultarFlujo;

        function mostrarOcultarFlujo() {
            vm.mostrarFlujo = !vm.mostrarFlujo;
            if (vm.mostrarFlujo) {
                $("#ver").html('Ocultar qué es esto');
            }
            else {
                $("#ver").html('Ver qué es esto');
            }            
        }  

        function validaHabilita(arg, aprueba) {
            vm.callback({ arg: arg, aprueba: aprueba });
        }

        function cambiarTab() {          
            $('.nav-tabs a[href="#FaseLider"]').tab('show');
        }
    }

    angular.module('backbone').component('preguntasPersonalizadasVerificacion', {
        templateUrl: "src/app/formulario/ventanas/Viabilidad/componentes/preguntasPersonalizadasVerificacion.html",
        controller: preguntasPersonalizadasVerificacionController,
        controllerAs: "vm",
        bindings: {
            callback: '&'
        }
    });
})();