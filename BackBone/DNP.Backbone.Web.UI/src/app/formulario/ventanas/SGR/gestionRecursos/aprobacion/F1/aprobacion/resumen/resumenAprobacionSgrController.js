(function () {
    'use strict';

    resumenAprobacionSgrController.$inject = [
        'utilidades',
        '$sessionStorage',
        'aprobacionSgrServicio'
    ];

    function resumenAprobacionSgrController(
        utilidades,
        $sessionStorage,
        aprobacionSgrServicio,
    ) {
        var vm = this;
        vm.user = {};
        vm.lang = "es";
        
        vm.nombreComponente = 'sgraprobacion1aprobacionregistroaprobacion';
        vm.nombreComponenteResumen = "sgraprobacion1aprobacionresumenaprobacion";
        vm.proyectoId = $sessionStorage.proyectoId;
        vm.data;

        // Método de inicialización
        vm.init = function () {
            aprobacionSgrServicio.ObtenerProyectoResumenAprobacionSGR(vm.proyectoId).then(
                function (res) {
                    var flat = res.data;
                    var grouped = {};                    
                        
                    flat.forEach(function (item) {
                        var key = item.EntidadApruebaTipo + '|' + item.EntidadApruebaNombre;

                        // 2) Formatear FechaAprobacion de "2025-05-22T15:34:03.917" a "dd/MM/yyyy"
                        var fechaFormateada = '';
                        if (item.FechaAprobacion) {
                            var fechaObj = new Date(item.FechaAprobacion);
                            var dia = String(fechaObj.getDate()).padStart(2, '0');
                            var mes = String(fechaObj.getMonth() + 1).padStart(2, '0'); // Mes base 0
                            var año = fechaObj.getFullYear();
                            fechaFormateada = dia + '/' + mes + '/' + año;
                        }

                        if (!grouped[key]) {
         
                            grouped[key] = {                                
                                EntidadApruebaTipo: item.EntidadApruebaTipo,
                                EntidadApruebaNombre: item.EntidadApruebaNombre,
                                EstadoAprobacion: item.EstadoAprobacion,
                                // Solo agregar FechaAprobacion si existe y tiene valor formateado
                                ...(fechaFormateada && { FechaAprobacion: fechaFormateada }),
                                Detalles: []
                            };
                        }
                        
                        
                        // Y añadimos el “detalle”
                        
                        grouped[key].Detalles.push({
                            Etapa: item.Etapa,
                            Entidad: item.TipoEntidad + ', ' + item.Entidad,                            
                            TipoRecurso: item.TipoRecurso,
                            Bienio: item.Bienio,
                            ValorSolicitado: item.ValorSolicitado,
                            //...(item.VigenciaFutura && { VigenciaFutura: item.VigenciaFutura }),
                            //...(item.ValorAprobado && { ValorAprobado: item.ValorAprobado }),
                            VigenciaFutura: item.VigenciaFutura,
                            ValorAprobado: typeof item.ValorAprobadoCredito !== 'undefined' ? item.ValorAprobadoCredito == 0.0 ? item.ValorAprobado : item.ValorSolicitado - item.ValorAprobadoCredito : ' ',
                            ValorDiferencia: typeof item.ValorAprobadoCredito !== 'undefined' ? item.ValorAprobadoCredito == 0.0 ? ' ' : item.ValorAprobadoCredito : ' '
                        });
                        
                        
                    });
                    
                    // 3) Convertimos el objeto agrupado a un array
                    vm.data = Object.values(grouped);
                }
            );
        };

        function ConvertirNumero(numero) {
            if (typeof (numero) == 'number') {
                return new Intl.NumberFormat('es-co', {
                    minimumFractionDigits: 2,
                }).format(numero);
            } else {
                return numero;
            };
        }

    }

    angular.module('backbone').component('resumenAprobacionSgr', {
        templateUrl: "/src/app/formulario/ventanas/SGR/gestionRecursos/aprobacion/F1/aprobacion/resumen/resumenAprobacionSgr.html",
        controller: resumenAprobacionSgrController,
        controllerAs: "vm",
    });

})();
