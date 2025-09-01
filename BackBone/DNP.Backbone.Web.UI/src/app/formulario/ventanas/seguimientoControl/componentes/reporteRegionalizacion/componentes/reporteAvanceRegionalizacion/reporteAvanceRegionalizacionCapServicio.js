(function () {
    'use strict';
    angular.module('backbone').factory('reporteAvanceRegionalizacionCapServicio', reporteAvanceRegionalizacionCapServicio);

    reporteAvanceRegionalizacionCapServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function reporteAvanceRegionalizacionCapServicio($q, $http, $location, constantesBackbone) {
        return {
            consultarAvanceRegionalizacion: consultarAvanceRegionalizacion,
            guardarAvanceRegionalizacion: guardarAvanceRegionalizacion,
            guardarAvanceRegionalizacionMasivo: guardarAvanceRegionalizacionMasivo,
            ObtenerDetalleRegionalizacionProgramacionSeguimiento: ObtenerDetalleRegionalizacionProgramacionSeguimiento
        };

        function guardarAvanceRegionalizacion(localizacion, proyectoId, bpin) {

            var fuentes = [];
            var objetivos = [];
            var productos = [];
            var localizaciones = [];

            localizaciones.push({ LocalizacionId: localizacion.localizacionId, RecursosPeriodosActivos: localizacion.detalleRegionalizacion.RecursosPeriodosActivos, MetasPeriodosActivos: localizacion.detalleRegionalizacion.MetasPeriodosActivos });
            productos.push({ ProductoId: localizacion.ProductoId, Localizaciones: localizaciones });
            objetivos.push({ ObjetivoEspecificoId: localizacion.ObjetivoEspecificoId, Productos: productos });
            fuentes.push({ FuenteId: localizacion.FuenteId, Objetivos: objetivos });

            const avanceMetaRegionalizacionDto = {
                ProyectoId: proyectoId,
                bpin: bpin,
                Fuentes: fuentes
            };

            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarAvanceRegionalizacion;
            return $http.post(url, avanceMetaRegionalizacionDto);

        }

        function guardarAvanceRegionalizacionMasivo(localizacion, proyectoId, bpin) {

            var fuentes = [];
            var objetivos = [];
            var productos = [];

            if (localizacion.Objetivos !== undefined) {
                for (var i = 0; i < localizacion.Objetivos.length; i++) {

                    localizacion.Objetivos[i].Productos.forEach(Indi => {
                        productos.push({ NumeroProducto: Indi.NumeroProducto, ProductoId: Indi.ProductoId, localizaciones: Indi.Localizaciones });
                    });
                }

                localizacion.Objetivos.forEach(Indi => {
                    objetivos.push({ ObjetivoEspecificoId: Indi.ObjetivoEspecificoId, Productos: productos });
                });

                fuentes.push({ FuenteId: localizacion.FuenteId, Objetivos: objetivos });

                const avanceMetaRegionalizacionDto = {
                    ProyectoId: proyectoId,
                    bpin: bpin,
                    Fuentes: fuentes
                };

                var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarAvanceRegionalizacion;
                return $http.post(url, avanceMetaRegionalizacionDto);
            } else {

                localizacion.forEach(fuente => {
                    fuentes.push({ FuenteId: fuente.FuenteId, Objetivos: fuente.Objetivos });

                    fuente.Objetivos.forEach(objetivo => {
                        objetivos.push({ ObjetivoEspecificoId: objetivo.ObjetivoEspecificoId, Productos: objetivo.Productos });

                        objetivo.Productos.forEach(producto => {
                            productos.push({ NumeroProducto: producto.NumeroProducto, ProductoId: producto.ProductoId, localizaciones: producto.Localizaciones });
                        });
                    });

                });


                const avanceMetaRegionalizacionDto = {
                    ProyectoId: proyectoId,
                    bpin: bpin,
                    Fuentes: fuentes
                };

                var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarAvanceRegionalizacion;
                return $http.post(url, avanceMetaRegionalizacionDto);
            }

        }

        function consultarAvanceRegionalizacion(objetoParametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneConsultarAvanceRegionalizacion;
            url += "?instanciaId=" + objetoParametros.instanciaId;
            url += "&proyectoId=" + objetoParametros.proyectoId;
            url += "&codigoBpin=" + objetoParametros.codigoBpin;
            url += "&vigencia=" + objetoParametros.vigencia;
            url += "&periodoPeriodicidad=" + objetoParametros.periodoPeriodicidad;
            return $http.get(url);
        }

        function ObtenerDetalleRegionalizacionProgramacionSeguimiento(json, usuarioDNP) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerDetalleRegionalizacionProgramacionSeguimiento + "?json=" + json + "&usuarioDNP=" + usuarioDNP;
            return $http.get(url);
        }
    }
})();