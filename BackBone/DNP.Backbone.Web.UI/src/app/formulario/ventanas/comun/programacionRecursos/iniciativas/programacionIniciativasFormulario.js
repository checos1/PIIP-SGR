(function () {
    'use strict';

    programacionIniciativasFormulario.$inject = [
        '$scope',
        '$sessionStorage',
        'utilidades',
        '$timeout',
        'localizacionJustificacionServicio',
        'comunesServicio'
    ];

    function programacionIniciativasFormulario(
        $scope,
        $sessionStorage,
        utilidades,
        $timeout,
        localizacionJustificacionServicio,
        comunesServicio
    ) {
        ///*Varibales */
        var vm = this;
        vm.lang = "es";
        vm.nombreComponente = "iniciativasiniciativasinversion";
        vm.seccionCapitulo = null;

        vm.habilitaBotones = !$sessionStorage.soloLectura ? true : false;// habilita solo en paso 1

        /*declara metodos*/
        vm.AgregarIniciativa = AgregarIniciativa;
        vm.EliminarIniciativa = EliminarIniciativa;
        vm.cambioIniciativa = cambioIniciativa;

        $scope.$watch('vm.tramiteproyectoid', function () {
            if (vm.tramiteproyectoid != '') {
                ObtenerDatosProgramacionDetalle();
                obtenerDepartamentos();
            }            
        });

        $scope.$watch('vm.calendarioiniciativas', function () {
            if (vm.calendarioiniciativas !== undefined && vm.calendarioiniciativas !== '')
                vm.habilitaBotones = vm.calendarioiniciativas === 'true' && !$sessionStorage.soloLectura ? true : false;
        });

        $scope.$watch('vm.modificardistribucion', function () {
            if (vm.modificardistribucion === '4') {
                ObtenerDatosProgramacionDetalle();
                vm.modificardistribucion = '0';
            }

        });      

        vm.init = function () {
            obtenerTipologias();
        };

        function ObtenerDatosProgramacionDetalle() {
            return comunesServicio.ObtenerDatosProgramacionDetalle(vm.tramiteproyectoid, vm.origen).then(
                function (respuesta) {
                    if (respuesta.data !== '') {
                        $scope.iniciativa = jQuery.parseJSON(jQuery.parseJSON(respuesta.data));
                    }
                    else {
                        $scope.iniciativa = [];
                    }
                });
        }

        function obtenerDepartamentos() {
            var jsonCondicion = '{"TramiteProyectoId":' + vm.tramiteproyectoid  + '}';
            return comunesServicio.ObtenerTablasBasicas(jsonCondicion, 'DepartamentosPorProyecto')
                .then(respuesta => {
                    if (!respuesta.data)
                        return;

                    var departamento = jQuery.parseJSON(jQuery.parseJSON(respuesta.data));
                    vm.listaDepartamento = departamento.registros;
                })
                .catch(error => {
                    utilidades.mensajeError("", null, "Hubo un error al cargar los Departamentos");
                });
        }

        function obtenerTipologias() {
            var jsonCondicion = '{}';
            return comunesServicio.ObtenerTablasBasicas(jsonCondicion, 'Transversal.Tipologias')
                .then(respuesta => {
                    if (!respuesta.data)
                        return;

                    var tipologia = jQuery.parseJSON(jQuery.parseJSON(respuesta.data));
                    vm.listaTipologia = tipologia.registros;
                })
                .catch(error => {
                    utilidades.mensajeError("", null, "Hubo un error al cargar las Tipologías");
                });
        }

        function cambioIniciativa() {           

            if (vm.Departamento == '' || vm.Departamento == undefined) {
                vm.Iniciativa = null;
                return false;
            }

            if (vm.Tipologia == '' || vm.Tipologia == undefined) {
                vm.Iniciativa = null;
                return false;
            }

            var deptoid = vm.Departamento.Id;
            var tipid = vm.Tipologia.id;
            var jsonCondicion = '{"TipoLogiaId": ' + tipid + ',"Departmentid": ' + deptoid + '}';

            return comunesServicio.ObtenerTablasBasicas(jsonCondicion, 'TransversalIniciativas')
                .then(respuesta => {
                    if (!respuesta.data)
                        return;

                    var iniciativa = jQuery.parseJSON(jQuery.parseJSON(respuesta.data));
                    vm.listaIniciativa = iniciativa.registros;
                })
                .catch(error => {
                    utilidades.mensajeError("", null, "Hubo un error al cargar las Iniciativas");
                });
        }

        function AgregarIniciativa() {

            if (vm.Departamento == '' || vm.Departamento == undefined) {
                utilidades.mensajeError('Debe seleccionar un Departamento.');
                return false;
            }

            if (vm.Tipologia == '' || vm.Tipologia == undefined) {
                utilidades.mensajeError('Debe seleccionar una Tipología.');
                return false;
            }

            if (vm.Iniciativa == '' || vm.Iniciativa == undefined) {
                utilidades.mensajeError('Debe seleccionar una Iniciativa.');
                return false;
            }

            var iniId = null;

            if (vm.Iniciativa != undefined) {
                iniId = vm.Iniciativa.id;
            }

            var validadionIniciativa = $scope.iniciativa.Iniciativas.filter(x => x.IniciativaId == iniId);

            if (validadionIniciativa != undefined) {
                if (validadionIniciativa.length > 0) {
                    utilidades.mensajeError('La iniciativa ya existe.');
                    return false;
                }
            }

            let Iniciativa = {};
            let ValoresIniciativa = [];

            let valores = {
                IniciativaId: iniId,
            };

            ValoresIniciativa.push(valores);

            ObtenerSeccionCapitulo();
            Iniciativa.TramiteProyectoId = vm.tramiteproyectoid;
            Iniciativa.SeccionCapitulo = vm.seccionCapitulo;
            Iniciativa.Iniciativa = ValoresIniciativa;

            return comunesServicio.GuardarDatosProgramacionIniciativa(Iniciativa).then(
                function (respuesta) {
                    if (respuesta.data && (respuesta.statusText === "OK" || respuesta.status === 200)) {
                        if (respuesta.data.Exito) {
                            vm.callback({ botonDevolver: false, botonSiguiente: false, ocultarDevolver: true });
                            utilidades.mensajeSuccess("", false, false, false, "Los datos han sido agregados y guardados con éxito");
                            ObtenerDatosProgramacionDetalle();
                            vm.init();
                        }
                        else {
                            utilidades.mensajeError(respuesta.data.Mensaje);
                        }
                    } else {
                        utilidades.mensajeError("", null, "Error al realizar la operación");
                    }
                });
        }

        //Se elimina a nivel de vista; se eliminan nuevos y existentes que no existan en vigencias futuras
        function EliminarIniciativa(Id) {
            utilidades.mensajeWarning("¿Está seguro de continuar?", function funcionContinuar() {

                let Iniciativa = {};
                let ValoresIniciativa = [];

                let valores = {
                    Id: Id,
                    IniciativaId: 0
                };

                ValoresIniciativa.push(valores);

                ObtenerSeccionCapitulo();
                Iniciativa.TramiteProyectoId = vm.tramiteproyectoid;
                Iniciativa.SeccionCapitulo = vm.seccionCapitulo;
                Iniciativa.Iniciativa = ValoresIniciativa;

                return comunesServicio.GuardarDatosProgramacionIniciativa(Iniciativa).then(
                    function (respuesta) {
                        if (respuesta.data && (respuesta.statusText === "OK" || respuesta.status === 200)) {
                            if (respuesta.data.Exito) {
                                vm.callback({ botonDevolver: false, botonSiguiente: false, ocultarDevolver: true });
                                utilidades.mensajeSuccess("", false, false, false, "Los datos han sido eliminados con éxito.");
                                ObtenerDatosProgramacionDetalle();
                                vm.init();
                            }
                            else {
                                utilidades.mensajeError(respuesta.data.Mensaje);
                            }
                        } else {
                            utilidades.mensajeError("", null, "Error al realizar la operación");
                        }
                    });

            }, function funcionCancelar(reason) {
            }, null, null, "La línea seleccionada será eliminada de la tabla");
        }

        //para guardar los capitulos modificados y que se llenen las lunas

        function ObtenerSeccionCapitulo() {
            const span = document.getElementById('id-capitulo-' + vm.nombreComponente);
            vm.seccionCapitulo = span.textContent;
        }
    }

    angular.module('backbone').component('programacionIniciativasFormulario', {

        templateUrl: "src/app/formulario/ventanas/comun/programacionRecursos/iniciativas/programacionIniciativasFormulario.html",
        controller: programacionIniciativasFormulario,
        controllerAs: "vm",
        bindings: {
            tramiteproyectoid: '@',
            origen: '@',
            guardadoevent: '&',
            notificacioncambios: '&',
            notificacionvalidacion: '&',
            notificacionestado: '&',
            notificarrefresco: '&',
            callback: '&',
            actualizacomponentes: '@',
            calendarioiniciativas: '@',
            modificardistribucion:'=',
        }
    })
})();
