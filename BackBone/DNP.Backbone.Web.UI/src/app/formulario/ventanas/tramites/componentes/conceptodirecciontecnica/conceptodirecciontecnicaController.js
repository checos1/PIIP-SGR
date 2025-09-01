(function () {
    'use strict';

    conceptodirecciontecnicaController.$inject = [
        'backboneServicios',
        'sesionServicios',
        'configurarEntidadRolSectorServicio',
        '$scope',
        'utilidades',
        'constantesCondicionFiltro',
        '$sessionStorage',
        'conceptodirecciontecnicaServicio',
        'constantesBackbone',
        '$routeParams',
        'servicioResumenDeProyectos',
        'uiGridConstants',
        '$timeout',
        '$location'
    ];



    function conceptodirecciontecnicaController(
        backboneServicios,
        sesionServicios,
        configurarEntidadRolSectorServicio,
        $scope,
        utilidades,
        constantesCondicionFiltro,
        $sessionStorage,
        conceptodirecciontecnicaServicio,
        constantesBackbone,
        $routeParams,
        servicioResumenDeProyectos,
        uiGridConstants,
        $timeout,
        $location
    ) {
        var vm = this;
        vm.init = init;
        vm.lang = "es";
        vm.Observaciones = "";
        vm.fechaHoy = formatearFecha(obtenerFechaSinHoras(new Date()));


        //vm.ConceptoDireccionTecnicaTramite = [];

        vm.peticion = {
            //IdUsuario: usuarioDNP,
            TramiteId: $sessionStorage.TramiteId,
            //Aplicacion: nombreAplicacionBackbone,
            //ListaIdsRoles: sesionServicios.obtenerUsuarioIdsRoles(),
            //IdFiltro: vm.EntityTypeCatalogOptionId,
            NivelId: constantesBackbone.idfaseControlPosteriorTramites,//'E8FC3694-C566-4944-A487-DAA494EB3581'
        };

        vm.ConceptoDireccionTecnicaTramite = [{
            FaseId: 0,
            ProjectId: 0,
            InstanciaId: 0,
            FormularioId: 0,
            CR: 0,
            AgregarRequisitos: "",
            Usuario: "",
            Fecha: "",
            Observaciones: "",
            Cumple: 0,// Boolean
            Definitivo: 0,// Boolean
            PreguntaId: 0,
            Pregunta: "",
            Respuesta: "",
            ObservacionPregunta: "",
            OpcionesRespuesta: [
                {
                    OpcionId: 0,
                    ValorOpcion: "",
                }
            ],
            NombreRol: "",
            NombreNivel: "",
            CuestionarioProyectoId: 0,
            TramiteId: 0
        }];


        function init() {
            ObtenerConceptoDireccionTecnicaTramite();
            //ObtenerObservacion();

        }

        function ObtenerConceptoDireccionTecnicaTramite() {
            conceptodirecciontecnicaServicio.ObtenerConceptoDireccionTecnicaTramite(vm.peticion)
                .then(resultado => {
                    vm.ConceptoDireccionTecnicaTramite = resultado.data;
                    vm.ConceptoDireccionTecnicaTramite.forEach(dt => {
                        vm.Observaciones = dt.Observaciones;
                    });
                })
        }
        function ObtenerObservacion() {
            vm.ConceptoDireccionTecnicaTramite.forEach(dt => {
                vm.Observaciones = dt.Observaciones;
            });
        }


        vm.GuardarPreguntasConceptoDireccionTecnica = function (response) {
            validarCampos();
            if (vm.cumple === true) {
                if (vm.ConceptoDireccionTecnicaTramite[0].InstanciaId == null) {
                    CargarParametrosConcepto();
                }
                conceptodirecciontecnicaServicio.GuardarConceptoDireccionTecnicaTramite(vm.ConceptoDireccionTecnicaTramite)
                    .then(function (response) {
                        if (response.data) {
                            utilidades.mensajeSuccess("Operación realizada con éxito!", false, false, false);
                            vm.versolicitarconcepto = false;
                            vm.callback({ arg: false });
                        } else {
                            swal('', "Error al realizar la operación", 'error');
                            vm.callback({ arg: true });
                        }
                    });
            }
            else {
                utilidades.mensajeError('El formulario no esta diligenciado en su totalidad');
                //swal('El formulario no esta diligenciado en su totalidad', 'Revise las campos nuevamente.', 'warning');
            }
        }
        function CargarParametrosConcepto() {
            vm.ConceptoDireccionTecnicaTramite.forEach(preguntas => {
                preguntas.InstanciaId = constantesBackbone.idfaseControlPosteriorTramites;
                preguntas.FormularioId = '00000000-0000-0000-0000-000000000000';
                preguntas.CR = 1;
                preguntas.AgregarRequisitos = false;
                preguntas.Usuario = $sessionStorage.concepto.IdUsuarioDNP;
                preguntas.Fecha = vm.fechaHoy;
                preguntas.Cumple = true;
                preguntas.ObservacionPregunta = vm.ConceptoDireccionTecnicaTramite[0].ObservacionPregunta;
            });
        }

        function validarCampos() {
            vm.cumple = true;

            var i = 0
            vm.ConceptoDireccionTecnicaTramite.forEach(preguntas => {
                preguntas.ObservacionPregunta = vm.ConceptoDireccionTecnicaTramite[0].ObservacionPregunta;                
                if (i == 0) {
                    if (preguntas.ObservacionPregunta == null || preguntas.ObservacionPregunta == '') {
                        vm.cumple = false;
                        i = 1;
                        return vm.cumple;
                    }

                }
                if (preguntas.Respuesta == '0' || preguntas.Respuesta == null) {
                    vm.cumple = false;
                }
                i = 1;
            });

            return vm.cumple
        }

        function formatearFecha(fecha) {
            let fechaString = fecha.toISOString();
            return fechaString.substring(0, 19);
        }
        function obtenerFechaSinHoras(fecha) {
            return new Date(Date.UTC(fecha.getFullYear(), fecha.getMonth(), fecha.getDate(), fecha.getHours(), fecha.getMinutes(), fecha.getSeconds()));
        }

    }

    angular.module('backbone').component('conceptodirecciontecnica', {

        templateUrl: "src/app/formulario/ventanas/tramites/componentes/conceptodirecciontecnica/conceptodirecciontecnica.html",
        controller: conceptodirecciontecnicaController,
        controllerAs: "vm",
        bindings: {
            disabled: '=',
            callback: '&'
        }
    });

})();