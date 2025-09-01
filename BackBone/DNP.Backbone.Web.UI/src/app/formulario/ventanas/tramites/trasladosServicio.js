(function () {
    'use strict';

    // ReSharper disable once UndeclaredGlobalVariableUsing
    angular.module('backbone').factory('trasladosServicio', trasladosServicio);

    trasladosServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];


    function trasladosServicio($q, $http, $location, constantesBackbone) {

        return {
            obtenerTramites: obtenerTramites,
            obtenerProyectosPorTramite: obtenerProyectosPorTramite,
            obtenerTramitesConsolaProcesos: obtenerTramitesConsolaProcesos,
            obtenerUsuariosPorEntidad: obtenerUsuariosPorEntidad,
            guardarProyectos: guardarProyectos,
            obtenerProyectosTramite: obtenerProyectosTramite,
            obtenerTipoDocumentoTramite: obtenerTipoDocumentoTramite,
            eliminarProyectoTramite: eliminarProyectoTramite,
            obtenerFuentesInformacionPresupuestal: obtenerFuentesInformacionPresupuestal,
            obtenerProyectoFuentePresupuestalPorTramite: obtenerProyectoFuentePresupuestalPorTramite,
            obtenerProyectoRequisitosPorTramite: obtenerProyectoRequisitosPorTramite,

            ActualizarInstanciaProyecto: ActualizarInstanciaProyecto,
            ActualizarValoresProyecto: ActualizarValoresProyecto,
            ValidarEnviarDatosTramite: ValidarEnviarDatosTramite,
            obtenerProyectosTramiteAprobacion: obtenerProyectosTramiteAprobacion,

            actualizarTramitesFuentesPresupuestales: actualizarTramitesFuentesPresupuestales,
            actualizarTramitesRequisitos: actualizarTramitesRequisitos,
            ObtenerTiposRequisito: ObtenerTiposRequisito,


            ObtenerFuentesTramiteProyectoAprobacion: ObtenerFuentesTramiteProyectoAprobacion,
            guardarFuentesTramiteProyectoAprobacion: guardarFuentesTramiteProyectoAprobacion,
            obtenerInstanciasPermiso: obtenerInstanciasPermiso,
            obtenerCodigoPresupuestal: obtenerCodigoPresupuestal,
            obtenerInstanciasActivasProyectos: obtenerInstanciasActivasProyectos,
            obtenerTarmitesPorProyectoEntidad: obtenerTarmitesPorProyectoEntidad,
            obtenerValoresProyectos: obtenerValoresProyectos,
            eliminarInstanciaProyectoTramite: eliminarInstanciaProyectoTramite,
            obtenerTarmitesEstadoCerrado: obtenerTarmitesEstadoCerrado,
            actualizarCodigoPresupuestal: actualizarCodigoPresupuestal,
            obtenerObservacionesPasoPadre: obtenerObservacionesPasoPadre,
            RegistrarPermisosAccionPorUsuario: RegistrarPermisosAccionPorUsuario,
            ValidarEnviarDatosTramiteAprobacion: ValidarEnviarDatosTramiteAprobacion,

            ObtenerEncabezado: ObtenerEncabezado,
            cargarFirma: cargarFirma,
            validarSiExisteFirmaUsuario: validarSiExisteFirmaUsuario,
            firmar: firmar,
            firmarTramite: firmarTramite,
            consultarCarta: consultarCarta,
            ReasignarRadicadoORFEO: ReasignarRadicadoORFEO,
            obtenerDetallesTramite: obtenerDetallesTramite,
            generarRadicadoSalida : generarRadicadoSalida,
            cerrar_CargarDocumentoElectronicoOrfeo : cerrar_CargarDocumentoElectronicoOrfeo,
            CrearReasignacionRadicadOrfeo: CrearReasignacionRadicadOrfeo,
            obtenerDatosUsuario: obtenerDatosUsuario,
            borrarFirma: borrarFirma,
            eliminarInstanciaCerrada_AbiertaProyectoTramite: eliminarInstanciaCerrada_AbiertaProyectoTramite,
            notificarUsuariosPorInstanciaPadre: notificarUsuariosPorInstanciaPadre
        };

        function obtenerTramites(peticionObtenerInbox, tramiteFiltro, columnasVisibles) {
            var listaFiltrados = [];
            Object.keys(tramiteFiltro).forEach(filtro => {
                if (tramiteFiltro[`${filtro}`].valor) {
                    listaFiltrados.push(tramiteFiltro[`${filtro}`]);
                }
            });

            const tramiteDto = {
                parametrosInboxDto: peticionObtenerInbox,
                tramiteFiltroDto: {
                    tokenAutorizacion: '',
                    idUsuarioDNP: peticionObtenerInbox.IdUsuario,
                    filtroGradeDtos: listaFiltrados,
                    IdsRoles: peticionObtenerInbox.ListaIdsRoles,
                    IdTipoObjetoNegocio: peticionObtenerInbox.IdObjeto,
                    IdsEtapas: peticionObtenerInbox.IdsEtapas,
                    InstanciaId: peticionObtenerInbox.IdInstancia
                },
                columnasVisibles: columnasVisibles,
                InstanciaId: peticionObtenerInbox.IdInstancia
            };

            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerTramite, tramiteDto);
        }

        function obtenerTramitesConsolaProcesos(peticionObtenerInbox, tramiteFiltro, columnasVisibles) {
            var listaFiltrados = [];
            Object.keys(tramiteFiltro).forEach(filtro => {
                if (tramiteFiltro[`${filtro}`].valor) {
                    listaFiltrados.push(tramiteFiltro[`${filtro}`]);
                }
            });

            const tramiteDto = {
                parametrosInboxDto: peticionObtenerInbox,
                tramiteFiltroDto: {
                    tokenAutorizacion: '',
                    idUsuarioDNP: peticionObtenerInbox.IdUsuario,
                    filtroGradeDtos: listaFiltrados,
                    IdsRoles: peticionObtenerInbox.ListaIdsRoles,
                    IdTipoObjetoNegocio: peticionObtenerInbox.IdObjeto,
                    IdsEtapas: peticionObtenerInbox.IdsEtapas
                },
                columnasVisibles: columnasVisibles
            };

            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerTramiteConsolaProcesos, tramiteDto);
        }

        function obtenerProyectosPorTramite(peticionObtenerInbox, tramiteId) {
            const tramiteDto = {
                parametrosInboxDto: peticionObtenerInbox,
                tramiteFiltroDto: {
                    tokenAutorizacion: '',
                    idUsuarioDNP: peticionObtenerInbox.IdUsuario,
                    tramiteId: tramiteId,
                    filtroGradeDtos: [],
                    InstanciaId: tramiteId
                },
                columnasVisibles: []
            };

            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectosPorTramite, tramiteDto);
        }


        function obtenerUsuariosPorEntidad(tipoEntidad, filtro, filtroExtra) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerUsuariosPorEntidad + tipoEntidad + '&filtro=' + filtro;
            var params = { filtroExtra: JSON.stringify(filtroExtra) };
            return $http.get(url, { /*data*/ params: params });
        }

        function guardarProyectos(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarProyectosTramiteNegocio;
            return $http.post(url, parametros);
        }


        function obtenerProyectosTramite(tramiteId) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneObtenerProyectosTramiteNegocio}${tramiteId}`;
            return $http.get(url);
        }

        function obtenerTipoDocumentoTramite(tipoTramiteId) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneObtenerTipoDocumentoTramite}${tipoTramiteId}`;
            return $http.get(url);
        }

        function eliminarProyectoTramite(peticionObtenerInbox, parametros) {
            const tramiteDto = {
                ParametrosInboxDto: peticionObtenerInbox,
                TramiteFiltroDto: {
                    tokenAutorizacion: '',
                    idUsuarioDNP: peticionObtenerInbox.IdUsuario,
                    tramiteId: parametros.TramiteId,
                    filtroGradeDtos: [],
                    InstanciaId: parametros.TramiteId,
                    ProyectoId: parametros.ProyectoId
                },
                columnasVisibles: []
            };

            //var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneEliminarInstanciaTramiteNegocio; 

            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneEliminarProyectoTramiteNegocio;
            return $http.post(url, tramiteDto);
        }

        function obtenerFuentesInformacionPresupuestal() {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneObtenerFuentesInformacionPresupuestal}`;
            return $http.get(url);
        }


        function ActualizarInstanciaProyecto(proyectoTramiteDto) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneActualizarInstanciaProyecto;
            return $http.post(url, proyectoTramiteDto);
        }

        function ActualizarValoresProyecto(proyectoTramiteDto) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneActualizarValoresProyecto;
            return $http.post(url, proyectoTramiteDto);
        }

        function obtenerProyectoFuentePresupuestalPorTramite(ProyectoId, TramiteId, TipoProyecto) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneObtenerProyectoFuentePresupuestalPorTramite}` + '?pProyectoId=' + ProyectoId + '&pTramiteId=' + TramiteId + '&pTipoProyecto=' + TipoProyecto;
            return $http.get(url);
        }

        function obtenerProyectoRequisitosPorTramite(ProyectoId, TramiteId) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneObtenerProyectoRequisitosPorTramite}` + '?pProyectoId=' + ProyectoId + '&pTramiteId=' + TramiteId + '&isCDP='+ true;
            return $http.get(url);
        }


        function obtenerProyectosTramiteAprobacion(tramiteId, tipoRolId) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneObtenerProyectosTramiteNegocioAprobacion}` + '?tramiteId=' + tramiteId + '&tipoRolId=' + tipoRolId;
            return $http.get(url);

        }

        function ValidarEnviarDatosTramite(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneValidarEnviarDatosTramiteNegocio;
            return $http.post(url, parametros);
        }

        function actualizarTramitesFuentesPresupuestales(TramiteFuentesPresupuestalesDto) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneActualizarTramitesFuentesPresupuestales;
            return $http.post(url, TramiteFuentesPresupuestalesDto);
        }

        function actualizarTramitesRequisitos(TramiteRequisitosDto) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneActualizarTramitesRequisitos;
            return $http.post(url, TramiteRequisitosDto);
        }

        function ObtenerTiposRequisito() {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneObtenerTiposRequisito}`;
            return $http.get(url);
        }

        function ObtenerFuentesTramiteProyectoAprobacion(tramiteId, proyectoId, pTipoProyecto) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneObtenerFuentesTramiteProyectoAprobacion}` + '?tramiteId=' + tramiteId + '&proyectoId=' + proyectoId + '&pTipoProyecto=' + pTipoProyecto;
            return $http.get(url);
        }

        function guardarFuentesTramiteProyectoAprobacion(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarFuentesTramiteProyectoAprobacion;
            return $http.post(url, parametros);
        }


        function obtenerInstanciasPermiso(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerInstanciasPermiso;
            return $http.post(url, parametros);

        }

        function obtenerCodigoPresupuestal(tramiteId, proyectoId, entidadId) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneObtenerCodigoPresupuestal}` + '?tramiteId=' + tramiteId + '&proyectoId=' + proyectoId + '&entidadId=' + entidadId;
            return $http.get(url);

        }

        function obtenerInstanciasActivasProyectos(Bpins) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneObtenerInstanciasActivasProyectos}` + '?Bpins=' + Bpins;
            return $http.get(url);
        }

        function obtenerTarmitesPorProyectoEntidad(entidadId, proyectoId) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.uriobtenerTarmitesPorProyectoEntidad}` + '?proyectoId=' + proyectoId + '&entidadId=' + entidadId;
            return $http.get(url);
        }

        function eliminarInstanciaProyectoTramite(instanciaTramite, Bpin) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneEliminarInstanciaProyectoTramite}` + '?instanciaTramite=' + instanciaTramite + '&Bpin=' + Bpin;
            return $http.post(url);
        }

        function obtenerTarmitesEstadoCerrado(entidadId, proyectoId) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneObtenerTarmitesEstadoCerrado}` + '?proyectoId=' + proyectoId + '&entidadId=' + entidadId;
            return $http.get(url);
        }

        function obtenerValoresProyectos(tramiteId, proyectoId, entidadId) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneObtenerValoresProyectos}` + '?proyectoId=' + proyectoId + '&tramiteId=' + tramiteId + '&entidadId=' + entidadId;
            return $http.get(url);
        }

        function actualizarCodigoPresupuestal(tramiteId, proyectoId, entidadId) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneActualizarCodigoPresupuestal}` + '?tramiteId=' + tramiteId + '&proyectoId=' + proyectoId + '&entidadId=' + entidadId;
            return $http.post(url);
        }

        function obtenerObservacionesPasoPadre(idInstancia, idAccion) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerObservacionesPasoPadre + "?idInstancia=" + idInstancia + "&idAccion=" + idAccion);
        }

        function RegistrarPermisosAccionPorUsuario(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneFlujoRegistrarPermisosAccionPorUsuario;
            return $http.post(url, parametros);
        }

        function ValidarEnviarDatosTramiteAprobacion(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneValidarEnviarDatosTramiteNegocioAprobacion;
            return $http.post(url, parametros);
        }

        function ObtenerEncabezado(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerEncabezadoGeneral;
            return $http.post(url, parametros);
        }

        function cargarFirma(firma, rol) {
            const parametro = {
                FileAsBase64: firma,
                RolId: rol
            }
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackBoneCargarFirma}`;
            return $http.post(url, parametro );
        }

        function validarSiExisteFirmaUsuario() {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackBoneValidarSiExisteFirmaUsuario}`;
            return $http.post(url);
        }


        function firmarTramite(tramiteId) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackBoneTramitesFirmar}` + '?tramiteId=' + tramiteId;
            return $http.post(url);
           
        }

        function firmar(tramiteId, numeroRadicado) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackBoneFirmar}` + '?tramiteId=' + tramiteId + '&numeroRadicado=' + numeroRadicado;
            return $http.post(url);

        }


        function consultarCarta(tramiteId) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackBoneConsultarCarta}` + '?tramiteId=' + tramiteId;
            return $http.get(url);
        }

        function ReasignarRadicadoORFEO(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneReasignarRadicado;
            return $http.post(url, parametros);
        }

        function obtenerDetallesTramite(numerotramite) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackBoneObtenerDetallesTramite}` + '?numeroTramite=' + numerotramite;
            return $http.get(url);
        }

        function obtenerProyectosPorTramite(instanciaId) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackBoneObtenerProyectosPorTramite}` + '?instanciaId=' + instanciaId;
            return $http.get(url);
        }

        function generarRadicadoSalida(numeroTramite, numeroRadicado) {
            const parametros = {
                numeroRadicado : numeroTramite,
                numeroRadicadoSalida : numeroRadicado
            }
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackBoneTramitesGenerarRadicado}` + '?numeroTramite=' + numeroTramite + '&numeroRadicadoSalida=' + numeroRadicado;
            return $http.get(url, parametros);
        }

        function cerrar_CargarDocumentoElectronicoOrfeo(parametros) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackBoneTramitesCerrar_CargarDocumentoElectronicoOrfeo}`;
            return $http.post(url, parametros);
        }

        function CrearReasignacionRadicadOrfeo(usuarioIdOrigen, usuarioIdDestino, tramiteId) {
            const UsuarioOrigen = {
                Login: usuarioIdOrigen
            };

            const UsuarioDestino = {
                Login: usuarioIdDestino
            };

            const ReasignacionRadicadoDto = {
                UsuarioOrigen: UsuarioOrigen,
                UsuarioDestino: UsuarioDestino,
                TramiteId: tramiteId,
            };

            ReasignarRadicadoORFEO(ReasignacionRadicadoDto).then(function (response) { }, (err) => {
                swal('', "Error al realizar la reasignación", 'error'); 
            });
        }

        function obtenerDatosUsuario(idUsuarioDnp, idEntidad, idAccion, idIntancia) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneObtenerDatosUsuarioTramite}` +
                '?idUsuarioDnp=' + idUsuarioDnp + '&idEntidad=' + idEntidad + 
                '&idAccion=' + idAccion + '&idIntancia=' + idIntancia;
            return $http.get(url);
        }

        function borrarFirma(rol) {
            const parametro = {
                RolId: rol
            }
             var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackBoneBorrarFirma}`;
            return $http.post(url, parametro);
        }

        function eliminarInstanciaCerrada_AbiertaProyectoTramite(instanciaTramite, Bpin) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneEliminarInstanciaCerrada_AbiertaProyectoTramite}` + '?instanciaTramite=' + instanciaTramite + '&Bpin=' + Bpin;
            return $http.post(url);
        }
        
        function notificarUsuariosPorInstanciaPadre(instanciaId, nombreNotificacion, texto) {
            var url = `${apiBackboneServicioBaseUri}${constantesBackbone.apiBackboneNotificarUsuariosPorInstanciaPadre}` + '?instanciaId=' + instanciaTramite + '&nombreNotificacion=' + nombreNotificacion + '&texto=' + texto;
            return $http.post(url);
        }
    }
})()