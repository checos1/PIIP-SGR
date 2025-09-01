(function () {
    'use strict'; 

    resumenCreditoAprobacionSgrController.$inject = [        
        '$sessionStorage',
        'aprobacionSgrServicio'
    ];

    function resumenCreditoAprobacionSgrController(        
        $sessionStorage,
        aprobacionSgrServicio,
    ) {
        var vm = this;
        vm.user = {};
        vm.lang = "es";        
        vm.nombreComponente = 'sgraprobacion1aprobacionopcresumenaprobacionopc';        
        vm.proyectoId = $sessionStorage.proyectoId;
        vm.data;

        // Método de inicialización
        vm.init = function () {
            aprobacionSgrServicio
                .ObtenerProyectoResumenEstadoAprobacionCreditoSGR(vm.proyectoId)
                .then(function (res) {
                    var raw = res.data;

                    // Filtramos entidades con al menos un detalle válido
                    vm.data = raw.map(function (entidad) {
                        // Filtrar los detalles con ValorCreditoAprobado > 0
                        var detallesValidos = entidad.Detalles.filter(function (det) {
                            return det.ValorCreditoOPC > 0;
                        });

                        // Si no hay detalles válidos, se omite más adelante
                        var gruposPorEtapa = {};

                        detallesValidos.forEach(function (det) {
                            var key = det.Etapa + '|' + det.EntidadSolicita + '|' + det.TipoRecurso + '|' + det.BienioSolicitado;

                            if (!gruposPorEtapa[key]) {
                                gruposPorEtapa[key] = {
                                    Etapa: det.Etapa,
                                    EntidadSolicita: det.EntidadSolicita,
                                    TipoEntidadSolicita: det.TipoEntidadSolicita,
                                    TipoRecurso: det.TipoRecurso,
                                    BienioSolicitado: det.BienioSolicitado,
                                    Detalles: []
                                };
                            }

                            gruposPorEtapa[key].Detalles.push(det);
                        });

                        entidad.Grupos = Object.values(gruposPorEtapa);
                        if (entidad.FechaAprobacion)
                            entidad.FechaAprobacion = entidad.FechaAprobacion.split('T')[0];
                        return entidad;
                    })
                        // Filtramos las entidades que no tienen grupos válidos
                        .filter(function (entidad) {
                            return entidad.Grupos.length > 0;
                        });
                })
                .catch(function (err) {
                    console.error(err);
                });
        };



    }

    angular.module('backbone').component('resumenCreditoAprobacionSgr', {
        templateUrl: "/src/app/formulario/ventanas/SGR/gestionRecursos/aprobacion/F1/aprobacion/resumencredito/resumenCreditoAprobacionSgr.html",
        controller: resumenCreditoAprobacionSgrController,
        controllerAs: "vm",
    });

})();
