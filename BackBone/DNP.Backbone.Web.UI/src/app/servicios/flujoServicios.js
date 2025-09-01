(function (apiFlujosServiciosBaseUri, apiMotorFlujos, nombreAplicacionBackbone, proyectosEstados) {
    'use strict';

    angular.module('backbone').factory('flujoServicios', flujoServicios);

    flujoServicios.$inject = ['$http', 'utilidades', 'sesionServicios', 'estadoAplicacionServicios', 'constantesBackbone'];

    function flujoServicios($http, utilidades, sesionServicios, estadoAplicacionServicios, constantesBackbone) {

        return {
            obtenerFlujosPorRoles: obtenerFlujosPorRoles,
           // obtenerFlujosPorMacroproceso: obtenerFlujosPorMacroproceso, No se llama en la aplicacion
            crearInstancia: crearInstancia,
            obtenerProyectosPorEntidadesyEstados: obtenerProyectosPorEntidadesyEstados,
            obtenerLogInstancia: obtenerLogInstancia,
            obtenerPdfLogInstancias: obtenerPdfLogInstancias,
            obtenerEntidades,
            generarInstancia: generarInstancia,
            generarInstanciaMasivo: generarInstanciaMasivo,
            ExisteFlujoProgramacion: ExisteFlujoProgramacion,
            Flujos_SubPasoEjecutar: Flujos_SubPasoEjecutar,
            Flujos_SubPasosValidar: Flujos_SubPasosValidar,
            obtenerProyectosSGRApriorizar: obtenerProyectosSGRApriorizar,
            obtenerProyectosPorEntidadesyEstadosSGR: obtenerProyectosPorEntidadesyEstadosSGR,
            TieneInstanciaActiva: TieneInstanciaActiva,
            GuardarPermisosPriorizionProyectoDetalleSGR: GuardarPermisosPriorizionProyectoDetalleSGR,
            GuardarProyectoPermisosProcesoSGR: GuardarProyectoPermisosProcesoSGR
        };

        ////////////

        function obtenerFlujosPorRoles(idsRoles, idMacroproceso) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiFlujosObtenerFlujosPorRoles;
            //var url = apiMotorFlujos + 'api/Flujo/ObtenerFlujosPorRoles';

            if (!idsRoles) {
                idsRoles = sesionServicios.obtenerUsuarioIdsRoles();
            }

            var configuracion = {
                params: {
                    idAplicacion: nombreAplicacionBackbone,
                    IdsRoles: idsRoles
                }
            }

            return $http.get(url, configuracion)
                .then(utilidades.httpRequestComplete);
        }

        //, No se llama en la aplicacion
        //function obtenerFlujosPorMacroproceso(idMacroproceso, IdUsuarioDNP) {
        //    var url = apiBackboneServicioBaseUri + constantesBackbone.apiFlujosObtenerFlujosPorRoles;
        //    //var url = apiMotorFlujos + 'api/Flujo/ObtenerFlujosPorRoles';

        //    var idsRoles = sesionServicios.obtenerUsuarioIdsRoles();

        //    var configuracion = {
        //        params: {
        //            idAplicacion: nombreAplicacionBackbone,
        //            IdMacroproceso: idMacroproceso,
        //            IdUsuario: IdUsuarioDNP
                    
        //        }
        //    }

        //    return $http.get(url, configuracion)
        //        .then(utilidades.httpRequestComplete);
        //}

        function crearInstancia(instanciaDto) {
           
            //var url = apiMotorFlujos + 'api/Flujo/CrearInstancia';
           var url = apiBackboneServicioBaseUri + constantesBackbone.apiFlujosCrearInstancia;

            return $http.post(url, instanciaDto)
                .then(utilidades.httpRequestComplete, utilidades.httpRequestErrorReject)
                .then(cuandoSeCreaInstancia);

            ////////////

            function cuandoSeCreaInstancia(data) {
                estadoAplicacionServicios.emitirEvento(constantesBackbone.eventoInstanciaCreada, instanciaDto.TipoObjetoId);

                return data;
            }
        }
        

        function generarInstancia(instanciaDto) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGenerarInstancias;
           

            return $http.post(url, instanciaDto)
                .then(utilidades.httpRequestComplete, utilidades.httpRequestErrorReject)
                .then(cuandoSeCreaInstancia);

            ////////////

            function cuandoSeCreaInstancia(data) {
                estadoAplicacionServicios.emitirEvento(constantesBackbone.eventoInstanciaCreada, instanciaDto.TipoObjetoId);
                return data;
            }
        }
        

        function generarInstanciaMasivo(listainstanciaDto) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGenerarInstanciasMasiva;
           // var url = apiMotorFlujos + 'api/Flujo/GenerarInstanciasMasivo';

            return $http.post(url, listainstanciaDto)
                .then(utilidades.httpRequestComplete, utilidades.httpRequestErrorReject)
                .then(cuandoSeCreaInstanciamasivo);

        }

        function cuandoSeCreaInstanciamasivo(data) {
           // estadoAplicacionServicios.emitirEvento(constantesBackbone.eventoInstanciaCreada, listainstanciaDto[0].TipoObjetoId);
            return data;
        }

        function cuandoSeCreaInstancia(data) {
            estadoAplicacionServicios.emitirEvento(constantesBackbone.eventoInstanciaCreada, instanciaDto.TipoObjetoId);
            return data;
        }
       


        function obtenerProyectosPorEntidadesyEstados(entidades, idOpcionDNP, estados, sectorId, entidadId, codigoBpin, proyectoNombre, tieneInstancias, flujoid, tipoTramiteId) {
            estados = estados || proyectosEstados.split('|');
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiFlujosProyectos;
            //var url = apiMotorFlujos + 'api/Flujos/Proyectos';
            var configuracion = {
                IdsEntidades: entidades,
                NombresEstadosProyectos: estados,
                SectorId: sectorId,
                EntidadId: entidadId,
                CodigoBpin: codigoBpin,
                ProyectoNombre: proyectoNombre,
                IdUsuarioDNP: usuarioDNP,
                IdsRoles: sesionServicios.obtenerUsuarioIdsRoles(),
                IdTipoObjetoNegocio: idTipoProyecto,
                TieneInstancias: tieneInstancias,
                IdOpcionDNP: idOpcionDNP,
                flujoid: flujoid,
                tipoTramiteId: tipoTramiteId
            }
            
            return $http.post(url, configuracion)
                .then(utilidades.httpRequestComplete);
        }

        function obtenerProyectosPorEntidadesyEstadosSGR(entidades, idOpcionDNP, estados, sectorId, entidadId, codigoBpin, proyectoNombre, tieneInstancias, flujoid, tipoTramiteId, IdUsuarioDNP) {
            estados = estados || proyectosEstados.split('|');
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiFlujosProyectos;
            //var url = apiMotorFlujos + 'api/Flujos/Proyectos';
            var configuracion = {
                IdsEntidades: entidades,
                NombresEstadosProyectos: estados,
                SectorId: sectorId,
                EntidadId: entidadId,
                CodigoBpin: codigoBpin,
                ProyectoNombre: proyectoNombre,
                IdUsuarioDNP: usuarioDNP,
                IdsRoles: sesionServicios.obtenerUsuarioIdsRoles(),
                IdTipoObjetoNegocio: idTipoProyecto,
                TieneInstancias: tieneInstancias,
                IdOpcionDNP: idOpcionDNP,
                flujoid: flujoid,
                tipoTramiteId: tipoTramiteId,
                IdUsuarioDNP: IdUsuarioDNP,
                tipoEntidad: ''
            }

            return $http.post(url, configuracion)
                .then(utilidades.httpRequestComplete);
        }

        function obtenerProyectosSGRApriorizar(usuario) {
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneSGR_ProyectosPriorizar + "?IdUsuarioDNP=" + usuario);
        }

        function obtenerLogInstancia(parametros) {
            var url = apiBackboneServicioBaseUri + 'api/Flujos/ObtenerLogInstancia';

            var parametrosDto = {
                Codigo: parametros.Codigo,
                FechaInicio: obtenerFechaHoraInicio(parametros.FechaInicio),
                FechaFin: obtenerFechaHoraTermino(parametros.FechaFin),
                BPIN: parametros.BPIN,
                Descripcion: parametros.Descripcion,
                Estado: parametros.Estado,
                UsuarioId: parametros.UsuarioId,
                EntityTypeCatalogOptionId: parametros.EntityTypeCatalogOptionId,
                EsTramite: parametros.esTramite
            }

            return $http.post(url, parametrosDto)
                .then(utilidades.httpRequestComplete);
        }

        function obtenerFechaHoraInicio(fecha) {
            if (fecha) {
                return new Date(fecha.getFullYear(), fecha.getMonth(), fecha.getDate(), 0, 0, 0);
            }
            return null;
        }

        function obtenerFechaHoraTermino(fecha) {
            if (fecha) {
                return new Date(fecha.getFullYear(), fecha.getMonth(), fecha.getDate(), 23, 59, 59);
            }
            return null;
        }

        function obtenerPdfLogInstancias(logFiltros) {
            var url = urlPDFBackbone + constantesBackbone.apiBackbonePDFInstanciasLog;
            return $http.post(url, logFiltros, { responseType: 'blob' });
        }

        function obtenerEntidades(tipoEntidad) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerEntidadPorTipoEntidad + tipoEntidad;
            return $http.get(url);
        }


        function ExisteFlujoProgramacion(entidadId, flujoId) {
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneExisteFlujoProgramacion + "?entidadId=" + entidadId + "&flujoId=" + flujoId);
        }

        function Flujos_SubPasoEjecutar(data) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiFlujosSubPasoEjecutar, data);
        }

        function Flujos_SubPasosValidar(idInstancia, idAccion, usuario) {
            return $http.get(apiBackboneServicioBaseUri + constantesBackbone.apiFlujosSubPasosValidar + "?idInstancia=" + idInstancia + "&idAccion=" + idAccion + "&usuario=" + usuario);
        }

        function TieneInstanciaActiva(objetoNegocioId) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneTieneInstanciaActiva + "?ObjetoNegocioId=" + objetoNegocioId;
            return $http.get(url);
        }

        function GuardarPermisosPriorizionProyectoDetalleSGR(model) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSGR_GuardarPermisosPriorizionProyectoDetalleSGR, model);
        }
        function GuardarProyectoPermisosProcesoSGR(model) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneSGR_Proyectos_GuardarProyectoPermisosProcesoSGR, model);
        }

    } 

})(apiFlujosServiciosBaseUri, apiMotorFlujos, nombreAplicacionBackbone, proyectosEstados);