(function () {
    'use strict';

    angular.module('backbone').factory('validacionArchivosServicio', validacionArchivosServicio);
    validacionArchivosServicio.$inject = ['$sessionStorage', 'documentoSoporteServicios', 'utilidades'];

    function validacionArchivosServicio($sessionStorage, documentoSoporteServicios, utilidades) {
        return {
            validarArchivosAdjuntos
        };

        async function validarArchivosAdjuntos(resDat, sectionSe, idRolSe, nivelarchivoSe, idtipotramitepresupuestalSe, seccionCap, reqEspecial) {
            try {
                const idTramite = $sessionStorage.tramiteId || 0;
                const tipoTramiteId = idtipotramitepresupuestalSe || $sessionStorage.tipoTramiteId;
                let parametros;

                if (reqEspecial == 'avaluso')
                    parametros = obtenerParametrosConsultaAvalUso(nivelarchivoSe);
                else
                    parametros = obtenerParametrosConsulta(nivelarchivoSe, sectionSe, idRolSe);

                const [
                    listaTipoArchivosObligatorios,
                    listaTipoDocumentoUnicoValidacion,
                    response
                ] = await Promise.all([
                    obtenerArchivosObligatorios(tipoTramiteId, idTramite),
                    utilidades.obtenerParametroTransversal('SGR_DocumentosUnicosValidacionSoporte'),
                    documentoSoporteServicios.ObtenerListadoArchivosPIIP(parametros, "tramites")
                ]);

                if (!response || typeof response === 'string')
                    return resDat;

                return validarArchivosFaltantes(resDat, listaTipoArchivosObligatorios, response, seccionCap, reqEspecial, listaTipoDocumentoUnicoValidacion);
            } catch (error) {
                console.error("Error al validar documentos soporte:", error);
                throw error;
            }
        }

        function obtenerParametrosConsultaAvalUso(nivelarchivoSe) {
            return ($sessionStorage.idNivel === nivelarchivoSe) ?
                { idInstancia: $sessionStorage.idInstancia, idNivel: $sessionStorage.idNivel } :
                { idNivel: $sessionStorage.idNivel, idInstancia: $sessionStorage.idInstancia, idaccion: $sessionStorage.idAccion, tipodocumentocodigo: 'AxU' };
        }

        function obtenerParametrosConsulta(nivelarchivoSe, sectionSe, idRolSe) {
            return ($sessionStorage.idNivel === nivelarchivoSe) ?
                { idInstancia: $sessionStorage.idInstancia, idNivel: $sessionStorage.idNivel } :
                { idInstancia: $sessionStorage.idInstancia, section: sectionSe, idAccion: $sessionStorage.idAccion, idNivel: $sessionStorage.idNivel/*, idRol: idRolSe*/ };
        }

        async function obtenerArchivosObligatorios(tipoTramiteId, idTramite) {
            const { data } = await documentoSoporteServicios.ObtenerListaTipoDocumentosSoportePorRolTrv(tipoTramiteId, "A", idTramite, $sessionStorage.idNivel, $sessionStorage.idInstancia, $sessionStorage.idAccion);

            if (!data)
                return [];

            const lista = JSON.parse(data);
            return lista.filter(a => a.Obligatorio);
        }

        function validarArchivosFaltantes(resDat, oblig, resp, seccionCap, reqEspecial, unicos) {
            const cargados = new Set((resp || [])
                .filter(a => a && a.metadatos && a.status !== 'Eliminado')
                .map(a => a.metadatos.tipodocumentoid));
            const grupo = new Set(String(unicos || '').split(',')
                .map(s => parseInt(s, 10)).filter(Number.isFinite));
            const alguno = tieneInterseccion(grupo, cargados);
            const faltantes = (oblig || [])
                .filter(o => !cargados.has(o.TipoDocumentoId) && !(alguno && grupo.has(o.TipoDocumentoId)))
                .map(o => o.TipoDocumento);

            if (faltantes.length)
                agregarErroresValidacion(resDat, faltantes, seccionCap, reqEspecial);

            return resDat;
        }

        function tieneInterseccion(setA, setB) {
            for (const v of setA) {
                if (setB.has(v)) return true;
            }
            return false;
        }

        function agregarErroresValidacion(resDat, listaArchivosFaltantes, seccionCap, reqEspecial) {
            let MeError;
            const capitulo = 'alojararchivo';

            if (reqEspecial == 'avaluso')
                MeError = 'SGRAVAL2';
            else
                MeError = 'SGRVDP1';

            const mensajeError = `El/Los documento(s) ${listaArchivosFaltantes.join(', ')} no ha(n) sido cargado(s), por favor adjuntarlo(s) para continuar.`;
            const errorValidacion = JSON.stringify({
                [seccionCap + capitulo]: [{
                    'Error': MeError,
                    'Descripcion': mensajeError,
                    'Completo': false
                }]
            });

            const validacionArchivos = resDat.find(p => p.Seccion === seccionCap);

            if (validacionArchivos)
                validacionArchivos.Errores = errorValidacion;
            else {
                resDat.push({
                    'Seccion': seccionCap,
                    'Capitulo': capitulo,
                    'Errores': errorValidacion
                });
            }
        }
    }
})();